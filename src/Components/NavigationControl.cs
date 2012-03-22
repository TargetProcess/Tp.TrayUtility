//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class NavigationControl : Control
	{
		private Rectangle canvas;
		private Rectangle screen;

		public NavigationControl()
		{
			Size = new Size(150, 150);
			DoubleBuffered = true;
		}

		public void UpdateView(Rectangle _canvas, Rectangle _screen)
		{
			screen = _screen;
			canvas = _canvas;
			Invalidate();
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			MoveViewPort(e.Location);
			base.OnMouseClick(e);
		}

		private void MoveViewPort(Point pt)
		{
			if (!canvas.Size.IsEmpty)
			{
				float aspectRatio = Math.Min(150.0f/canvas.Width, 150.0f/canvas.Height);
				int tx = (int) (pt.X/aspectRatio + canvas.Left - screen.Width/2);
				int ty = (int) (pt.Y/aspectRatio + canvas.Top - screen.Height/2);
				ImageViewPort._viewPortInstance.MoveViewPort(tx, ty);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);

			if (!canvas.Size.IsEmpty)
			{
				float aspectRatio = Math.Min(150.0f/canvas.Width, 150.0f/canvas.Height);
				if (aspectRatio > 1.0f)
					aspectRatio = 1.0f;
				ImageViewPort._viewPortInstance.RenderToGraphics(e.Graphics, aspectRatio);
				e.Graphics.DrawRectangle(Pens.Yellow, (int) (-screen.X*aspectRatio), (int) (-screen.Y*aspectRatio),
				                         (int) (screen.Width*aspectRatio), (int) (screen.Height*aspectRatio));
			}
			e.Graphics.DrawRectangle(Pens.DarkBlue, 0, 0, Width - 1, Height - 1);
		}
	}
}