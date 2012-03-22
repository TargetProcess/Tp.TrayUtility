//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Components.TpSecureForms;

namespace TpTrayUtility
{
	public partial class frmDashboard : TpSecureSimpleForm
	{
		private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (lastException != null)
			{
				webBrowser.Document.GetElementById("lblError").InnerHtml = lastException.Message.Replace("/r/n", "<br>");
				lastException = null;
			}
		}

		private Exception lastException;

		private void Instance_OnError(object sender, ErrorEventArgs args)
		{
			Exception exception = args.GetException();
			lastException = exception;
			webBrowser.Navigate(DashboardManager.Instance.HtmlErrorFilePath);
		}

		private void frmDashboard_Closing(object sender, CancelEventArgs e)
		{
			DashboardManager.Instance.OnAssignmentsUpdated -=
				Instance_OnAssignmentsUpdated;
		}

		private void Instance_OnAssignmentsUpdated()
		{
			webBrowser.Navigate(DashboardManager.Instance.HtmlFilePath);
		}

		public override void Init()
		{
			InitializeComponent();
			DashboardManager.Instance.Refresh();
			DashboardManager.Instance.OnAssignmentsUpdated += Instance_OnAssignmentsUpdated;
			DashboardManager.Instance.OnError += Instance_OnError;
			webBrowser.ObjectForScripting = DashboardManager.Instance;
			webBrowser.Navigate(DashboardManager.Instance.HtmlFilePath);

			Closing += frmDashboard_Closing;

			webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
		}
	}
}