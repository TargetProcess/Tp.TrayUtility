// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class TpForm : Form
	{
		private Point mouseOffset;
		private bool isMoveWindow;
		private bool isResizeWindow;

		private bool drawBackGround = true;
		private bool drawCaption = true;
		private bool drawCloseButton;

		private bool isResizeble;

		public bool DrawBackGround
		{
			get { return drawBackGround; }
			set { drawBackGround = value; }
		}

		public bool DrawCaption
		{
			get { return drawCaption; }
			set { drawCaption = value; }
		}

		public bool DrawCloseButton
		{
			get { return drawCloseButton; }
			set { drawCloseButton = value; }
		}

		public bool Resizeble
		{
			get { return isResizeble; }
			set
			{
				isResizeble = value;
				resizeButton.Visible = value;
				Invalidate();
			}
		}

		protected TpCloseButton closeButton = new TpCloseButton();
		private readonly TpMaxiMiniButton resizeButton = new TpMaxiMiniButton();

		public static int HeaderHeight = 32;
		public static int BorderSize = 10;

		private const int CS_DROPSHADOW = 0x00020000;

		// Override the CreateParams property
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= CS_DROPSHADOW;
				return cp;
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			resizeButton.Minimize = this.WindowState == FormWindowState.Maximized;
		}


		public TpForm()
		{
			BackColor = Color.FromArgb(255, 48, 48, 48);

			FormBorderStyle = FormBorderStyle.None;
			DoubleBuffered = true;

			this.MouseDown += this.Form1_MouseDown;
			this.MouseUp += this.Form1_MouseUp;
			this.MouseMove += this.Form1_MouseMove;
			this.MouseDoubleClick += TpForm_MouseDoubleClick;			


			closeButton.Location = new Point(Width - closeButton.Width - 10, 7);
			closeButton.Click += closeButton_Click;
			Controls.Add(closeButton);

			resizeButton.Location = new Point(Width - closeButton.Width - 30, 7);
			resizeButton.Click += resizeButton_Click;
			resizeButton.Visible = false;
			resizeButton.Minimize = this.WindowState == FormWindowState.Maximized;
			Controls.Add(resizeButton);
		}

		protected override void OnResize(EventArgs e)
		{
			Region = new Region(RoundedRectangle.Create(0, 0, Width, Height, 7));

			closeButton.Location = new Point(Width - closeButton.Width - 10, 7);
			resizeButton.Location = new Point(Width - closeButton.Width - 30, 7);

			base.OnResize(e);
		}


		private Point lastPosition = new Point(0, 0);
		private Size lastSize = new Size(0, 0);

		private void resizeButton_Click(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				MaximumSize = new Size(int.MaxValue, int.MaxValue);
				this.WindowState = FormWindowState.Normal;
			}
			else
			{
				Point pt = Cursor.Position;
				Screen currentScreen;
				currentScreen = Screen.FromPoint(pt);

				// test whether the screen is a primary display
				if (currentScreen.Primary)
				{
					MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
				}
				else
				{
					MaximumSize = new Size(int.MaxValue, int.MaxValue);
				}
				this.WindowState = FormWindowState.Maximized;
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			int xOffset;
			int yOffset;

			if (e.Button == MouseButtons.Left && e.Y < 25 && DrawCaption)
			{
				xOffset = -e.X;
				yOffset = -e.Y;
				mouseOffset = new Point(xOffset, yOffset);
				isMoveWindow = true;
			}
			else if (e.Button == MouseButtons.Left && e.Y > Height - 20 && e.X > Width - 20 && Resizeble)
			{
				xOffset = -e.X;
				yOffset = -e.Y;
				mouseOffset = new Point(xOffset, yOffset);
				isResizeWindow = true;
			}
		}

		private void TpForm_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.Y < 25 && DrawCaption && isResizeble)
			{
				resizeButton_Click(null, null);
			}
		}


		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Y < 25 && DrawCaption)
			{
				Cursor = Cursors.Hand;
			}
			else if (e.Y > Height - 20 && e.X > Width - 20 && Resizeble)
			{
				Cursor = Cursors.SizeNWSE;
			}
			else
			{
				Cursor = Cursors.Arrow;
			}

			if (isMoveWindow)
			{
				Point mousePos = MousePosition;
				mousePos.Offset(mouseOffset.X, mouseOffset.Y);
				Location = mousePos;
			}

			if (isResizeWindow)
			{
				Point mousePos = MousePosition;
				mousePos.Offset(mouseOffset.X, mouseOffset.Y);
				Size = new Size(e.X, e.Y);
				Update();
				Invalidate();
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				isMoveWindow = false;
				isResizeWindow = false;
				Invalidate();
			}
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			if (Resizeble)
			{
				for (int zx = 0; zx < 5; zx++)
				{
					for (int zy = 0; zy < 5; zy++)
					{
						if (zx >= 5 - zy)
							DrawShadowedPoint(Width - 15 + zx*2, Height - 15 + zy*2, e.Graphics);
					}
				}
			}
			using (Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 1))
			{
				e.Graphics.DrawPath(pen, RoundedRectangle.Create(0, 0, Width, Height, 7));
			}


			if (DrawCaption)
			{
				e.Graphics.DrawImage(Resources.TPIcon, new Point(10, 10));

				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				e.Graphics.DrawString(Text, UI.DefaultFont, Brushes.White, new Rectangle(31, 10, Width - 25, 18), sf);
			}
		}

		protected void DrawShadowedPoint(int x, int y, Graphics g)
		{
			g.FillRectangle(Brushes.Black, x, y, 1, 1);
			g.FillRectangle(Brushes.LightGray, x + 1, y, 1, 1);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (DrawBackGround)
			{
				e.Graphics.Clear(BackColor);
			}
		}
	}
}