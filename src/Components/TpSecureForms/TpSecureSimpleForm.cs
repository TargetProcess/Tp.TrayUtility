using System;
using System.Windows.Forms;

namespace TpTrayUtility.Components.TpSecureForms
{
	public abstract class TpSecureSimpleForm : Form, ITpSecureForm
	{
		public abstract void Init();

		protected TpSecureSimpleForm()
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