using System.ComponentModel;

namespace TpTrayUtility
{
	partial class SecurityForm
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new TpTrayUtility.Components.TpLabel();
			this.label2 = new TpTrayUtility.Components.TpLabel();
			this.buttonLogin = new TpTrayUtility.Components.TpButton();
			this.textUserName = new TpTrayUtility.Components.TpTextBox();
			this.textPwd = new TpTrayUtility.Components.TpTextBox();
			this.checkBoxSavePwd = new TpTrayUtility.TpCheckBox();
			this.label3 = new TpTrayUtility.Components.TpLabel();
			this.label4 = new TpTrayUtility.Components.TpLabel();
			this.textBoxTPURL = new TpTrayUtility.Components.TpTextBox();
			this.checkBoxUseIntegratedAuth = new TpTrayUtility.TpCheckBox();
			this.btnCancel = new TpTrayUtility.Components.TpButton();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label1.ForeColor = System.Drawing.SystemColors.Control;
			this.label1.Location = new System.Drawing.Point(12, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Login/Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label2.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label2.ForeColor = System.Drawing.SystemColors.Control;
			this.label2.Location = new System.Drawing.Point(12, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Password";
			// 
			// buttonLogin
			// 
			this.buttonLogin.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.buttonLogin.Location = new System.Drawing.Point(352, 155);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(82, 26);
			this.buttonLogin.TabIndex = 4;
			this.buttonLogin.Text = "Login";
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// textUserName
			// 
			this.textUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
			this.textUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
			this.textUserName.BackColor = System.Drawing.Color.White;
			this.textUserName.Location = new System.Drawing.Point(133, 88);
			this.textUserName.Multiline = false;
			this.textUserName.Name = "textUserName";
			this.textUserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.textUserName.Size = new System.Drawing.Size(389, 16);
			this.textUserName.TabIndex = 2;
			this.textUserName.UseSystemPasswordChar = false;
			this.textUserName.Validated += new System.EventHandler(this.textUserName_Validated);
			// 
			// textPwd
			// 
			this.textPwd.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
			this.textPwd.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
			this.textPwd.BackColor = System.Drawing.Color.White;
			this.textPwd.Location = new System.Drawing.Point(133, 118);
			this.textPwd.Multiline = false;
			this.textPwd.Name = "textPwd";
			this.textPwd.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.textPwd.Size = new System.Drawing.Size(389, 16);
			this.textPwd.TabIndex = 3;
			this.textPwd.UseSystemPasswordChar = true;
			this.textPwd.Validated += new System.EventHandler(this.textPwd_Validated);
			// 
			// checkBoxSavePwd
			// 
			this.checkBoxSavePwd.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxSavePwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBoxSavePwd.Location = new System.Drawing.Point(136, 145);
			this.checkBoxSavePwd.Name = "checkBoxSavePwd";
			this.checkBoxSavePwd.Size = new System.Drawing.Size(170, 17);
			this.checkBoxSavePwd.TabIndex = 6;
			this.checkBoxSavePwd.Text = "Save Password";
			this.checkBoxSavePwd.UseVisualStyleBackColor = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label3.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label3.ForeColor = System.Drawing.SystemColors.Control;
			this.label3.Location = new System.Drawing.Point(12, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "TargetProcess Server";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label4.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label4.ForeColor = System.Drawing.SystemColors.Control;
			this.label4.Location = new System.Drawing.Point(130, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(143, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "(e.g. http://localserver/TP2/)";
			// 
			// textBoxTPURL
			// 
			this.textBoxTPURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.textBoxTPURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
			this.textBoxTPURL.BackColor = System.Drawing.Color.White;
			this.textBoxTPURL.Location = new System.Drawing.Point(133, 43);
			this.textBoxTPURL.Multiline = false;
			this.textBoxTPURL.Name = "textBoxTPURL";
			this.textBoxTPURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.textBoxTPURL.Size = new System.Drawing.Size(389, 16);
			this.textBoxTPURL.TabIndex = 1;
			this.textBoxTPURL.UseSystemPasswordChar = false;
			this.textBoxTPURL.Validated += new System.EventHandler(this.textBoxTPURL_Validated);
			// 
			// checkBoxUseIntegratedAuth
			// 
			this.checkBoxUseIntegratedAuth.BackColor = System.Drawing.Color.Transparent;
			this.checkBoxUseIntegratedAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBoxUseIntegratedAuth.Location = new System.Drawing.Point(136, 168);
			this.checkBoxUseIntegratedAuth.Name = "checkBoxUseIntegratedAuth";
			this.checkBoxUseIntegratedAuth.Size = new System.Drawing.Size(170, 17);
			this.checkBoxUseIntegratedAuth.TabIndex = 7;
			this.checkBoxUseIntegratedAuth.Text = "Use my windows logon";
			this.checkBoxUseIntegratedAuth.UseVisualStyleBackColor = false;
			this.checkBoxUseIntegratedAuth.CheckedChanged += new System.EventHandler(this.checkBoxUseIntegratedAuth_CheckedChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.ButtonType = TpTrayUtility.Components.ButtonType.Grey;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(440, 155);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(82, 26);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// SecurityForm
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(547, 209);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.checkBoxUseIntegratedAuth);
			this.Controls.Add(this.checkBoxSavePwd);
			this.Controls.Add(this.textPwd);
			this.Controls.Add(this.textBoxTPURL);
			this.Controls.Add(this.textUserName);
			this.Controls.Add(this.buttonLogin);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Icon = global::TpTrayUtility.Properties.Resources.tp;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SecurityForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Login TargetProcess";
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.buttonLogin, 0);
			this.Controls.SetChildIndex(this.textUserName, 0);
			this.Controls.SetChildIndex(this.textBoxTPURL, 0);
			this.Controls.SetChildIndex(this.textPwd, 0);
			this.Controls.SetChildIndex(this.checkBoxSavePwd, 0);
			this.Controls.SetChildIndex(this.checkBoxUseIntegratedAuth, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TpTrayUtility.Components.TpLabel label1;
		private TpTrayUtility.Components.TpLabel label2;
		private TpTrayUtility.Components.TpButton buttonLogin;
		private TpTrayUtility.Components.TpTextBox textUserName;
		private TpTrayUtility.Components.TpTextBox textPwd;
		private TpCheckBox checkBoxSavePwd;
		private TpTrayUtility.Components.TpLabel label3;
		private TpTrayUtility.Components.TpLabel label4;
		private TpTrayUtility.Components.TpTextBox textBoxTPURL;
        private TpCheckBox checkBoxUseIntegratedAuth;
		private TpTrayUtility.Components.TpButton btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
	}
}