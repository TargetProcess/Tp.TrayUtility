//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Properties;

namespace TpTrayUtility
{
	public partial class Magnify : TpForm
	{
		private readonly Bitmap bmpSrc;
		private readonly Graphics gfxSrc;

		private readonly Pen redPen = new Pen(Color.Red, 1);
		private int hInfo;
		private int vInfo;

		private readonly TpButton snap = new TpButton();

		private readonly TpButton cobine = new TpButton();

		private const int GWL_EXSTYLE = (-20);
		private const int WS_EX_TOOLWINDOW = 0x80;
		private const int WS_EX_APPWINDOW = 0x40000;


		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int
		                                                                 	dwNewLong);

		public Magnify()
		{
			bmpSrc = new Bitmap(80, 80, PixelFormat.Format32bppArgb);
			gfxSrc = Graphics.FromImage(bmpSrc);
			gfxSrc.InterpolationMode = InterpolationMode.NearestNeighbor;
			InitializeComponent();
			Size = new Size(344, 400);
			Location = new Point(Screen.PrimaryScreen.Bounds.Right - Size.Width, Screen.PrimaryScreen.Bounds.Bottom - Size.Height);
			Closing += Magnify_Closing;
			
			cobine.ButtonType = ButtonType.Default;
			cobine.Size = new Size(100, 26);
			cobine.Location = new Point(Width - 14 - cobine.Width, Height - 36);
			cobine.Text = "Capture";
			cobine.MouseDown += combine_Click;
			var cobineToolTip = new ToolTip {ShowAlways = true};
			cobineToolTip.SetToolTip(cobine, "Capture picture and put it to library.");

			Controls.Add(cobine);


			snap.ButtonType = ButtonType.Default;
			snap.Size = new Size(120, 26);
			snap.Location = new Point(Width - 14 - snap.Width - 10 - cobine.Width, Height - 36);
			snap.Text = @"Capture & Edit";			
			snap.MouseDown += snap_Click;

			var snapToolTip = new ToolTip();
			snapToolTip.ShowAlways = true;
			snapToolTip.SetToolTip(snap, "Capture picture and edit it.");
			Controls.Add(snap);


			ShowInTaskbar = false;
			SetWindowLong(Handle, GWL_EXSTYLE,
			              (GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
		}


		protected override void OnActivated(EventArgs e)
		{
			SetWindowLong(Handle, GWL_EXSTYLE,
			              (GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
			base.OnActivated(e);
		}


		private static void combine_Click(object sender, EventArgs e)
		{
			FormsManager.GetInstance<TPEMain>().CaptureToLib();
		}

		private static void snap_Click(object sender, EventArgs e)
		{
			FormsManager.GetInstance<TPEMain>().SwitchToEditor();
		}


		private void Magnify_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			FormsManager.Hide(typeof (Magnify));
			FormsManager.Hide(typeof (TPEMain));
		}

		public void UpdatePicture(Bitmap bmp, int x, int y, int w, int h)
		{
			gfxSrc.Clear(Color.Black);
			gfxSrc.CopyFromScreen(x - 40, y - 40, 0, 0, bmpSrc.Size, CopyPixelOperation.SourceCopy);

			Text = string.Format("Capture Utility - Cursor({0}:{1})", x, y);

			vInfo = h;
			hInfo = w;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var destRect = new Rectangle(BorderSize, HeaderHeight, 324, 324);
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			e.Graphics.DrawImage(bmpSrc, destRect, 0, 0, 80, 80, GraphicsUnit.Pixel);

			e.Graphics.DrawLine(redPen, destRect.Left + 160, destRect.Top, destRect.Left + 160, destRect.Top + 320);
			e.Graphics.DrawLine(redPen, destRect.Left + 164, destRect.Top, destRect.Left + 164, destRect.Top + 320);
			e.Graphics.DrawLine(redPen, destRect.Left, destRect.Top + 160, destRect.Left + 320, destRect.Top + 160);
			e.Graphics.DrawLine(redPen, destRect.Left, destRect.Top + 164, destRect.Left + 320, destRect.Top + 164);

			e.Graphics.DrawImage(Resources.PictHorizontal, 10, 372);
			e.Graphics.DrawString(hInfo.ToString(), UI.DefaultFont, new SolidBrush(Color.Black), 26, 370);
			e.Graphics.DrawImage(Resources.PictVertical, 60, 370);
			e.Graphics.DrawString(vInfo.ToString(), UI.DefaultFont, new SolidBrush(Color.Black), 70, 370);
		}

		private void Magnify_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				FormsManager.Hide(typeof (Magnify));
				FormsManager.Hide(typeof (TPEMain));
			}

			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
			{
				FormsManager.GetInstance<TPEMain>().SwitchToEditor();
			}
		}
	}


	public class Magnify2 : Control
	{
		private readonly Bitmap bmpSrc;
		private readonly Graphics gfxSrc;

		private readonly Pen redPen = new Pen(Color.Red, 1);
		private int hInfo;
		private int vInfo;

		private readonly TpButton snap = new TpButton();
		private readonly TpButton cobine = new TpButton();

		private readonly TpCloseButton closeButton = new TpCloseButton();
		
