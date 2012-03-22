//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using Tp.MyAssignmentsServiceProxy;
using TpTrayUtility.Components;

namespace TpTrayUtility
{
	public partial class TimeDescriptionForm : Form
	{
		private DateTime startDateTime;
		private DateTime finishDateTime;

		private int assignableID;

		public DateTime StartDate
		{
			set
			{
				startDateTime = value;
				startTime.Text = startDateTime.ToShortTimeString();
			}
			get { return startDateTime; }
		}

		public DateTime FinishDate
		{
			set
			{
				finishDateTime = value;
				finishTime.Text = startDateTime.ToShortTimeString();
			}
			private get { return finishDateTime; }
		}

		public int AssignableID
		{
			set { assignableID = value; }
			get { return assignableID; }
		}


		public TimeDescriptionForm()
		{
			InitializeComponent();
		}

		private bool ValidateTime()
		{
			DateTime tmpDate1;
			DateTime tmpDate2;
			if (DateTime.TryParse(startTime.Text, out tmpDate1) && DateTime.TryParse(finishTime.Text, out tmpDate2))
			{
				startDateTime = tmpDate1;
				finishDateTime = tmpDate2;
				return true;
			}
			return false;
		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			if (ValidateTime())
			{
				if (!TpAuthenticationManager.Instance.IsAuthenticatedWithLoginForm)
					return;
				var myAssigmentsService = ServiceManager.GetService<MyAssignmentsService>();

				var timeRecords = new TimeSimpleDTO[1];
				timeRecords[0] = new TimeSimpleDTO
				                 	{
				                 		AssignableID = assignableID,
				                 		Started = StartDate,
				                 		Ended = FinishDate,
				                 		Description = timeDescription.Text
				                 	};
				try
				{
					var errors = myAssigmentsService.SubmitTime(timeRecords);
				}
				catch (SoapException)
				{
					Messenger.ShowIncorrectVersionError();
				}

				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void time_TextChanged(object sender, EventArgs e)
		{
			if (ValidateTime())
			{
				var time = Utility.GetElapsedTimeString(startDateTime, finishDateTime);
				totalTime.Text = time;
			}
		}
	}
}