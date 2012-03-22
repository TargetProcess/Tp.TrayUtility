//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility
{
	public partial class AdvancedTextEditor : Form
	{
		private event EventHandler InputTextChanged;

		public Color FontColor { get; set; }

		public Color FontBackColor { get; set; }


		public AdvancedTextEditor(EventHandler textChanged)
		{
			FontColor = Color.Red;
			FontBackColor = Color.White;
			InputTextChanged = textChanged;
			InitializeComponent();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			using (var brush = new SolidBrush(Color.FromArgb(255, 194, 228, 183)))
			{
				using (var pen = new Pen(Color.Green))
				{
					e.Graphics.FillRectangle(brush, ClientRectangle.Left, ClientRectangle.Bottom - 50, ClientRectangle.Right,
					                         ClientRectangle.Bottom - 50);
					e.Graphics.DrawLine(pen, ClientRectangle.Left, ClientRectangle.Bottom - 50, ClientRectangle.Right,
					                    ClientRectangle.Bottom - 50);
				}
			}
		}

		public void Reset()
		{
			InputText = "";
		}

		private void InputTextBox_TextChanged(object sender, EventArgs e)
		{
			InputTextChanged(sender, e);
		}

		public String InputText
		{
			get { return inputTextBox.Text; }
			set { inputTextBox.Text = value; }
		}

		public Font InputFont
		{
			get { return inputTextBox.Font; }
		}

		private void toolStripButtonSelectFont_Click(object sender, EventArgs e)
		{
			if (fontDialog.ShowDialog() == DialogResult.OK)
			{
				inputTextBox.Font = fontDialog.Font;
				InputTextChanged(sender, e);
			}
		}

		private void toolStripButtonSelectBackColor_Click(object sender, EventArgs e)
		{
			colorDialog.Color = FontBackColor;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				FontBackColor = colorDialog.Color;
				InputTextChanged(sender, e);
			}
		}

		private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 10 && ModifierKeys == Keys.Control)
			{
				FinishText();
			}
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			FinishText();
		}

		private void FinishText()
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}