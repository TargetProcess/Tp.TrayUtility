using System;
namespace TpTrayUtility
{
	partial class TPEMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPEMain));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemAddBug = new System.Windows.Forms.ToolStripMenuItem();
			this.toDoList = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipText = "Balloon Tip Text.";
			this.notifyIcon1.BalloonTipTitle = "Balloon Tip Title";
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = Properties.Resources.tp;
			this.notifyIcon1.Text = "TP.Tray";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
			this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripMenuItem3,
            this.toolStripMenuItemAddBug,
            /*this.toDoList,*/
            this.toolStripMenuItemSetting,
            this.toolStripMenuItem2,
            this.toolStripSeparator2,
            this.toolStripMenuAbout,
            this.toolStripSeparator1,
            this.toolStripMenuItem1});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(213, 192);
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(212, 22);
			this.toolStripTextBox1.Text = "Capture Screenshot";
			this.toolStripTextBox1.ToolTipText = "Capture new screenshot discarding previous actions.";
			this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
			// 
			// toolStripMenuItem3
			// 
			//this.toolStripMenuItem3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuItem3.Text = "Screenshot Editor";
			this.toolStripMenuItem3.ToolTipText = "Add captured screenshot to previois scene.";
			this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripTextBox2_Click);
			// 
			// toolStripMenuItemAddBug
			// 
			this.toolStripMenuItemAddBug.Name = "toolStripMenuItemAddBug";
			this.toolStripMenuItemAddBug.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuItemAddBug.Text = "Add Bug";
			this.toolStripMenuItemAddBug.Click += new System.EventHandler(this.toolStripMenuItemAddBug_Click);
			// 
			// toDoList
			// 
			this.toDoList.Name = "toDoList";
			this.toDoList.Size = new System.Drawing.Size(212, 22);
			this.toDoList.Text = "ToDo List";
			this.toDoList.Click += new System.EventHandler(this.toDo_Click);
			// 
			// toolStripMenuItemSetting
			// 
			this.toolStripMenuItemSetting.Name = "toolStripMenuItemSetting";
			this.toolStripMenuItemSetting.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuItemSetting.Text = "Login";
			this.toolStripMenuItemSetting.Click += new System.EventHandler(this.toolStripMenuItemSetting_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuItem2.Text = "Define hotkeys";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
			// 
			// toolStripMenuAbout
			// 
			this.toolStripMenuAbout.Name = "toolStripMenuAbout";
			this.toolStripMenuAbout.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuAbout.Text = "About";
			this.toolStripMenuAbout.Click += new System.EventHandler(this.toolStripMenuAbout_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(212, 22);
			this.toolStripMenuItem1.Text = "Exit";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.MainMenu_ExitClick);
			// 
			// TPEMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 400);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = Properties.Resources.tp;
			this.Name = "TPEMain";
			this.Text = "TP.Tray";
			//this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripTextBox1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSetting;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAbout;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddBug;
        private System.Windows.Forms.ToolStripMenuItem toDoList;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
	}
}

