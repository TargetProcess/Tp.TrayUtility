// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility.SubmitBug
{
	public class AttachmentControlScrollButton : TpBaseButton
	{
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Color color1;
			Color color2;
			if (isOver)
			{
				color1 = Color.FromArgb(255, 72, 72, 72);
				color2 = Color.FromArgb(255, 64, 64, 64);
			}
			else
			{
				color1 = Color.FromArgb(255, 64, 64, 64);
				color2 = Color.FromArgb(255, 56, 56, 56);
			}

			using (var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), color1, color2))
			{
				pevent.Graphics.FillRectangle(brush, 0, 0, Width, Height);
			}

			if (!isPressed)
			{
				pevent.Graphics.DrawLine(Pens.DarkGray, 0, 0, Width, 0);
				pevent.Graphics.DrawLine(Pens.DimGray, 0, 0, 0, Height);
				pevent.Graphics.DrawLine(Pens.Black, Width - 1, 0, Width - 1, Height);
				pevent.Graphics.DrawLine(Pens.Black, 0, Height - 1, Width, Height - 1);
			}
		}
	}
}