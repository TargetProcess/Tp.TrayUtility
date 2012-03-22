// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class ShadowForm : Form
	{
		public ShadowForm()
		{
			BackColor = Color.FromArgb(255, 0, 0, 0);
			FormBorderStyle = FormBorderStyle.None;
			DoubleBuffered = true;
			Opacity = 0.8;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Region = new Region(RoundedRectangle.Create(0, 0, Width, Height, 7));
			e.Graphics.Clear(Color.Transparent);
		}
	}
}