// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class TpNumericUpDown : Control
	{
		public EventHandler ValueChanged;

		public TpNumericUpDown()
		{
			DoubleBuffered = true;
			Size = new Size(40, 20);
		}

		private bool mouseOver;

		protected override void OnMouseEnter(EventArgs e)
		{
			mouseOver = true;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			mouseOver = false;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Invalidate();
		}

		private int pressState;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Location.X > 20)
			{
				if (e.Location.Y < 10)
				{
					Value += Increment;
					pressState = 1;
				}
				else
				{
					Value -= Increment;
					pressState = 2;
				}
				if (ValueChanged != null)
					ValueChanged.Invoke(null, new EventArgs());
			}
			else
				pressState = 0;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			pressState = 0;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			switch (pressState)
			{
				case 1:
					e.Graphics.DrawImageUnscaled(Resources.select_small_press_top, 0, 0);
					break;
				case 2:
					e.Graphics.DrawImageUnscaled(Resources.select_small_press_bottom, 0, 0);
					break;
				default:
					if (mouseOver && PointToClient(MousePosition).X > 20)
					{
						if (PointToClient(MousePosition).Y < 10)
							e.Graphics.DrawImageUnscaled(Resources.select_small_hover_top, 0, 0);
						else
							e.Graphics.DrawImageUnscaled(Resources.select_small_hover_bottom, 0, 0);
					}
					else
					{
						e.Graphics.DrawImageUnscaled(Resources.select_small, 0, 0);
					}
					break;
			}
			var drawFormat = new StringFormat {Alignment = StringAlignment.Center};
			e.Graphics.DrawString(Value.ToString(), UI.SmallFont, Brushes.Black, new Rectangle(0, 2, 30, 20), drawFormat);
		}

		private int val;

		public int Value
		{
			set
			{
				if (value < Minimum)
					val = Minimum;
				else if (value > Maximum)
					val = Maximum;
				else
					val = value;
			}
			get { return val; }
		}

		private int minval = Int32.MinValue;

		public int Minimum
		{
			set { minval = value; }
			get { return minval; }
		}

		private int maxval = Int32.MaxValue;

		public int Maximum
		{
			set { maxval = value; }
			get { return maxval; }
		}

		public int Increment { get; set; }
	}
}