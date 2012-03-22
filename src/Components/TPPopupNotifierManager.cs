// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Tp.MyAssignmentsServiceProxy;

namespace TpTrayUtility.Components
{
	internal class TPPopupNotifier : TpForm
	{
		public static Size FormSize = new Size(250, 80);

		private LinkLabel link = new LinkLabel();
		private int _entityID;

		public int EntityID
		{
			get { return _entityID; }
			set { _entityID = value; }
		}


		public bool RemovalFlag;

		public bool MessageRead;

		public AssignmentData assignmentData;

		public TPPopupNotifier(AssignmentData assignment, bool _RemovalFlag)
		{
			RemovalFlag = _RemovalFlag;
			MessageRead = !RemovalFlag;

			assignmentData = new AssignmentData();
			assignmentData.EntityID = assignment.EntityID;
			assignmentData.Name = assignment.Name;
			InitializeComponent();
		}

		public TPPopupNotifier(AssignableSimpleDTO assignment)
		{
			/*			switch (assignment.EntityTypeID)
			{
				case 4: type = NotificationType.Task; break;
				case 5: type = NotificationType.Task; break;
				case 8: type = NotificationType.Bug; break;
				case 9: type = NotificationType.Feature; break;
				case 17: type = NotificationType.Request; break;
				default: type = NotificationType.Other; break;
			}*/


			/*switch (notificationType)
			{
				case NotificationType.Bug:
					BackColor = Color.LightPink;
					break;
				default:
					BackColor = Color.Azure;
					break;
			}*/

			assignmentData = new AssignmentData();
			assignmentData.EntityID = assignment.ID.Value;
			assignmentData.Name = assignment.Name;

			InitializeComponent();
		}


		private readonly Timer autoCloseTimer = new Timer();
		private double opacacityValue = 1.0;


		private void InitializeComponent()
		{
			autoCloseTimer.Interval = 10000;
			autoCloseTimer.Tick += autoCloseTimer_Tick;
			autoCloseTimer.Start();

			EntityID = assignmentData.EntityID;

			Size = FormSize;
			//TopMost = true;
			ShowInTaskbar = false;
			DrawCaption = false;
			StartPosition = FormStartPosition.Manual;

			Cursor = Cursors.Hand;
			closeButton.Click += closeButton_Click;
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			MessageRead = true;
		}


		protected override void OnMouseClick(MouseEventArgs e)
		{
			string url = SettingsManager.TargetProcessPath + "View.aspx?id=" + assignmentData.EntityID;
			SubmitBugForm.ShellExecute("open", url);
			Close();
		}

		private void link_Click(object sender, EventArgs e)
		{
			string url = SettingsManager.TargetProcessPath + "View.aspx?id=" + assignmentData.EntityID;
			SubmitBugForm.ShellExecute("open", url);
			Close();
		}


		private void autoCloseTimer_Tick(object sender, EventArgs e)
		{
			autoCloseTimer.Stop();
			if (autoCloseTimer.Interval >= 1000)
			{
				autoCloseTimer.Interval = 50;
				opacacityValue = 1.0;
			}
			else
			{
				opacacityValue -= 0.05;
				if (opacacityValue < 0.1)
				{
					Close();
					return;
				}

				//Invalidate();
			}
			autoCloseTimer.Start();
			Opacity = opacacityValue;
		}

/*		protected override void OnMouseEnter(EventArgs e)
		{
			Opacity = 1.0;
			autoCloseTimer.Stop();
			autoCloseTimer.Interval = 10000;
			autoCloseTimer.Start();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Opacity = 1.0;
			autoCloseTimer.Stop();
			autoCloseTimer.Interval = 10000;
			autoCloseTimer.Start();
			base.OnMouseMove(e);
		}*/


		protected override void OnClosing(CancelEventArgs e)
		{
//			MessageRead = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawString(string.Format("#{0} {1}", assignmentData.EntityID, assignmentData.Name), UI.SmallFont,
			                      Brushes.LightGray, new Rectangle(20, 20, Width - 40, Height - 20));

/*	
			link.AutoSize = false;
			link.Text = );
			link.Size = FormSize;
			link.Width -= 60;
			link.Height -= 20;
			link.Location = new Point(20, 15);
			link.BackColor = Color.Transparent;
			link.Click += new EventHandler(link_Click);
			Controls.Add(link);
*/


			if (RemovalFlag)
			{
				Pen redPen = Pens.Red;
				e.Graphics.DrawLine(redPen, 3, 3, Width - 3, Height - 3);
				e.Graphics.DrawLine(redPen, Width - 3, 3, 3, Height - 3);
			}
		}
	}

	public delegate void AssignmentRetreiverEventHalder(object sender, AssignmentRetreiverEventArgs args);
}