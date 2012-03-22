// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class GlobalMouseHandler : IMessageFilter
	{
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_RBUTTONDOWN = 0x204;
		private const int WM_MBUTTONDOWN = 0x207;

		public event MouseEventHandler MouseClick = null;

		public GlobalMouseHandler()
		{
			Application.AddMessageFilter(this);
		}


		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN)
			{
				if (MouseClick != null)
				{
					int x = m.LParam.ToInt32() & 0xFFFF;
					int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
					MouseEventArgs mea = new MouseEventArgs(MouseButtons.None, 0, x, y, 0);
					MouseClick.Invoke(null, mea);
				}
			}
			return false;
		}
	}
}