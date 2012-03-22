//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class TpTextItem : TpItem
	{
		private readonly string value;
		private readonly TpLabel lbl;

		public TpTextItem(string text)
		{
			BackColor = Color.White;

			Tag = text;
			value = text;
			lbl = new TpLabel
			      	{
			      		Text = text,
			      		Location = new Point(0, 0),
			      		Size = new Size(100, 20)
			      	};

			lbl.MouseEnter += lbl_MouseEnter;
			lbl.MouseLeave += lbl_MouseLeave;

			lbl.MouseClick += lbl_MouseClick;


			Controls.Add(lbl);
		}

		public override void SpecialDrawingFunction(Graphics g, Rectangle rc)
		{
			g.DrawString(value, UI.DefaultFont, Brushes.Black, rc);
		}

		public override bool Selected
		{
			set
			{
				base.Selected = value;
				Invalidate();
				lbl.BackColor = Selected ? Color.LightGreen : Color.White;
			}
		}

		private void lbl_MouseLeave(object sender, EventArgs e)
		{
			lbl.BackColor = Selected ? Color.LightGreen : Color.White;
			Invalidate();
		}

		private void lbl_MouseEnter(object sender, EventArgs e)
		{
			Invalidate();
			lbl.BackColor = Color.LightBlue;
		}

		protected override void OnResize(EventArgs e)
		{
			if (lbl != null)
				lbl.Size = Size;
			base.OnResize(e);
		}

		private void lbl_MouseClick(object sender, MouseEventArgs e)
		{
			OnMouseClick(e);
		}

		public override TpItem Clone()
		{
			return new TpTextItem(value);
		}

		public override bool Contain(object data)
		{
			return data.ToString() == Tag.ToString();
		}


		public static int Compare(TpItem item1, TpItem item2)
		{
			return string.Compare(((TpTextItem) item1).value, ((TpTextItem) item2).value);
		}
	}
}