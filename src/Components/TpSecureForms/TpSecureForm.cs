using System;

namespace TpTrayUtility.Components.TpSecureForms
{
	public abstract class TpSecureForm : TpForm, ITpSecureForm
	{
		public abstract void Init();

		protected TpSecureForm()
		{
			TpSecureFormManager.CheckAuthentication(Init, Close);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			TpSecureFormManager.OnClosed();
		}
	}
}