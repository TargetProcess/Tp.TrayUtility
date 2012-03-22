// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using Tp.BugServiceProxy;
using Tp.ProjectServiceProxy;
using Tp.Service.Proxies.FileTransfer;
using Tp.UserStoryServiceProxy;
using TpTrayUtility.Components;
using TpTrayUtility.Components.TpSecureForms;
using TpTrayUtility.SubmitBug;
using TpTrayUtility.Events;
using CreateCompletedEventArgs = Tp.BugServiceProxy.CreateCompletedEventArgs;
using PriorityDTO = Tp.BugServiceProxy.PriorityDTO;

namespace TpTrayUtility
{
	public partial class SubmitBugForm : TpSecureForm
	{
		#region Fields

		private readonly List<string> filesToDeleteAfterSubmit = new List<string>();

		private ProjectDTO[] projects;
		private UserStoryDTO[] _stories;		
		private UserStoryDTO[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				lock (this)
				{					
					_stories = value;
				}
			}
		}
		private SeverityDTO[] severities;
		private PriorityDTO[] priorities;
		private BugService bugServiceWse;
		private BugDTO bugDTO;
		private string lastProjectForStoriesName;

		private AttachmentControl attachmentControl;

		private readonly ToolTip fullPathToolTip = new ToolTip();

		private bool openBrowserAfterSubmit;

		private Queue<string> submitingFiles;
		private int submitedBugID;
		private Func<int, UserStoryDTO[]> RetrieveUserStoriesHandler;

		private UserStoryService _userStoryService;
		public EventHandler BugBeforePost;
		public EventHandler<MessageEventArgs> BugPostingAction;
		public EventHandler BugPosted;

		#endregion

		public SubmitBugForm()
		{
			TpAuthenticationManager.Instance.OnChangeLoginEvent += InitUserStoryService;
		}

		#region Methods

		public override void Init()
		{
			InitUserStoryService();

			InitAll();
		}

		private void InitUserStoryService()
		{
			RetrieveUserStoriesHandler = new Func<int, UserStoryDTO[]>(RetrieveUserStories);
			try
			{
				_userStoryService = ServiceManager.GetService<UserStoryService>();
			}
			catch (SoapException)
			{
				Messenger.ShowIncorrectVersionError();
			}
			
		}

		public static void ShellExecute(string cmd, string path)
		{
			var psi = new ProcessStartInfo { UseShellExecute = true, FileName = path, CreateNoWindow = true, Verb = "open" };
			Process.Start(psi);
		}

		internal void AddFile(string fileName, bool isTemporary)
		{
			attachmentControl.AddFile(fileName);
			if (isTemporary)
				filesToDeleteAfterSubmit.Add(fileName);
		}

		private void InitAll()
		{
			fullPathToolTip.AutoPopDelay = 5000;
			fullPathToolTip.InitialDelay = 1000;
			fullPathToolTip.ReshowDelay = 500;
			fullPathToolTip.ShowAlways = true;

			InitializeComponent();

			attachmentControl = new AttachmentControl { Size = new Size(210, Height - 34), Location = new Point(0, 34) };
			attachmentControl.DeleteFileCliked += attachmentControl_DeleteFileCliked;
			attachmentControl.CaptureAnotherPicture += attachmentControl_CaptureAnotherPicture;
			attachmentControl.AddAnotherPicture += attachmentControl_AddAnotherPicture;
			TpAuthenticationManager.Instance.OnChangeLoginEvent += CancelAndRefresh;

			Controls.Add(attachmentControl);

			InitDropDowns();
		}

		private void InitDropDowns()
		{
			FillProjectData();
			PrepareBugData();
		}

		private void CancelAndRefresh()
		{
			ClearControls();
			InitDropDowns();
			CancelForm();
		}

		private void DeleteFile(string fName)
		{
			if (!filesToDeleteAfterSubmit.Contains(fName)) return;

			try
			{
				File.Delete(fName);
			}
			catch
			{
				MessageBox.Show(@"Could not delete " + fName);
			}
		}

		private void Close(bool removeFiles)
		{
			if (removeFiles)
			{
				filesToDeleteAfterSubmit.ForEach(DeleteFile);
			}
		}

		private static T GetByName<T>(IEnumerable<T> objects, string name) where T : class
		{
			if (name == null || objects == null)
				return null;
			//used reflection because Generic don't supports something....
			return objects.FirstOrDefault(obj => (string) (obj.GetType().GetProperty("Name").GetValue(obj, null)) == name);
		}

		private void SelectProject(string projectName)
		{
			comboBoxUserStory.Clear();
			var project = GetByName(projects, projectName);
			SettingsManager.LastProject = project != null ? projectName : string.Empty;
		}

