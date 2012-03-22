// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class PropertyControlBase : Control
	{
		public EventHandler PropertyChanged;

		public int Zoom { get; set; }

		protected virtual Color BackgroundColor
		{
			get { return Color.FromArgb(128, 160, 160, 160); }
		}

		private int _controlOffset = X_OFFSET;

		private const int X_OFFSET = 20;
		private const int Y_OFFSET20 = 20;
		private const int Y_OFFSET22 = 22;


		public PropertyControlBase()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//Size = new Size(600, 60);
			DoubleBuffered = true;

			BackColor = Color.Transparent;
			Padding = new Padding(20);
			Margin = new Padding(20);
		}

		public void Reset()
		{
			_controlOffset = X_OFFSET;
			Controls.Clear();
		}

		public virtual void AttachTo(Primitive primitive)
		{
		}

		public TpLabel AddLabel(string text)
		{
			var txt = new TpLabel
			          	{
			          		Text = text,
			          		Location = new Point(_controlOffset, Y_OFFSET22),
			          		Height = 20,
			          		Font = UI.DefaultFont,
			          		ForeColor = Color.White,
							BackColor = Color.Transparent
			          	};

			Size sz = TextRenderer.MeasureText(text, UI.DefaultFont);
			txt.Width = sz.Width;

			Controls.Add(txt);
			_controlOffset += sz.Width;
			return txt;
		}

		public TpNumericUpDown AddNumControl(int min, int max, int step, int width)
		{
			var numControl = new TpNumericUpDown
			{
				Minimum = min,
				Maximum = max,
				Increment = step,
				Location = new Point(_controlOffset, Y_OFFSET20)
			};
			Controls.Add(numControl);
			_controlOffset += width + 5;
			return numControl;
		}

		public TogleButton AddTogleButtonControl(Bitmap normal, Bitmap over, Bitmap pressed)
		{
			var tb = new TogleButton(normal, over, pressed) {Location = new Point(_controlOffset, Y_OFFSET20)};
			Controls.Add(tb);
			_controlOffset += tb.Width;
			return tb;
		}

		public ChangeColorControl AddColorControl()
		{
			var colorBorderControl = new ChangeColorControl {Location = new Point(_controlOffset, Y_OFFSET22)};
			Controls.Add(colorBorderControl);
			_controlOffset += 30;
			return colorBorderControl;
		}

		public void AddControl(Control control, int width)
		{
			control.Location = new Point(_controlOffset, Y_OFFSET20);
			Controls.Add(control);
			_controlOffset += width;
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);

			using (Brush sb = new SolidBrush(BackgroundColor))
			{
				pevent.Graphics.FillPath(sb, RoundedRectangle.Create(0, 0, Width - 10, 60, 5));
			}

			pevent.Graphics.DrawString(String.Format("{0}%", Zoom), UI.DefaultFont, Brushes.White, Width - 65, Height - 37);
		}
	}
}