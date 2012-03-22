// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;

namespace TpTrayUtility.Components
{
	public class TpServicePolicy : Policy
	{
		public TpServicePolicy()
		{
			Assertions.Add(new UsernameOverTransportAssertion());
			Assertions.Add(new RequireActionHeaderAssertion());
		}

		public static UsernameToken GetUsernameToken(string username, string password, PasswordOption passwordOption)
		{
			var token = new UsernameToken(username, password, passwordOption);

			var securityTokenManager = SecurityTokenManager.GetSecurityTokenManagerByTokenType(WSTrust.TokenTypes.UsernameToken);
			securityTokenManager.CacheSecurityToken(token);

			return token;
		}

		public static void ApplyAutheticationTicket(WebServicesClientProtocol protocol, string userName, string password)
		{
			var token = GetUsernameToken(userName, password, PasswordOption.SendPlainText);
			protocol.SetClientCredential(token);
			protocol.SetPolicy(new TpServicePolicy());
		}
	}
}