// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class ToolControl : Control
	{
		public int buttonOffset = 5;

		public ToolControl()
		{
			Size = new Size(600, 38);
		}

		public void UnTogleAll()
		{
			foreach (Control ctrl in Controls)
			{
				TpToolButton tpb = ctrl as TpToolButton;
				if (tpb != null)
					tpb.Togle = false;
			}
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			Color apper = Color.FromArgb(255, 89, 89, 89);
			Color lower = Color.FromArgb(255, 36, 36, 36);
			using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), apper, lower))
			{
				pevent.Graphics.FillRectangle(brush, 0, 0, Width, Height);
			}
			using (Pen pen = new Pen(Color.FromArgb(255, 18, 18, 18)))
			{
				pevent.Graphics.DrawLine(pen, 0, 0, Width, 0);
				pevent.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
			}
		}

		public TpToolButton AddToolButton(Bitmap pict, string toolTipText)
		{
			TpToolButton tb = new TpToolButton(pict);
			tb.Location = new Point(buttonOffset, (Height - tb.Height)/2);
			buttonOffset += tb.Width + 5;
			Controls.Add(tb);
			ToolTip tip = new ToolTip();
			tip.SetToolTip(tb, toolTipText);
			return tb;
		}

		public void AddVerticalSeparator()
		{
			TpVerticalSeporator vs = new TpVerticalSeporator();
			vs.Location = new Point(buttonOffset + 5, (Height - vs.Height)/2);
			buttonOffset += 11;
			Controls.Add(vs);
		}
	}
}