namespace TpTrayUtility.Components
{
	partial class StatusForm
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
			this.lblMessage = new TpTrayUtility.Components.TpLabel();
			this.btnClose = new TpTrayUtility.Components.TpButton();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(413, 7);
			this.closeButton.Visible = false;
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.lblMessage.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.lblMessage.ForeColor = System.Drawing.Color.Silver;
			this.lblMessage.Location = new System.Drawing.Point(12, 37);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(29, 17);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "Text";
			// 
			// btnClose
			// 
			this.btnClose.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.btnClose.Location = new System.Drawing.Point(351, 55);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(77, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// StatusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.ClientSize = new System.Drawing.Size(438, 90);
			this.ControlBox = false;
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnClose);
			this.Name = "StatusForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "";
			this.TopMost = true;
			this.Controls.SetChildIndex(this.btnClose, 0);
			this.Controls.SetChildIndex(this.lblMessage, 0);
			this.Controls.SetChildIndex(this.closeButton, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TpButton btnClose;
		private TpLabel lblMessage;
	}
}