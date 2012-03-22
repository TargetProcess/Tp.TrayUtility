// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TpItem : Control
	{
		public event EventHandler ItemClicked = null;

		public virtual bool Selected { get; set; }

		public bool Filtered { get; set; }

		public TpItem()
		{
			Size = new Size(100, 20);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked.Invoke(this, new EventArgs());
		}

		public virtual TpItem Clone()
		{
			return new TpItem();
		}

		public virtual bool Contain(object data)
		{
			return Tag == data;
		}

		public virtual void SpecialDrawingFunction(Graphics g, Rectangle rc)
		{
		}
	}
}