		private void FillProjectData()
		{
			var projectServiceWse = ServiceManager.GetService<ProjectService>();

			try
			{
				projects =
				projectServiceWse.Retrieve(
					"select p from Project as p where p.DeleteDate is null and p.Process.ProcessID in (select pp.Process.ProcessID from ProcessPractice as pp where pp.Practice.PracticeID=3)",
					new object[] { });
			}
			catch (SoapException)
			{
				Messenger.ShowIncorrectVersionError();
				projects = new ProjectDTO[]{};
			}
			

			comboBoxProject.Clear();
			foreach (var dto in projects)
				comboBoxProject.AddItem(dto.Name);

			comboBoxProject.SortByName();

			SelectProject(SettingsManager.LastProject);
			comboBoxProject.SelectItem(SettingsManager.LastProject);
		}

		private void PrepareBugData()
		{
			bugServiceWse = ServiceManager.GetService<BugService>();

			comboBoxSeverity.Clear();
			comboBoxPriority.Clear();

			try
			{
				severities = bugServiceWse.GetSeverities();
				priorities = bugServiceWse.GetPriorities();
			}
			catch (SoapException)
			{
				Messenger.ShowIncorrectVersionError();
				severities = new SeverityDTO[]{};
				priorities = new PriorityDTO[]{};
			}
			
			
			foreach (SeverityDTO severity in severities)
				comboBoxSeverity.AddItem(severity.Name);
			
			comboBoxSeverity.SelectedIndex = comboBoxSeverity.Items.Count / 2;
			
			foreach (PriorityDTO property in priorities)
				comboBoxPriority.AddItem(property.Name);

	        comboBoxPriority.SelectedIndex = comboBoxPriority.Items.Count / 2;
		}

		private void SubmitBug()
		{
			if (string.IsNullOrEmpty(textBoxBugName.Text.Trim()))
			{
				MessageBox.Show(@"Bug Name should not be empty.");
				return;
			}

			ProjectDTO project = null;
			if (comboBoxProject.SelectedItem != null)
				project = GetByName(projects, comboBoxProject.SelectedItemText);

			if (project == null)
			{
				MessageBox.Show(@"Please select a project to add the bug.");
				return;
			}

			
			FormBug(project);

			Cursor = Cursors.WaitCursor;
			buttonSubmit.Enabled = false;
			PostBug();
		}

		private void PostBug()
		{
			TpSecureFormManager.CheckAuthentication(Close);

			//Text = @"Submitting Bug";
			var status = new MessageForm(this);
			status.Closed += (sender, e) => Close();
			status.Show(this);
			this.Enabled = false;

			if (BugBeforePost != null)
				BugBeforePost(this, EventArgs.Empty);
			bugServiceWse.CreateAsync(bugDTO);
			bugServiceWse.CreateCompleted += postBug_Completed;
			bugServiceWse.AddAttachmentToBugCompleted += bugServiceWse_AddAttachmentToBugCompleted;
		}

		
		private void PostAttachments()
		{
			if (submitingFiles.Count > 0)
			{
				var upload =
					new FileTransferUpload(TpAuthenticationManager.Instance.IsIntegrated, TpAuthenticationManager.Instance.UserName,
										   TpAuthenticationManager.Instance.Password);
				upload.WebService.Url = SettingsManager.TargetProcessPath + "Services/FileService.asmx";
				upload.AutoSetChunkSize = true;
				upload.LocalFilePath = submitingFiles.Dequeue();
				upload.ProgressChanged += upload_ProgressChanged;
				upload.RunWorkerCompleted += fileUpload_Completed;
				upload.RunWorkerAsync();
			}
			else
			{
				if (openBrowserAfterSubmit)
				{
					string url = SettingsManager.TargetProcessPath + "Project/QA/Bug/View.aspx?BugID=" + submitedBugID + "&ProjectID=" +
								 bugDTO.ProjectID;
					ShellExecute("open", url);
				}
				
				if (BugPosted != null)
					BugPosted(this, EventArgs.Empty);

				DialogResult = DialogResult.OK;				
				//Close();
			}
		}
		

		private void FormBug(ProjectDTO project)
		{
			bugDTO = new BugDTO
			{
				Name = textBoxBugName.Text,
				Description = textBoxBugDescription.Text.Replace("\r\n", "<br/>"),
				ProjectID = project.ID
			};

			var severity = GetByName(severities, comboBoxSeverity.SelectedItemText);
			if (severity != null)
				bugDTO.SeverityID = severity.ID;

			var priority = GetByName(priorities, comboBoxPriority.SelectedItemText);
			if (priority != null)
				bugDTO.PriorityID = priority.ID;

			var story = GetByName(Stories, comboBoxUserStory.SelectedItemText);
			if (story != null)
				bugDTO.UserStoryID = story.ID;

			bugDTO.Effort = 0;
		}

		#endregion

		#region Events

