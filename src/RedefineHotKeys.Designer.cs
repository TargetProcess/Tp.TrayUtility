using TpTrayUtility.Components;
namespace TpTrayUtility
{
	partial class RedefineHotKeys
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
			this.label3 = new TpTrayUtility.Components.TpLabel();
			this.label1 = new TpTrayUtility.Components.TpLabel();
			this.buttonApply = new TpTrayUtility.Components.TpButton();
			this.hotKeyControl2 = new TpTrayUtility.HotKeyControl();
			this.hotKeyControl1 = new TpTrayUtility.HotKeyControl();
			this.checkBox1 = new TpTrayUtility.TpCheckBox();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.ForeColor = System.Drawing.SystemColors.Control;
			this.label3.Location = new System.Drawing.Point(25, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 23;
			this.label3.Text = "Capture screenshot";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label1.ForeColor = System.Drawing.SystemColors.Control;
			this.label1.Location = new System.Drawing.Point(25, 84);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 17);
			this.label1.TabIndex = 23;
			this.label1.Text = "Open dashboard";
			// 
			// buttonApply
			// 
			this.buttonApply.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.buttonApply.Location = new System.Drawing.Point(225, 152);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.Size = new System.Drawing.Size(128, 26);
			this.buttonApply.TabIndex = 1;
			this.buttonApply.Text = "Close";
			this.buttonApply.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// hotKeyControl2
			// 
			this.hotKeyControl2.Location = new System.Drawing.Point(135, 84);
			this.hotKeyControl2.Name = "hotKeyControl2";
			this.hotKeyControl2.Size = new System.Drawing.Size(218, 20);
			this.hotKeyControl2.TabIndex = 4;
			// 
			// hotKeyControl1
			// 
			this.hotKeyControl1.Location = new System.Drawing.Point(135, 44);
			this.hotKeyControl1.Name = "hotKeyControl1";
			this.hotKeyControl1.Size = new System.Drawing.Size(218, 20);
			this.hotKeyControl1.TabIndex = 3;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.checkBox1.ForeColor = System.Drawing.SystemColors.Control;
			this.checkBox1.Location = new System.Drawing.Point(135, 125);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(113, 17);
			this.checkBox1.TabIndex = 24;
			this.checkBox1.Text = "Enable hotkeys";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.Checked = SettingsManager.HotKeyEnabled;
			this.checkBox1.Click += new System.EventHandler(checkBox1_Click);
			// 
			// RedefineHotKeys
			// 
			this.AcceptButton = this.buttonApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 190);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.buttonApply);
			this.Controls.Add(this.hotKeyControl2);
			this.Controls.Add(this.hotKeyControl1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Icon = global::TpTrayUtility.Properties.Resources.tp;
			this.Name = "RedefineHotKeys";
			this.Text = "Define HotKeys";
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.hotKeyControl1, 0);
			this.Controls.SetChildIndex(this.hotKeyControl2, 0);
			this.Controls.SetChildIndex(this.buttonApply, 0);
			this.Controls.SetChildIndex(this.checkBox1, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		#endregion

		private TpTrayUtility.Components.TpLabel label3;
		private HotKeyControl hotKeyControl1;
		private HotKeyControl hotKeyControl2;
		private TpTrayUtility.Components.TpLabel label1;
		private TpTrayUtility.Components.TpButton buttonApply;
		private TpCheckBox checkBox1;
	}
}