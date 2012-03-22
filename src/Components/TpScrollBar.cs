//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	internal class TpScrollBar : Control
	{
		public enum Style
		{
			Vertical,
			Horizontal
		} ;

		private Style style;
		public event EventHandler ValueChanged = null;
		private int positionCount;
		private int position;

		private readonly Timer repeatClickTimer = new Timer();

		public TpScrollBar(Style st)
		{
			repeatClickTimer.Tick += repeatClickTimer_Tick;
			repeatClickTimer.Stop();

			style = st;

			Size = st == Style.Vertical ? new Size(20, 100) : new Size(100, 20);

			DoubleBuffered = true;
		}

		private void repeatClickTimer_Tick(object sender, EventArgs e)
		{
			const int PageSize = 0;
			if (PositionCount > 0)
				Math.Max(sliderSize, Height/PositionCount);

			var pt = PointToClient(Cursor.Position);
			if (pt.Y < sliderPosition)
			{
				Position--;
				Invalidate();

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
				repeatClickTimer.Interval = 50;
			}
			else if (pt.Y > sliderPosition + PageSize)
			{
				Position++;
				Invalidate();
				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
				repeatClickTimer.Interval = 50;
				repeatClickTimer.Start();
			}
		}

		public int PositionCount
		{
			set
			{
				if (positionCount != value)
				{
					positionCount = value;
					if (positionCount < 0)
						positionCount = 0;
					UpdateFromReal();
				}
			}
			get { return positionCount; }
		}

		public int Position
		{
			set
			{
				position = value;
				if (position < 0)
					position = 0;
				if (position > positionCount)
					position = positionCount;
				UpdateFromReal();
			}
			get { return position; }
		}

		protected void UpdateFromReal()
		{
			if (PositionCount == 0)
				sliderPosition = 0;
			else
				sliderPosition = (Position*(Height - sliderSize))/PositionCount;
		}

		protected void UpdateReal()
		{
			position = ((sliderPosition + dragDelta)*PositionCount)/(Height - sliderSize);
		}

		private int sliderPosition;
		private const int sliderSize = 52;

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (
				var brush = new LinearGradientBrush(new Point(0, 0), new Point(Width, 0), Color.Gray,
				                                    Color.FromArgb(255, 48, 48, 48)))
			{
				e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
			}

			using (var pen = new Pen(Color.FromArgb(255, 36, 36, 36)))
			{
				e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(Resources.slide, 2, sliderPosition + dragDelta + 2);
		}

		protected void SetSliderPoisitonByCoords(int axisOffset)
		{
			Position = axisOffset;
		}

		private bool isDragMode;
		private int dragValue;
		private int dragDelta;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			int PageSize = 0;
			if (PositionCount > 0)
				PageSize = Math.Max(sliderSize, Height/PositionCount);

			if (e.Y >= sliderPosition && e.Y < sliderPosition + PageSize)
			{
				isDragMode = true;
				dragValue = e.Y;
			}
			else if (e.Y < sliderPosition)
			{
				Position--;
				Invalidate();

				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
				repeatClickTimer.Interval = 500;
				repeatClickTimer.Start();
			}
			else if (e.Y > sliderPosition + PageSize)
			{
				Position++;
				Invalidate();
				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
				repeatClickTimer.Interval = 500;
				repeatClickTimer.Start();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			repeatClickTimer.Stop();
			if (isDragMode)
			{
				isDragMode = false;
				sliderPosition = sliderPosition + dragDelta;
				dragDelta = 0;
				if (ValueChanged != null)
					ValueChanged(this, new EventArgs());
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (isDragMode)
			{
				dragDelta = e.Y - dragValue;
				if (sliderPosition + dragDelta < 0)
					dragDelta = -sliderPosition;
				if (sliderPosition + dragDelta + sliderSize >= Height)
					dragDelta = Height - sliderSize - sliderPosition;
				Invalidate();

				int oldPos = position;
				UpdateReal();
				if (ValueChanged != null && oldPos != position)
					ValueChanged(this, new EventArgs());
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			isDragMode = false;
			base.OnMouseLeave(e);
		}
	}
}