// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class ChangeColorControl : Control
	{
		private static int[] customColors;
		protected Color color;
		public EventHandler ColorChanged;

		private readonly ToolTip colorToolTip = new ToolTip();

		public Color Color
		{
			get { return color; }
			set
			{
				color = value;
				colorToolTip.ShowAlways = true;
				colorToolTip.SetToolTip(this, TextFromColor(color));
			}
		}

		public string TextFromColor(Color c)
		{
			return String.Format("R:{0} G:{1} B:{2}", c.R, c.G, c.B);
		}

		public ChangeColorControl()
		{
			Size = new Size(16, 15);
			Cursor = Cursors.Hand;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (var brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, 3, 3, 10, 10);
			}
			e.Graphics.DrawImageUnscaled(Resources.select_color, 0, 0);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			var colorDialog = new ColorDialog {Color = Color};

			ApplyCustomColors(colorDialog);

			if (colorDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			SaveDefinedCustomColors(colorDialog);

			Color = colorDialog.Color;
			Invalidate();
			if (ColorChanged != null)
				ColorChanged.Invoke(null, new EventArgs());
		}

		private void SaveDefinedCustomColors(ColorDialog colorDialog)
		{
			if (colorDialog.CustomColors != null && colorDialog.CustomColors.Any())
			{
				customColors = colorDialog.CustomColors;
			}
		}

		private void ApplyCustomColors(ColorDialog colorDialog)
		{
			if (customColors != null && customColors.Any())
			{
				colorDialog.CustomColors = customColors;
			}
		}
	}
}