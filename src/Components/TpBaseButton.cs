// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TpBaseButton : Button
	{
		protected bool isOver;
		protected bool isPressed;

		protected Bitmap normal;

		public Bitmap ViewNormal
		{
			set
			{
				normal = value;
				Size = normal.Size;
			}
		}

		public TpBaseButton()
		{
			Offset = new Point();
		}

		protected Bitmap over;

		public Bitmap ViewOver
		{
			set { over = value; }
		}

		protected Bitmap pressed;

		public Bitmap ViewPressed
		{
			set { pressed = value; }
		}

		public Point Offset { get; set; }

		protected override void OnPaint(PaintEventArgs pevent)
		{
			OnPaintBackground(pevent);

			if (isPressed && pressed != null)
			{
				pevent.Graphics.DrawImage(pressed, Offset);
			}
			else if (isOver && over != null)
			{
				pevent.Graphics.DrawImage(over, Offset);
			}
			else if (normal != null)
			{
				pevent.Graphics.DrawImage(normal, Offset);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			isOver = true;
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			isOver = false;
			isPressed = false;
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			isPressed = true;
			Invalidate();
			base.OnMouseDown(mevent);
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			isPressed = false;
			Invalidate();
			base.OnMouseUp(mevent);
		}
	}
}