// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Net;
using Tp.AuthenticationServiceProxy;

namespace TpTrayUtility.Components
{
	public class TpAuthenticationManager : IAuthProvider
	{
		private static TpAuthenticationManager _authenticationManager;
		private bool _isIntegrated;
		private string _userName = string.Empty;
		private string _password = string.Empty;
		private static Exception _lastException;
		private readonly IAuthProvider _authProvider;
		public const string DEFAULT_PASSWORD_TEMPLATE = "Password";

		public delegate void ChangeLoginDelegate();

		public event ChangeLoginDelegate OnChangeLoginEvent;

		public void ChangeLogin()
		{
			if (OnChangeLoginEvent != null)
				OnChangeLoginEvent();
		}

		public static TpAuthenticationManager Instance
		{
			get
			{
				if (_authenticationManager == null)
					_authenticationManager = new TpAuthenticationManager();

				return _authenticationManager;
			}
		}

		public TpAuthenticationManager()
		{
			_authProvider = this;
			_password = SettingsManager.UserPassword;
			_userName = SettingsManager.UserName;
			_isIntegrated = SettingsManager.UseIntegratedAuth;
		}


		public bool IsAuthenticatedWithLoginForm
		{
			get
			{
				lock (this)
				{
					if (IsAuthenticated)
						return true;

					if (SettingsManager.ForceLogin)
					{
						SecurityForm securityForm = new SecurityForm();
						securityForm.ShowDialog();
					}
					return IsAuthenticated;
				}
			}
		}

		public bool TryAuthentificate(string userName, string password, bool isIntegrated, string processPath)
		{
			if (userName == string.Empty || processPath == "http://")
				return false;

			if (!processPath.EndsWith("/"))
				processPath += @"/";

			LastError = _authProvider.Authenticate(userName, password, isIntegrated, processPath);
			return LastError == null;
		}

		public bool IsAuthenticated
		{
			get
			{
				return TryAuthentificate(_userName, _password, _isIntegrated, SettingsManager.TargetProcessPath);
			}
		}

		public Exception LastError
		{
			get { return _lastException; }
			set { _lastException = value; }
		}

		public bool IsIntegrated
		{
			get { return _isIntegrated; }
			set
			{
				SettingsManager.UseIntegratedAuth = value;

				_isIntegrated = value;
			}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				SettingsManager.UserName = value;

				_userName = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if (SettingsManager.PasswordStored)
					SettingsManager.UserPassword = value;

				_password = value;
			}
		}

		public Exception Authenticate(string userName, string userPassword, bool isIntegrated, string processPath)
		{
			
			try
			{
				var wse = new AuthenticationService();

				wse.Credentials = GetCredentials(isIntegrated, userName, userPassword);
				wse.Url = processPath + "Services/AuthenticationService.asmx";
				if (isIntegrated && string.IsNullOrEmpty(userPassword))
					userPassword = DEFAULT_PASSWORD_TEMPLATE;
				TpServicePolicy.ApplyAutheticationTicket(wse, userName, userPassword);
				wse.Authenticate();
			}
			catch (Exception exception)
			{
				return exception;
			}

			return null;
		}

		public ICredentials GetCredentials()
		{
			return GetCredentials(_isIntegrated, _userName, _password);
		}

		public static ICredentials GetCredentials(bool isIntegratedAuth, string name, string pwd)
		{
			if (isIntegratedAuth)
				return CredentialCache.DefaultCredentials;

			string domain = "";
			string user = name;

			string[] strs = name.Split(new[] {'\\'}, 2);

			if (strs.Length == 2)
			{
				domain = strs[0];
				user = strs[1];
			}

			return new NetworkCredential(user, pwd, domain);
		}
	}
}