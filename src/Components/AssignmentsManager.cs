// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tp.MyAssignmentsServiceProxy;

namespace TpTrayUtility.Components
{
	internal class AssignmentsManager
	{
		public static AssignmentsManager Instance;

		private readonly List<TPPopupNotifier> popupList = new List<TPPopupNotifier>();
		private List<AssignmentData> previousTasks = SettingsManager.KnownAssignments;
		private readonly List<AssignmentData> unreadTasks = SettingsManager.UnreadAssignments;

		private bool isFirstTime;

		public AssignmentsManager(bool firstTime)
		{
			isFirstTime = firstTime;
			Instance = this;
			AssignmentRetreiver.assignmentsHandler += UpdateTaskList;
			if (unreadTasks.Count > 0)
				FormsManager.GetInstance<TPEMain>().StartNewMessageBlinking();
		}

		private const int SW_SHOWNA = 8;

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

		private void RecalculatePopupPosistions()
		{
			int offset = 0;
			foreach (var form in popupList)
			{
				form.SetDesktopLocation(Screen.PrimaryScreen.WorkingArea.Width - form.Width - 7,
				                        Screen.PrimaryScreen.WorkingArea.Height - offset - 7 - form.Height);
				ShowWindow(form.Handle, SW_SHOWNA);
				offset += form.Height + 2;
				form.Update();
				form.Invalidate();
			}
		}


		public bool IsPreviousTask(int ID)
		{
			return previousTasks.Any(knownTask => ID == knownTask.EntityID);
		}

		public bool IsUnreadTask(int ID)
		{
			return unreadTasks.Any(knownTask => ID == knownTask.EntityID);
		}


		private void UpdateTaskList(object sender, AssignmentRetreiverEventArgs args)
		{
			if (args.Assignments == null)
				return;

			var knownTasks = new List<AssignmentData>();

			foreach (var assignment in args.Assignments.Assignables)
			{
				knownTasks.Add(new AssignmentData(assignment.ID.Value, assignment.Name));
				if (!IsPreviousTask(assignment.ID.Value) && !IsUnreadTask(assignment.ID.Value) && !isFirstTime)
				{
					AddNewAssignment(assignment);
				}
			}

			isFirstTime = false;
			previousTasks = knownTasks;
			SettingsManager.KnownAssignments = previousTasks;
		}

		private void AddNewAssignmentReadOnce(AssignmentData knownTask)
		{
			var form = new TPPopupNotifier(knownTask, false);
			form.FormClosing += form_FormClosing;
			popupList.Add(form);
			RecalculatePopupPosistions();
		}


		private void AddNewAssignment(AssignableSimpleDTO assignment)
		{
			var form = new TPPopupNotifier(assignment);
			form.FormClosing += form_FormClosing;
			popupList.Add(form);
			RecalculatePopupPosistions();
		}


		private void form_FormClosing(object sender, FormClosingEventArgs e)
		{
			var popup = (TPPopupNotifier) sender;
			if (popupList.Any(form => popup.EntityID == form.EntityID))
			{
				popupList.Remove(popup);
				if (popup.MessageRead)
				{
					previousTasks.Add(popup.assignmentData);
					SettingsManager.KnownAssignments = previousTasks;
				}
				else
				{
					unreadTasks.Add(popup.assignmentData);
					SettingsManager.UnreadAssignments = unreadTasks;
					FormsManager.GetInstance<TPEMain>().StartNewMessageBlinking();
				}
			}
			RecalculatePopupPosistions();
		}


		internal void ReadNextMessage()
		{
			if (unreadTasks.Count > 0)
			{
				AddNewAssignmentReadOnce(unreadTasks[0]);
				unreadTasks.RemoveAt(0);
				SettingsManager.UnreadAssignments = unreadTasks;
			}
			if (unreadTasks.Count == 0)
			{
				FormsManager.GetInstance<TPEMain>().StopNewMessageBlinking();
			}
		}

		public bool IsMessageVisible
		{
			get { return popupList.Count > 0; }
		}
	}
}