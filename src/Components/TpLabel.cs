// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TpLabel : Label
	{
		public TpLabel()
		{
			Font = new Font("Arial Narrow", 10);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			if (Parent != null)
				BackColor = Parent.BackColor;
			base.OnParentChanged(e);
		}

		protected override void OnParentBackColorChanged(EventArgs e)
		{
			BackColor = Parent.BackColor;
			base.OnParentBackColorChanged(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			base.OnPaint(e);
		}

		protected override void OnClick(EventArgs e)
		{
		}
	}
}