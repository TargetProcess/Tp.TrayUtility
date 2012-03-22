using TpTrayUtility.Components;
using TpTrayUtility.Controls;

namespace TpTrayUtility
{
	partial class SubmitBugForm
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
			this.label1 = new TpTrayUtility.Components.TpLabel();
			this.label2 = new TpTrayUtility.Components.TpLabel();
			this.textBoxBugName = new TpTrayUtility.Components.TpTextBox();
			this.textBoxBugDescription = new TpTrayUtility.Components.TpTextBox();
			this.comboBoxProject = new TpTrayUtility.Controls.TpComboBox();
			this.label3 = new TpTrayUtility.Components.TpLabel();
			this.label5 = new TpTrayUtility.Components.TpLabel();
			this.label6 = new TpTrayUtility.Components.TpLabel();
			this.label7 = new TpTrayUtility.Components.TpLabel();
			this.buttonSubmit = new TpTrayUtility.Components.TpButton();
			this.buttonCancel = new TpTrayUtility.Components.TpButton();
			this.comboBoxUserStory = new TpTrayUtility.Controls.TpComboBox();
			this.comboBoxSeverity = new TpTrayUtility.Controls.TpComboBox();
			this.comboBoxPriority = new TpTrayUtility.Controls.TpComboBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tpButton1 = new TpTrayUtility.Components.TpButton();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(653, 7);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label1.ForeColor = System.Drawing.Color.Silver;
			this.label1.Location = new System.Drawing.Point(220, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Bug Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label2.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label2.ForeColor = System.Drawing.Color.Silver;
			this.label2.Location = new System.Drawing.Point(220, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Bug Description";
			// 
			// textBoxBugName
			// 
			this.textBoxBugName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
			this.textBoxBugName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
			this.textBoxBugName.BackColor = System.Drawing.Color.White;
			this.textBoxBugName.Location = new System.Drawing.Point(221, 58);
			this.textBoxBugName.Multiline = false;
			this.textBoxBugName.Name = "textBoxBugName";
			this.textBoxBugName.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.textBoxBugName.Size = new System.Drawing.Size(445, 20);
			this.textBoxBugName.TabIndex = 1;
			this.textBoxBugName.UseSystemPasswordChar = false;
			// 
			// textBoxBugDescription
			// 
			this.textBoxBugDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
			this.textBoxBugDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
			this.textBoxBugDescription.BackColor = System.Drawing.Color.White;
			this.textBoxBugDescription.Location = new System.Drawing.Point(223, 98);
			this.textBoxBugDescription.Multiline = true;
			this.textBoxBugDescription.Name = "textBoxBugDescription";
			this.textBoxBugDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxBugDescription.Size = new System.Drawing.Size(443, 206);
			this.textBoxBugDescription.TabIndex = 2;
			this.textBoxBugDescription.UseSystemPasswordChar = false;
			// 
			// comboBoxProject
			// 
			this.comboBoxProject.IsLoadingEntities = false;
			this.comboBoxProject.Location = new System.Drawing.Point(291, 310);
			this.comboBoxProject.Name = "comboBoxProject";
			this.comboBoxProject.SelectedIndex = -1;
			this.comboBoxProject.Size = new System.Drawing.Size(375, 21);
			this.comboBoxProject.TabIndex = 3;
			this.comboBoxProject.SelectedIndexChanged += new System.EventHandler(this.comboBoxProject_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label3.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label3.ForeColor = System.Drawing.Color.Silver;
			this.label3.Location = new System.Drawing.Point(220, 314);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 17);
			this.label3.TabIndex = 5;
			this.label3.Text = "Project";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label5.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label5.ForeColor = System.Drawing.Color.Silver;
			this.label5.Location = new System.Drawing.Point(220, 341);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 17);
			this.label5.TabIndex = 7;
			this.label5.Text = "User Story";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label6.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label6.ForeColor = System.Drawing.Color.Silver;
			this.label6.Location = new System.Drawing.Point(220, 368);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 17);
			this.label6.TabIndex = 8;
			this.label6.Text = "Severity";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
			this.label7.Font = new System.Drawing.Font("Arial Narrow", 10F);
			this.label7.ForeColor = System.Drawing.Color.Silver;
			this.label7.Location = new System.Drawing.Point(220, 395);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(42, 17);
			this.label7.TabIndex = 9;
			this.label7.Text = "Priority";
			// 
			// buttonSubmit
			// 
			this.buttonSubmit.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.buttonSubmit.Location = new System.Drawing.Point(223, 436);
			this.buttonSubmit.Name = "buttonSubmit";
			this.buttonSubmit.Size = new System.Drawing.Size(153, 25);
			this.buttonSubmit.TabIndex = 11;
			this.buttonSubmit.Text = "Submit";
			this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(586, 436);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 25);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// comboBoxUserStory
			// 
			this.comboBoxUserStory.IsLoadingEntities = false;
			this.comboBoxUserStory.Location = new System.Drawing.Point(291, 337);
			this.comboBoxUserStory.Name = "comboBoxUserStory";
			this.comboBoxUserStory.SelectedIndex = -1;
			this.comboBoxUserStory.Size = new System.Drawing.Size(375, 21);
			this.comboBoxUserStory.TabIndex = 4;
			this.comboBoxUserStory.DropDownOpened += new System.EventHandler(this.comboBoxUserStory_DropDownOpened);
			// 
			// comboBoxSeverity
			// 
			this.comboBoxSeverity.IsLoadingEntities = false;
			this.comboBoxSeverity.Location = new System.Drawing.Point(291, 364);
			this.comboBoxSeverity.Name = "comboBoxSeverity";
			this.comboBoxSeverity.SelectedIndex = -1;
			this.comboBoxSeverity.Size = new System.Drawing.Size(375, 21);
			this.comboBoxSeverity.TabIndex = 5;
			// 
			// comboBoxPriority
			// 
			this.comboBoxPriority.IsLoadingEntities = false;
			this.comboBoxPriority.Location = new System.Drawing.Point(291, 391);
			this.comboBoxPriority.Name = "comboBoxPriority";
			this.comboBoxPriority.SelectedIndex = -1;
			this.comboBoxPriority.Size = new System.Drawing.Size(375, 21);
			this.comboBoxPriority.TabIndex = 6;
			// 
			// tpButton1
			// 
			this.tpButton1.ButtonType = TpTrayUtility.Components.ButtonType.Default;
			this.tpButton1.Location = new System.Drawing.Point(406, 436);
			this.tpButton1.Name = "tpButton1";
			this.tpButton1.Size = new System.Drawing.Size(146, 25);
			this.tpButton1.TabIndex = 11;
			this.tpButton1.Text = "Submit & View";
			this.tpButton1.Click += new System.EventHandler(this.buttonSubmitView_Click);
			// 
			// SubmitBugForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(678, 497);
			this.Controls.Add(this.comboBoxPriority);
			this.Controls.Add(this.comboBoxSeverity);
			this.Controls.Add(this.comboBoxUserStory);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.tpButton1);
			this.Controls.Add(this.buttonSubmit);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboBoxProject);
			this.Controls.Add(this.textBoxBugDescription);
			this.Controls.Add(this.textBoxBugName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = global::TpTrayUtility.Properties.Resources.tp;
			this.Name = "SubmitBugForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Submit Bug";
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.textBoxBugName, 0);
			this.Controls.SetChildIndex(this.textBoxBugDescription, 0);
			this.Controls.SetChildIndex(this.comboBoxProject, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.label5, 0);
			this.Controls.SetChildIndex(this.label6, 0);
			this.Controls.SetChildIndex(this.label7, 0);
			this.Controls.SetChildIndex(this.buttonSubmit, 0);
			this.Controls.SetChildIndex(this.tpButton1, 0);
			this.Controls.SetChildIndex(this.buttonCancel, 0);
			this.Controls.SetChildIndex(this.comboBoxUserStory, 0);
			this.Controls.SetChildIndex(this.comboBoxSeverity, 0);
			this.Controls.SetChildIndex(this.comboBoxPriority, 0);
			this.Controls.SetChildIndex(this.closeButton, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TpLabel label1;
		private TpLabel label2;
		private TpTextBox textBoxBugName;
		private TpTextBox textBoxBugDescription;
		private TpComboBox comboBoxProject;
		private TpLabel label3;
		private TpLabel label5;
		private TpLabel label6;
		private TpLabel label7;
		private TpButton buttonSubmit;
		private TpButton buttonCancel;
		private TpComboBox comboBoxUserStory;
		private TpComboBox comboBoxSeverity;
		private TpComboBox comboBoxPriority;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private TpButton tpButton1;
	}
}