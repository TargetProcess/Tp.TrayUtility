namespace TpTrayUtility
{
	partial class Magnify
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
			this.SuspendLayout();
			// 
			// Magnify
			// 
			//this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			//this.ClientSize = new System.Drawing.Size(292, 273);
			//this.ControlBox = false;
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			//this.MaximizeBox = false;
			//this.MinimizeBox = false;
			this.DoubleBuffered = true;
			this.Name = "Magnify";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Capture Utility";
			this.TopMost = true;
			this.ShowInTaskbar = false;
			//this.Activated += new System.EventHandler(this.Magnify_Activated);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Magnify_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion
	}
}