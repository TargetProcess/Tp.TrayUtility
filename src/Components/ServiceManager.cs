// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using Microsoft.Web.Services3;

namespace TpTrayUtility.Components
{
	public class ServiceManager
	{
		public static T GetService<T>() where T : WebServicesClientProtocol, new()
		{
			T serviceWse = new T
			{
			    Url =
			        string.Format("{0}Services/{1}.asmx", SettingsManager.TargetProcessPath,
			               			typeof (T).Name),
			    Credentials = TpAuthenticationManager.Instance.GetCredentials()				
			};

			var password = TpAuthenticationManager.Instance.IsIntegrated &&
			               string.IsNullOrEmpty(TpAuthenticationManager.Instance.Password)
			               	? TpAuthenticationManager.DEFAULT_PASSWORD_TEMPLATE
			               	: TpAuthenticationManager.Instance.Password;
			

			TpServicePolicy.ApplyAutheticationTicket(serviceWse, TpAuthenticationManager.Instance.UserName, password);
			return serviceWse;
		}
	}
}