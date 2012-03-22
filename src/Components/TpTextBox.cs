// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TpTextBox : Control
	{
		public TextBox textBox = new TextBox();

		public new string Text
		{
			get { return textBox.Text; }
			set { textBox.Text = value; }
		}

		public bool UseSystemPasswordChar
		{
			get { return textBox.UseSystemPasswordChar; }
			set { textBox.UseSystemPasswordChar = value; }
		}

		public AutoCompleteMode AutoCompleteMode
		{
			get { return textBox.AutoCompleteMode; }
			set { textBox.AutoCompleteMode = value; }
		}

		public AutoCompleteSource AutoCompleteSource
		{
			get { return textBox.AutoCompleteSource; }
			set { textBox.AutoCompleteSource = value; }
		}

		public TpTextBox()
		{
			textBox.BorderStyle = BorderStyle.None;
			textBox.BackColor = Color.White;
			BackColor = Color.White;
			this.Controls.Add(textBox);
		}

		protected override void OnResize(EventArgs e)
		{
			textBox.Size = new Size(this.Width - 2, this.Height - 2);
			textBox.Location = new Point(1, 1);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
		}

		public bool Multiline
		{
			get { return textBox.Multiline; }
			set { textBox.Multiline = value; }
		}

		public ScrollBars ScrollBars
		{
			get { return textBox.ScrollBars; }
			set { textBox.ScrollBars = value; }
		}
	}
}