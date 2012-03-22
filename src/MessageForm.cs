//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Events;

namespace TpTrayUtility
{
	internal class MessageForm
	{
		private readonly SubmitBugForm _submitBugForm;
		private readonly StatusForm form;

		public MessageForm(SubmitBugForm submitBugForm)
		{
			_submitBugForm = submitBugForm;
			_submitBugForm.BugBeforePost += BugBeforePost;
			_submitBugForm.BugPosted += BugPosted;
			_submitBugForm.BugPostingAction += BugPostingAction;

			form = new StatusForm();
			form.BtnClose.Visible = false;			
			form.Hiding += Form_Hiding;
		}

		public event EventHandler Closed;

		private void Form_Hiding(object sender, EventArgs e)
		{
			if (_submitBugForm.InvokeRequired)
			{
				//Synchronization with _submitBugForm Thread
				_submitBugForm.Invoke(new EventHandler(Form_Hiding), new[] {sender, e});
				return;
			}
			if (Closed != null)
				Closed(sender, e);
		}


		private void BugPostingAction(object sender, MessageEventArgs e)
		{
			if (form.InvokeRequired)
			{
				form.Invoke(new EventHandler<MessageEventArgs>(BugPostingAction), new[] {sender, e});
				return;
			}
			form.Message.Text = e.Message;
		}

		private void BugPosted(object sender, EventArgs e)
		{
			if (form.InvokeRequired)
			{
				form.Invoke(new EventHandler(BugPosted), new[] {sender, e});
				return;
			}
			form.Message.Text = @"Bug Posted.";
			form.BtnClose.Visible = true;
		}

		private void BugBeforePost(object sender, EventArgs e)
		{
			if (form.InvokeRequired)
			{
				form.Invoke(new EventHandler(BugBeforePost), new[] {sender, e});
				return;
			}
			form.Message.Text = @"Posting Bug ...";
		}

		public void Show(Form owner)
		{
			form.Show(owner);		
		}
	}
}