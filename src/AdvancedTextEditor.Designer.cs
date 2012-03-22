namespace TpTrayUtility
{
	partial class AdvancedTextEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedTextEditor));
			this.inputTextBox = new System.Windows.Forms.TextBox();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonSelectFont = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSelectBackColor = new System.Windows.Forms.ToolStripButton();
			this.fontDialog = new System.Windows.Forms.FontDialog();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputTextBox
			// 
			this.inputTextBox.AcceptsReturn = true;
			this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.inputTextBox.Location = new System.Drawing.Point(12, 28);
			this.inputTextBox.Multiline = true;
			this.inputTextBox.Name = "inputTextBox";
			this.inputTextBox.Size = new System.Drawing.Size(467, 102);
			this.inputTextBox.TabIndex = 0;
			this.inputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputTextBox_KeyPress);
			this.inputTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSelectFont,
            this.toolStripButtonSelectBackColor});
			this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(491, 23);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// toolStripButtonSelectFont
			// 
			this.toolStripButtonSelectFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSelectFont.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectFont.Image")));
			this.toolStripButtonSelectFont.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSelectFont.Name = "toolStripButtonSelectFont";
			this.toolStripButtonSelectFont.Size = new System.Drawing.Size(23, 20);
			this.toolStripButtonSelectFont.Text = "Select Font";
			this.toolStripButtonSelectFont.Click += new System.EventHandler(this.toolStripButtonSelectFont_Click);
			// 
			// toolStripButtonSelectBackColor
			// 
			this.toolStripButtonSelectBackColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSelectBackColor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectBackColor.Image")));
			this.toolStripButtonSelectBackColor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonSelectBackColor.Name = "toolStripButtonSelectBackColor";
			this.toolStripButtonSelectBackColor.Size = new System.Drawing.Size(23, 20);
			this.toolStripButtonSelectBackColor.Text = "Select Back Color";
			this.toolStripButtonSelectBackColor.Click += new System.EventHandler(this.toolStripButtonSelectBackColor_Click);
			// 
			// colorDialog
			// 
			this.colorDialog.Color = System.Drawing.Color.Red;
			// 
			// buttonOk
			// 
			this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonOk.Location = new System.Drawing.Point(277, 150);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(91, 29);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonCancel.Location = new System.Drawing.Point(388, 150);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(91, 29);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// AdvancedTextEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(491, 191);
			this.ControlBox = false;
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.inputTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AdvancedTextEditor";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "AdvancedTextEditor";
			this.TopMost = true;
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inputTextBox;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.FontDialog fontDialog;
		private System.Windows.Forms.ToolStripButton toolStripButtonSelectFont;
		private System.Windows.Forms.ToolStripButton toolStripButtonSelectBackColor;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
	}
}