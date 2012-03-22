//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Tp.MyAssignmentsServiceProxy;

namespace TpTrayUtility.Components
{
	public class CompletedEventArgs : EventArgs
	{
		public MyAssignments Assignments { get; set; }

		public Exception Exception { get; set; }
	}

	public delegate void OnAssignmentsRetrievedHandler(object sender, CompletedEventArgs ex);

	public interface IAssignmentsRetriever
	{
		void StartRetrieving();
		event OnAssignmentsRetrievedHandler OnAssignmentsRetrieved;
	}

	[ComVisible(true)]
	public class DashboardManager : IAssignmentsRetriever
	{
		public const string EMPTY_FILE_PATH = "files\\Empty.htm";
		public const string TODO_FILE_PATH = "files\\ToDo.htm";
		private const string TRANSFORMATION_FILE_PATH = "files\\Transformation.xslt";
		public const string ERROR_FILE_PATH = "files\\Error.htm";

		private MyAssignments _assignments;
		private IAssignmentsRetriever _assignmentsRetriever;
		private static DashboardManager _instance;
		private string _lastResultXML = string.Empty;

		public delegate void OnAssignmentsUpdatedHandler();

		public event OnAssignmentsUpdatedHandler OnAssignmentsUpdated;
		public event OnAssignmentsRetrievedHandler OnAssignmentsRetrieved;

		private readonly Timer _assignmentsRefreshTimer = new Timer();


		public delegate void OnErrorHandler(object sender, ErrorEventArgs args);

		public event OnErrorHandler OnError;

		public static void ResetInstance()
		{
			_instance = null;
		}

		public static DashboardManager Instance
		{
			get { return _instance ?? (_instance = new DashboardManager()); }
		}

		public DashboardManager()
		{
			AssignmentsRefreshTimer.Interval = 30000;
			AssignmentsRefreshTimer.Enabled = false;
			AssignmentsRefreshTimer.Elapsed += OnAssignmentsRefreshTimerElapsed;
			SetAssignmentsRetriever(this);
		}

		public void SetAssignmentsRetriever(IAssignmentsRetriever assignmentsRetriever)
		{
			if (_assignmentsRetriever != null)
			{
				_assignmentsRetriever.OnAssignmentsRetrieved -=
					ProcessOnAssignmentsRetrieved;
			}

			assignmentsRetriever.OnAssignmentsRetrieved +=
				ProcessOnAssignmentsRetrieved;

			_assignmentsRetriever = assignmentsRetriever;
		}

		private void ProcessOnAssignmentsRetrieved(object sender, CompletedEventArgs e)
		{
			if (e.Exception != null && OnError != null)
			{
				var args = new ErrorEventArgs(e.Exception);
				OnError(sender, args);
				return;
			}

			SetAssignments(e.Assignments);
		}


		public void ChangeState(int assignableID, int stateID)
		{
			_assignmentsRefreshTimer.Stop();

			var myAssigmentsService = ServiceManager.GetService<MyAssignmentsService>();

			myAssigmentsService.ChangeStateCompleted += OnChangeStateCompleted;

			try
			{
				myAssigmentsService.ChangeStateAsync(assignableID, stateID);
			}
			catch(SoapException)
			{
				Messenger.ShowIncorrectVersionError();
			}
		}

		private void OnChangeStateCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				OnError(this, new ErrorEventArgs(e.Error));
				return;
			}

			Refresh();
		}

		public void InvokeUpdatedEvent()
		{
			if (OnAssignmentsUpdated != null)
				OnAssignmentsUpdated();

			_assignmentsRefreshTimer.Start();
		}

		public void Refresh()
		{
			_assignmentsRetriever.StartRetrieving();
		}

		private void OnAssignmentsRefreshTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_assignmentsRetriever.StartRetrieving();
		}

		public string HtmlErrorFilePath
		{
			get { return GetFilePath(ERROR_FILE_PATH); }
		}

		public string HtmlFilePath
		{
			get
			{
				return GetFilePath(_assignments == null ? EMPTY_FILE_PATH : TODO_FILE_PATH);
			}
		}

		private static string GetFilePath(string filePath)
		{
			return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, filePath);
		}

		void IAssignmentsRetriever.StartRetrieving()
		{
			if (!TpAuthenticationManager.Instance.IsAuthenticated)
				return;

			AssignmentsRefreshTimer.Stop();

			var myAssigmentsService = ServiceManager.GetService<MyAssignmentsService>();
			myAssigmentsService.GetMyAssigmentsCompleted += OnGetMyAssigmentsCompleted;

			try
			{
				myAssigmentsService.GetMyAssigmentsAsync();
			}
			catch (SoapException)
			{
				Messenger.ShowIncorrectVersionError();
			}
		}

		private void OnGetMyAssigmentsCompleted(object sender, GetMyAssigmentsCompletedEventArgs e)
		{
			var args = new CompletedEventArgs {Exception = e.Error, Assignments = e.Result};
			OnAssignmentsRetrieved(sender, args);

			if (e.Error == null)
				AssignmentsRefreshTimer.Start();
		}

		private void SetAssignments(MyAssignments assignments)
		{
			if (assignments != null && assignments.Assignables != null)
			{
				foreach (AssignableSimpleDTO assignable in assignments.Assignables)
				{
					assignable.IconPath = SettingsManager.TargetProcessPath + assignable.IconPath;
				}
			}

			var xmlSerializer = new XmlSerializer(typeof (MyAssignments));
			TextWriter writer = new StringWriter();

			xmlSerializer.Serialize(writer, assignments);

			var document = new XmlDocument();
			var results = writer.ToString();

			if (results == _lastResultXML)
				return;

			_lastResultXML = results;
			_assignments = assignments;

			results = results.Replace(" xmlns=\"http://targetprocess.com\"", "");
			document.LoadXml(results);

			PeformTransformation(document);

			if (OnAssignmentsUpdated != null)
				OnAssignmentsUpdated();
		}

		private static void PeformTransformation(XmlDocument document)
		{
			var compiledTransform = new XslCompiledTransform();
			var settings = new XsltSettings(true, true);
			compiledTransform.Load(GetFilePath(TRANSFORMATION_FILE_PATH), settings, new XmlUrlResolver());

			using (var streamWriter = new StreamWriter(GetFilePath(TODO_FILE_PATH), false))
				compiledTransform.Transform(document, null, streamWriter);
		}

		public Timer AssignmentsRefreshTimer
		{
			get { return _assignmentsRefreshTimer; }
		}
	}
}