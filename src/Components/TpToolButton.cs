// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	internal class TpToolButton : TpBaseButton
	{
		private bool isTogled;

		public bool Togle
		{
			set { isTogled = value; }
			get { return isTogled; }
		}

		public TpToolButton(Bitmap pict)
		{
			ViewNormal = pict;
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			BackColor = Color.Transparent;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			OnPaintBackground(pevent);
			if (isOver)
				pevent.Graphics.DrawImage(Resources.icon_hover, 0, 0);
			else if (isTogled)
				pevent.Graphics.DrawImage(Resources.icon_pressed, 0, 0);
			pevent.Graphics.DrawImage(normal, 0, 0);
		}
	}
}