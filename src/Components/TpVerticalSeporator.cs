// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class TpVerticalSeporator : Control
	{
		public TpVerticalSeporator()
		{
			Size = new Size(2, 24);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.White, 0, 0, 0, Height);
			e.Graphics.DrawLine(Pens.Black, 1, 0, 1, Height);
		}
	}
}