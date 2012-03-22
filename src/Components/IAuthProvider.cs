using System;

namespace TpTrayUtility.Components
{
	public interface IAuthProvider
	{
		Exception Authenticate(string userName, string userPassword, bool isIntegrated, string processPath);
	}
}