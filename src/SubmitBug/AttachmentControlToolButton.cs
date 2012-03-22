// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Windows.Forms;

namespace TpTrayUtility.SubmitBug
{
	public class AttachmentControlToolButton : AttachmentControlScrollButton
	{
		public AttachmentControlToolButton(string toolTip)
		{
			var thisTip = new ToolTip {ShowAlways = true};
			thisTip.SetToolTip(this, toolTip);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			pevent.Graphics.DrawImageUnscaled(normal, (Width - normal.Width)/2, (Height - normal.Height)/2);
		}
	}
}