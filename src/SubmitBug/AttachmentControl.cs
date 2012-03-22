// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.SubmitBug
{
	public class AttachmentControl : Control
	{
		private readonly List<string> attachedFiles = new List<string>();

		public IEnumerable<string> AttachedFiles
		{
			get { return new List<string>(attachedFiles); }
		}

		private readonly AttachmentControlToolButton addFromLib =
			new AttachmentControlToolButton("Add new picture from editor");

		private readonly AttachmentControlToolButton addFile = new AttachmentControlToolButton("Add file");
		private readonly AttachmentControlToolButton newCaptrue = new AttachmentControlToolButton("Add New snapshot");

		private readonly AttachmentControlToolButton pageUp = new AttachmentControlToolButton("");
		private readonly AttachmentControlToolButton pageDown = new AttachmentControlToolButton("");

		private int attachmentOffset;

		public event EventHandler<EventArgs> CaptureAnotherPicture = delegate { };
		public event EventHandler<EventArgs> AddAnotherPicture = delegate { };
		public event EventHandler<EventArgs> DeleteFileCliked = delegate { };

		public AttachmentControl()
		{
			addFromLib.Click += addFromLib_Click;
			addFromLib.ViewNormal = Resources.send_bug;
			addFromLib.Size = new Size(40, 40);
			Controls.Add(addFromLib);

			addFile.Click += addFile_Click;
			addFile.ViewNormal = Resources.save;
			addFile.Size = new Size(40, 40);
			Controls.Add(addFile);

			newCaptrue.Click += newCaptrue_Click;
			newCaptrue.ViewNormal = Resources.new_capture;
			newCaptrue.Size = new Size(40, 40);
			Controls.Add(newCaptrue);

			pageUp.Click += pageUp_Click;
			pageUp.ViewNormal = Resources.scrollup;
			Controls.Add(pageUp);

			pageDown.Click += pageDown_Click;
			pageDown.ViewNormal = Resources.scrolldown;
			Controls.Add(pageDown);
		}


		private void newCaptrue_Click(object sender, EventArgs e)
		{
			CaptureAnotherPicture(this, e);
		}

		public void AddFile(string fileName)
		{
			attachedFiles.Add(fileName);
			ShowFiles();
		}

		public void DeleteFiles()
		{
			attachedFiles.Clear();
			attachmentOffset = 0;
			ShowFiles();
		}

		private void addFromLib_Click(object sender, EventArgs e)
		{
			AddAnotherPicture(this, e);
		}

		private void addFile_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				attachedFiles.Add(dialog.FileName);
				ShowFiles();
			}
		}

		private void pageDown_Click(object sender, EventArgs e)
		{
			if (attachmentOffset > 0)
				attachmentOffset--;
			ShowFiles();
			Update();
			Invalidate();
		}

		private void pageUp_Click(object sender, EventArgs e)
		{
			if (attachmentOffset + 2 < attachedFiles.Count)
				attachmentOffset++;
			ShowFiles();
			Update();
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			addFromLib.Location = new Point(0, Height - 40);
			addFile.Location = new Point(40, Height - 40);
			newCaptrue.Location = new Point(80, Height - 40);
			pageUp.Location = new Point(0, 0);
			pageUp.Size = new Size(Width, 25);
			pageDown.Location = new Point(0, Height - 65);
			pageDown.Size = new Size(Width, 25);
		}

		private void ShowFiles()
		{
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				if (Controls[i] is AttachmentControlItem)
				{
					Controls.RemoveAt(i);
				}
			}

			int offset = 24;
			int count = 2;
			int countOffs = attachmentOffset;
			foreach (string file in attachedFiles)
			{
				if (countOffs == 0)
				{
					var ac = new AttachmentControlItem(file);
					ac.DeleteClicked += ac_DeleteClicked;
					ac.Location = new Point(0, offset);
					Controls.Add(ac);
					offset += ac.Height;
					count--;
				}
				else
				{
					countOffs--;
				}

				if (count == 0)
					break;
			}
		}

		private void ac_DeleteClicked(object sender, EventArgs e)
		{
			var aci = (AttachmentControlItem) sender;
			attachedFiles.Remove(aci.fileName);
			if (attachmentOffset + 2 > attachedFiles.Count)
			{
				attachmentOffset = attachedFiles.Count - 2;
			}
			if (attachmentOffset < 0)
				attachmentOffset = 0;
			ShowFiles();
			Update();
			Invalidate();

			DeleteFileCliked(aci.fileName, null);
		}
	}
}