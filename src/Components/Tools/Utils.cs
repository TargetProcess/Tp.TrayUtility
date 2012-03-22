using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TpTrayUtility.Components.Tools
{
	public static class Utils
	{
		public static bool IsEmpty (this StringBuilder sb)
		{
			return sb.Length == 0;
		}

		public static string GetString(this Keys key)
		{
			switch (key)
			{
				case Keys.PageDown:
					return "PageDown";
				case Keys.Enter:
					return "Enter";
				default:
					return key.ToString();
			}			
		}

		public static bool IsSystemKey(this Keys key)
		{
			return key >= Keys.PageUp && key <= Keys.Help
			       || key >= Keys.F1 && key <= Keys.F12
			       || key == Keys.Escape
			       || key >= Keys.Back && key <= Keys.Enter
			       || key == Keys.Space
			       || key == Keys.NumLock;
		}
	}
}
