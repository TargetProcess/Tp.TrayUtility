// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TogleButton : TpBaseButton
	{
		public TogleButton(Bitmap _normal, Bitmap _over, Bitmap _pressed)
		{
			ViewNormal = _normal;
			ViewOver = _over;
			ViewPressed = _pressed;
		}

		public bool Togled { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(255, 160, 160, 160));
			if (Togled)
			{
				e.Graphics.DrawImageUnscaled(pressed, 0, 0);
			}
			else
			{
				base.OnPaint(e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			Togled = !Togled;
			base.OnMouseDown(mevent);
		}
	}
}