namespace TpTrayUtility.Components.TpSecureForms
{
	public class TpSecureFormManager
	{
		public delegate void VoidFunc();

		public static void CheckAuthentication(VoidFunc init, VoidFunc close)
		{
			SettingsManager.ForceLogin = true;

			if (TpAuthenticationManager.Instance.IsAuthenticatedWithLoginForm)
			{
				init();
			}
			else
			{
				close();
			}
		}

		public static void CheckAuthentication(VoidFunc close)
		{
			CheckAuthentication(() => { }, close);
		}

		public static void OnClosed()
		{
			SettingsManager.ForceLogin = false;
		}
	}
}