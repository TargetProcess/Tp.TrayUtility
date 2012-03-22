namespace TpTrayUtility
{
	partial class TimeDescriptionForm
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
			this.timeDescription = new System.Windows.Forms.TextBox();
			this.startTime = new System.Windows.Forms.TextBox();
			this.finishTime = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.totalTime = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timeDescription
			// 
			this.timeDescription.Location = new System.Drawing.Point(12, 6);
			this.timeDescription.Multiline = true;
			this.timeDescription.Name = "timeDescription";
			this.timeDescription.Size = new System.Drawing.Size(456, 148);
			this.timeDescription.TabIndex = 0;
			// 
			// startTime
			// 
			this.startTime.Location = new System.Drawing.Point(56, 160);
			this.startTime.Name = "startTime";
			this.startTime.Size = new System.Drawing.Size(117, 20);
			this.startTime.TabIndex = 1;
			this.startTime.TextChanged += new System.EventHandler(this.time_TextChanged);
			// 
			// finishTime
			// 
			this.finishTime.Location = new System.Drawing.Point(226, 160);
			this.finishTime.Name = "finishTime";
			this.finishTime.Size = new System.Drawing.Size(122, 20);
			this.finishTime.TabIndex = 1;
			this.finishTime.TextChanged += new System.EventHandler(this.time_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 163);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Started";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(179, 163);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Finished";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(354, 163);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Total";
			// 
			// totalTime
			// 
			this.totalTime.Location = new System.Drawing.Point(391, 160);
			this.totalTime.Name = "totalTime";
			this.totalTime.ReadOnly = true;
			this.totalTime.Size = new System.Drawing.Size(77, 20);
			this.totalTime.TabIndex = 1;
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(12, 223);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(119, 23);
			this.OkButton.TabIndex = 3;
			this.OkButton.Text = "Ok";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.Location = new System.Drawing.Point(349, 223);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(119, 23);
			this.CancelButton.TabIndex = 3;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// TimeDescriptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(480, 258);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.totalTime);
			this.Controls.Add(this.finishTime);
			this.Controls.Add(this.startTime);
			this.Controls.Add(this.timeDescription);
			this.Name = "TimeDescriptionForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox timeDescription;
		private System.Windows.Forms.TextBox startTime;
		private System.Windows.Forms.TextBox finishTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox totalTime;
		private System.Windows.Forms.Button OkButton;
		private new System.Windows.Forms.Button CancelButton;
	}
}