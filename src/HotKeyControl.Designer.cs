namespace TpTrayUtility
{
	partial class HotKeyControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.hotKeyField = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// hotKeyField
			// 
			this.hotKeyField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.hotKeyField.Location = new System.Drawing.Point(0, 0);
			this.hotKeyField.Name = "hotKeyField";
			this.hotKeyField.ReadOnly = true;
			this.hotKeyField.Size = new System.Drawing.Size(217, 20);
			this.hotKeyField.TabIndex = 25;
			this.hotKeyField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.captureBugHotKey_KeyUp);
			this.hotKeyField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.captureBugHotKey_KeyDown);
			// 
			// HotKeyControl
			// 
			//this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			//this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.hotKeyField);
			this.Name = "HotKeyControl";
			this.Size = new System.Drawing.Size(218, 20);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.captureBugHotKey_KeyUp);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.captureBugHotKey_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox hotKeyField;
	}
}