		public Magnify2()
		{
			DoubleBuffered = true;
			BackColor = Color.FromArgb(255, 48, 48, 48);

			bmpSrc = new Bitmap(80, 80, PixelFormat.Format32bppArgb);
			gfxSrc = Graphics.FromImage(bmpSrc);
			gfxSrc.InterpolationMode = InterpolationMode.NearestNeighbor;
			Size = new Size(344, 400);
			Location = Point.Empty;			

			Region = new Region(RoundedRectangle.Create(0, 0, Width, Height, 7));			

			cobine.ButtonType = ButtonType.Default;
			cobine.Size = new Size(100, 26);
			cobine.Location = new Point(Width - 14 - cobine.Width, Height - 36);
			cobine.Text = @"Capture";			
			cobine.MouseDown += combine_Click;
			var cobineToolTip = new ToolTip {ShowAlways = true};
			cobineToolTip.SetToolTip(cobine, "Capture picture and put it to library.");

			Controls.Add(cobine);


			snap.ButtonType = ButtonType.Default;
			snap.Size = new Size(120, 26);
			snap.Location = new Point(Width - 14 - snap.Width - 10 - cobine.Width, Height - 36);
			snap.Text = @"Capture & Edit";
			//snap.Click += new EventHandler(snap_Click);
			snap.MouseDown += snap_Click;

			var snapToolTip = new ToolTip {ShowAlways = true};
			snapToolTip.SetToolTip(snap, "Capture picture and edit it.");
			Controls.Add(snap);


			closeButton.Location = new Point(Width - closeButton.Width - 10, 7);
			closeButton.Click += closeButton_Click;
			Controls.Add(closeButton);

			MouseDown += Form1_MouseDown;
			MouseUp += Form1_MouseUp;
			MouseMove += Form1_MouseMove;
		}

		private static void closeButton_Click(object sender, EventArgs e)
		{
			FormsManager.Hide(typeof (TPEMain));
		}

		private const int BorderSize = 10;
		private const int HeaderHeight = 32;


		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			using (var pen = new Pen(Color.FromArgb(255, 0, 0, 0), 1))
			{
				e.Graphics.DrawPath(pen, RoundedRectangle.Create(0, 0, Width, Height, 7));
			}


			e.Graphics.DrawImage(Resources.TPIcon, new Point(10, 10));
			var sf = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center};
			e.Graphics.DrawString(Text, UI.DefaultFont, Brushes.White, new Rectangle(31, 10, Width - 25, 18), sf);


			var destRect = new Rectangle(BorderSize, HeaderHeight, 320, 320);
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			e.Graphics.DrawImage(bmpSrc, destRect, 0, 0, 80, 80, GraphicsUnit.Pixel);

			e.Graphics.DrawLine(redPen, destRect.Left + 160, destRect.Top, destRect.Left + 160, destRect.Top + 320);
			e.Graphics.DrawLine(redPen, destRect.Left + 164, destRect.Top, destRect.Left + 164, destRect.Top + 320);
			e.Graphics.DrawLine(redPen, destRect.Left, destRect.Top + 160, destRect.Left + 320, destRect.Top + 160);
			e.Graphics.DrawLine(redPen, destRect.Left, destRect.Top + 164, destRect.Left + 320, destRect.Top + 164);

			e.Graphics.DrawImage(Resources.PictHorizontal, 10, 372);
			e.Graphics.DrawString(hInfo.ToString(), UI.DefaultFont, new SolidBrush(Color.Black), 26, 370);
			e.Graphics.DrawImage(Resources.PictVertical, 60, 370);
			e.Graphics.DrawString(vInfo.ToString(), UI.DefaultFont, new SolidBrush(Color.Black), 70, 370);
		}

		private static void combine_Click(object sender, EventArgs e)
		{
			FormsManager.GetInstance<TPEMain>().CaptureToLib();
		}

		private void snap_Click(object sender, EventArgs e)
		{
			FormsManager.GetInstance<TPEMain>().SwitchToEditor();
		}

		public void UpdatePicture(Bitmap bmp, int x, int y, int w, int h)
		{
			gfxSrc.Clear(Color.Black);			
			gfxSrc.CopyFromScreen(x - 40, y - 40, 0, 0, bmpSrc.Size, CopyPixelOperation.SourceCopy);

			Text = string.Format("Capture Utility - Cursor({0}:{1})", x, y);

			vInfo = h;
			hInfo = w;
		}

		private bool isMoveWindow;
		private Point mouseOffset;

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.Y < HeaderHeight)
			{
				int xOffset = -e.X;
				int yOffset = -e.Y;
				if (Parent != null)
				{
					xOffset -= Parent.Bounds.Location.X;
					yOffset -= Parent.Bounds.Location.Y;
				}
				mouseOffset = new Point(xOffset, yOffset);
				isMoveWindow = true;
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			Cursor = e.Y < HeaderHeight ? Cursors.Hand : Cursors.Arrow;

			if (isMoveWindow)
			{
				var mousePos = MousePosition;
				mousePos.Offset(mouseOffset.X, mouseOffset.Y);
				Location = mousePos;
				Update();
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				isMoveWindow = false;
				Invalidate();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				FormsManager.Hide(typeof (TPEMain));
			}

			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
			{
				FormsManager.GetInstance<TPEMain>().SwitchToEditor();
			}
		}
	}
}