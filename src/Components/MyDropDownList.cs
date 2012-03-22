using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class MyDropDownList: ComboBox
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			
			base.OnPaint(e);
		}
		protected override void  OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
		}
	}
}