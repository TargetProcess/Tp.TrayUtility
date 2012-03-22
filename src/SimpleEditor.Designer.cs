using TpTrayUtility.Components;
using System.Windows.Forms;
using TpTrayUtility.Controls;

namespace TpTrayUtility
{
	partial class SimpleEditor
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
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.SuspendLayout();
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "JPeg";
			this.saveFileDialog.Filter = "JPEG Image File (*.jpg) |*.jpg|Portable Network Graphics (*.png) |*.png|Bitmaps (" +
				"*.bmp) |*.bmp";
			this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
			// 
			// SimpleEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.Icon = global::TpTrayUtility.Properties.Resources.tp;
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(250, 100);
			this.Name = "SimpleEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Editor";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SimpleEditor_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ColorDialog colorDialog;

	}
}