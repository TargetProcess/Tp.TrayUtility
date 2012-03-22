//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.Net;
using System.Security.Principal;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility
{
	public partial class SecurityForm : TpForm
	{
		private const string ADDITIONAL_ERROR_MESSAGE = "Please check IIS Authentication configuration of TargetProcess.";
		private const string UNAUTHORIZED_ERROR_MESSAGE = "Could not connect to the server. Please check entered data.";
		
		private const string COULDNOT_CONNECT_ERROR_MESSAGE = "Could not connect to the server";
		private const string CHECK_ENTERED_DATA = "Please check entered data";


		public String UserName
		{
			get { return textUserName.Text; }
			set { textUserName.Text = value; }
		}

		public String Pwd
		{
			get { return textPwd.Text; }
			set { textPwd.Text = value; }
		}

		public String Url
		{
			get { return textBoxTPURL.Text; }
			set { textBoxTPURL.Text = value; }
		}

		public SecurityForm()
		{
			InitializeComponent();

			checkBoxSavePwd.Checked = SettingsManager.PasswordStored;
			UserName = TpAuthenticationManager.Instance.UserName;
			Pwd = TpAuthenticationManager.Instance.Password;
			Url = SettingsManager.TargetProcessPath;
			checkBoxUseIntegratedAuth.Checked = TpAuthenticationManager.Instance.IsIntegrated;
		}

		private void buttonLogin_Click(object sender, EventArgs e)
		{
			if (!ValidateInputData())
				return;

			if (!ValidateChildren())
				return;

			Cursor = Cursors.WaitCursor;

			try
			{
				if (TpAuthenticationManager.Instance.TryAuthentificate(UserName, Pwd, checkBoxUseIntegratedAuth.Checked, Url))
				{
					SaveCredentials();
					SettingsManager.ForceLogin = true;
					Cursor = Cursors.Arrow;
					Close();
					DialogResult = DialogResult.OK;
					TpAuthenticationManager.Instance.ChangeLogin();
					
					return;
				}
				var exception = TpAuthenticationManager.Instance.LastError;

				if (exception != null)
					throw exception;
			}
			catch (SoapHeaderException ex)
			{
				Messenger.ShowError(string.Format("{0}. The reason is: {1}. {2}.\n{3}", COULDNOT_CONNECT_ERROR_MESSAGE, ex.Code.Name, CHECK_ENTERED_DATA, ADDITIONAL_ERROR_MESSAGE));
			}
			catch (WebException)
			{
				Messenger.ShowError(string.Format("{0}. {1}.", COULDNOT_CONNECT_ERROR_MESSAGE, CHECK_ENTERED_DATA));
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("SecurityFault") || ex.Message.Contains("Authentication failed"))
				{
					Messenger.ShowError(string.Format("Authentication failed. Check your login and password.\n{0}",ADDITIONAL_ERROR_MESSAGE));
				}
				else if (ex.Message.Contains("WSE910"))
				{
					if (ex.InnerException != null && ex.InnerException.InnerException != null &&
					    ex.InnerException.InnerException.Message.Contains("WSE065"))
						Messenger.ShowError(
							"The clock on your computer is not synchronized with server's clock. Please syncrhonize them.");
					else
						Messenger.ShowError(UNAUTHORIZED_ERROR_MESSAGE);
				}
				else
				{
					Messenger.ShowError(UNAUTHORIZED_ERROR_MESSAGE);
				}
			}
			Cursor = Cursors.Arrow;
		}

		private void SaveCredentials()
		{
			SettingsManager.TargetProcessPath = Url;
			SettingsManager.PasswordStored = checkBoxSavePwd.Checked;
			TpAuthenticationManager.Instance.IsIntegrated = checkBoxUseIntegratedAuth.Checked;
			TpAuthenticationManager.Instance.UserName = UserName;
			TpAuthenticationManager.Instance.Password = Pwd;
		}

		private bool ValidateInputData()
		{
			return ValidatePassword() && ValidateUserName() && ValidateUrl();
		}

		private void checkBoxUseIntegratedAuth_CheckedChanged(object sender, EventArgs e)
		{
			textUserName.Enabled = textPwd.Enabled = !checkBoxUseIntegratedAuth.Checked;

			textUserName.Text = checkBoxUseIntegratedAuth.Checked
			                    	? WindowsIdentity.GetCurrent().Name
			                    	: SettingsManager.UserName;
		}

		private void textBoxTPURL_Validated(object sender, EventArgs e)
		{
			ValidateUrl();
		}

		private bool ValidateUrl()
		{
			return ValidateTextBox(IsValidUrl, textBoxTPURL, "Please provide url to TP v.2 System");
		}

		private bool IsValidUrl()
		{
			return Url != string.Empty && Url != "http://";
		}

		private bool IsValidName()
		{
			return UserName != string.Empty;
		}

		private bool IsValidPassword()
		{
			return Pwd != string.Empty || checkBoxUseIntegratedAuth.Checked;
		}

		private void textUserName_Validated(object sender, EventArgs e)
		{
			ValidateUserName();
		}

		private bool ValidateUserName()
		{
			return ValidateTextBox(IsValidName, textUserName, "Please provide user name");
		}

		private void textPwd_Validated(object sender, EventArgs e)
		{
			ValidatePassword();
		}

		private bool ValidatePassword()
		{
			return ValidateTextBox(IsValidPassword, textPwd, "Please provide user password");
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = DialogResult.Cancel;
		}

		private bool ValidateTextBox(Func<bool> validate, TpTextBox textBox, string message)
		{
			var result = validate();

			errorProvider.SetError(textBox, result ? "" : message);

			return result;
		}
	}
}