		protected override void OnClosing(CancelEventArgs e)
		{
			Close(true);
			base.OnClosing(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			e.Graphics.DrawLine(Pens.Black, 0, 33, Width, 33);
			e.Graphics.DrawLine(Pens.Black, 210, 33, 210, Height);
		}

		private static void attachmentControl_AddAnotherPicture(object sender, EventArgs e)
		{
			FormsManager.Show(typeof(SimpleEditor));
		}

		private static void attachmentControl_CaptureAnotherPicture(object sender, EventArgs e)
		{
			FormsManager.HideForms();
		}

		private void attachmentControl_DeleteFileCliked(object sender, EventArgs e)
		{
			DeleteFile((string) (sender));
		}

		private void buttonSubmit_Click(object sender, EventArgs e)
		{
			openBrowserAfterSubmit = false;
			SubmitBug();
		}

		private void buttonSubmitView_Click(object sender, EventArgs e)
		{
			openBrowserAfterSubmit = true;
			SubmitBug();
		}

		private void postBug_Completed(object sender, CreateCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				MessageBox.Show(e.Error.Message);
				Cursor = Cursors.Default;
				buttonSubmit.Enabled = true;
				return;
			}
			submitedBugID = e.Result;
			submitingFiles = new Queue<string>(attachmentControl.AttachedFiles);

			PostAttachments();
		}

		private void fileUpload_Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				Cursor = Cursors.Default;
				buttonSubmit.Enabled = true;
				MessageBox.Show(e.Error.Message);
			}
			else
			{
				var uploader = (sender as FileTransferUpload);
				if (uploader == null) return;

				bugServiceWse.AddAttachmentToBugAsync(submitedBugID, Path.GetFileName(uploader.LocalFilePath), string.Empty,
													  Guid.NewGuid());
			}
		}

		private void bugServiceWse_AddAttachmentToBugCompleted(object sender, AddAttachmentToBugCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				Cursor = Cursors.Default;
				buttonSubmit.Enabled = true;
				MessageBox.Show(e.Error.Message);
			}
			else
			{
				PostAttachments(); //Recursive!
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			CancelForm();
		}

		private void CancelForm()
		{
			attachmentControl.DeleteFiles();
			Close(true);
			FormsManager.Show(typeof(SimpleEditor));
		}

		private void ClearControls()
		{
			textBoxBugName.Text = string.Empty;
			textBoxBugDescription.Text = string.Empty;
		}

		private void comboBoxProject_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectProject(comboBoxProject.SelectedItemText);
			comboBoxUserStory.Clear();
			//comboBoxUserStory.Update();
						
		}

		private void upload_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if(BugPostingAction != null)
				BugPostingAction(this, new MessageEventArgs(@"Submitting attachment: " + e.UserState));			
			Update();
			Invalidate();
		}

		private void comboBoxUserStory_DropDownOpened(object sender, EventArgs e)
		{

			if (!comboBoxUserStory.IsLoadingEntities && comboBoxProject.SelectedItem != null && lastProjectForStoriesName != comboBoxProject.SelectedItemText)
			{
				lastProjectForStoriesName = comboBoxProject.SelectedItemText;
				var project = GetByName(projects, comboBoxProject.SelectedItemText);
				if (project != null)
				{
					comboBoxUserStory.IsLoadingEntities = true;
					var selectedProject =comboBoxProject.SelectedItemText;
					RetrieveUserStoriesHandler.BeginInvoke(project.ID.Value, RetrieveUserStoriesCallback, selectedProject);					
				}
				else
					comboBoxUserStory.Update();
			}
		}

		delegate void UserStoryDTOParameterDelegate(UserStoryDTO[] value);

		private void UpdateUserStories(UserStoryDTO[] stories)
		{			
			if (InvokeRequired)
			{
				BeginInvoke(new UserStoryDTOParameterDelegate(UpdateUserStories), new object[]{stories});
				return;
			}
			comboBoxUserStory.IsLoadingEntities = false;			
			comboBoxUserStory.AddItem(String.Empty);
			comboBoxUserStory.SelectItem(String.Empty); 
			stories.ForEach(story =>comboBoxUserStory.AddItem(story.Name));
			comboBoxUserStory.SortByName();
			comboBoxUserStory.Update();
			Stories = stories;
		}

		private void RetrieveUserStoriesCallback(IAsyncResult ar)
		{
			var stories = RetrieveUserStoriesHandler.EndInvoke(ar);
			if (ar.AsyncState.ToString() != lastProjectForStoriesName)
			{
				comboBoxUserStory.IsLoadingEntities = false;
				return;
			}
			UpdateUserStories(stories);
		}

		private UserStoryDTO[] RetrieveUserStories(int projectId)
		{
			try
			{
				return _userStoryService.RetrieveNames(projectId);
			}
			catch (Exception)
			{
				Invoke(new EventHandler(delegate { ShowErrorMessage(); }));
				
				return new UserStoryDTO[] { };
			}
		}

		private void ShowErrorMessage()
		{
			FormsManager.DisableForms();
			if(Messenger.ShowIncorrectVersionError() == DialogResult.OK)
			{
				FormsManager.EnableForms();
			}
		}
		#endregion
	}
}