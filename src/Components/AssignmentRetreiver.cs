// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Tp.MyAssignmentsServiceProxy;

namespace TpTrayUtility.Components
{
	internal class AssignmentRetreiver : Form
	{
		public static AssignmentRetreiverEventHalder assignmentsHandler;

		private static Timer timer;
		private static AssignmentRetreiver instance;
		private static BackgroundWorker backgroundWorker;

		private MyAssignments assignments;

		public AssignmentRetreiver()
		{
			if (instance != null)
				return;
			instance = this;

			backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += backgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;


			timer = new Timer();
			timer.Tick += TimerMethod;
			timer.Interval = 100;
			timer.Start();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			assignmentsHandler.Invoke(null, new AssignmentRetreiverEventArgs(assignments));
			timer.Start();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			RetrieveLatestAssignments();
		}

		private void TimerMethod(Object state, EventArgs myEventArgs)
		{
			timer.Stop();
			timer.Interval = 300000;
			backgroundWorker.RunWorkerAsync();
		}

		private void RetrieveLatestAssignments()
		{
			if (!TpAuthenticationManager.Instance.IsAuthenticatedWithLoginForm)
				return;
			var myAssigmentsService = ServiceManager.GetService<MyAssignmentsService>();
			try
			{
				assignments = myAssigmentsService.GetMyAssigments();
			}
			catch
			{
				Messenger.ShowIncorrectVersionError();
			}
		}
	}
}