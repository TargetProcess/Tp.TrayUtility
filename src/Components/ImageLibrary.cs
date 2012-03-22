// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class ImageItem : Control
	{
		private bool FileLoaded;
		private readonly BackgroundWorker backgroundWorker;
		public FileSystemInfo fileInfo;
		private Image thumbNail;

		private bool isOver;

		public event EventHandler<EventArgs> ItemClicked = delegate { };
		public event EventHandler<EventArgs> DeleteClicked = delegate { };

		public ImageItem(FileSystemInfo _fileInfo)
		{
			DoubleBuffered = true;
			Size = new Size(160, 180);

			fileInfo = _fileInfo;

			backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += backgroundWorker_DoWork;
			backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
		}


		protected override void OnMouseClick(MouseEventArgs e)
		{
			if ((new Rectangle(140, 150, 20, 20)).Contains(PointToClient(Cursor.Position)))
			{
				DeleteClicked(this, e);
			}
			else
			{
				ItemClicked(this, e);
			}
		}


		protected override void OnResize(EventArgs e)
		{
			Region = new Region(RoundedRectangle.Create(0, 0, Width, Height, 3));
			base.OnResize(e);
		}


		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			FileLoaded = true;
			Invalidate();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			using (var bmp = new Bitmap(fileInfo.FullName))
			{
				float aspectRatio = Math.Min(150.0f/bmp.Width, 150.0f/bmp.Height);
				if (aspectRatio > 1.0f)
					aspectRatio = 1.0f;

				thumbNail = new Bitmap((int) (bmp.Width*aspectRatio), (int) (bmp.Height*aspectRatio));
				Graphics gfxTmp = Graphics.FromImage(thumbNail);
				gfxTmp.SmoothingMode = SmoothingMode.AntiAlias;
				gfxTmp.DrawImage(bmp, 0, 0, thumbNail.Width, thumbNail.Height);
			}
		}


		protected override void OnMouseEnter(EventArgs e)
		{
			isOver = true;
			//base.OnMouseEnter(e);
			//Controls.Add(deleteFile);
			//deleteFile.Show();
			Update();
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			isOver = false;
			//base.OnLeave(e);
			//deleteFile.Hide();
			//Update();
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Invalidate();
			base.OnMouseMove(e);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			if (isOver)
			{
				using (Brush br = new SolidBrush(Color.FromArgb(255, 60, 60, 60)))
				{
					GraphicsPath pth = RoundedRectangle.Create(0, 0, Width, Height, 3);
					e.Graphics.FillPath(br, pth);
				}
				using (var pen = new Pen(Color.FromArgb(255, 100, 100, 100), 2))
				{
					GraphicsPath pth2 = RoundedRectangle.Create(1, 1, Width - 2, Height - 2, 3);
					e.Graphics.DrawPath(pen, pth2);
				}
			}

			if (!FileLoaded)
			{
				e.Graphics.DrawLine(Pens.Red, 0, 0, Width, Height);
				e.Graphics.DrawLine(Pens.Red, Width, 0, 0, Height);
				if (!FileLoaded && !backgroundWorker.IsBusy)
					backgroundWorker.RunWorkerAsync();
			}
			else
			{
				e.Graphics.DrawImageUnscaled(thumbNail, (Width - thumbNail.Width)/2, 150 - thumbNail.Height);
			}

			var sf = new StringFormat {Alignment = StringAlignment.Center};

			e.Graphics.DrawString(fileInfo.CreationTime.ToLongDateString(), UI.SmallFont, Brushes.White,
			                      new Rectangle(0, 150, Width, 160), sf);
			e.Graphics.DrawString(fileInfo.CreationTime.ToLongTimeString(), UI.SmallFont, Brushes.White,
			                      new Rectangle(0, 160, Width, 170), sf);

			e.Graphics.DrawImageUnscaled(Resources.close_default, 140, 150);

			base.OnPaint(e);
		}
	}


	public class ImageLibrary : Control
	{
		public static string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
		                                   "/TargetProcess/SnapShotLib";

		private int firstFileToShow;
		private readonly FileSystemInfo[] files;
		private readonly List<ImageItem> imageList = new List<ImageItem>();

		private readonly TpScrollBar scrollbar = new TpScrollBar(TpScrollBar.Style.Vertical);

		public event EventHandler<EventArgs> PictureSelected = delegate { };

		private static int CompareFileInfo(FileSystemInfo f1, FileSystemInfo f2)
		{
			return f2.CreationTime.CompareTo(f1.CreationTime);
		}


		public ImageLibrary()
		{
			Directory.CreateDirectory(DefaultPath);
			var di = new DirectoryInfo(DefaultPath);
			files = di.GetFileSystemInfos();

			Array.Sort(files, CompareFileInfo);

			foreach (FileSystemInfo file in files)
			{
				var ii = new ImageItem(file);
				ii.DeleteClicked += ii_DeleteClicked;
				ii.ItemClicked += ii_ItemClicked;
				imageList.Add(ii);
			}

			scrollbar.ValueChanged += scrollbar_ValueChanged;

			Focus();

			var watcher = new FileSystemWatcher();
			watcher.Filter = "*.bmp";
			watcher.Created += watcher_FileCreated;
			watcher.Path = DefaultPath;
			watcher.EnableRaisingEvents = true;
		}

		private void watcher_FileCreated(object sender, FileSystemEventArgs e)
		{
			var di = new DirectoryInfo(DefaultPath);
			var fl = di.GetFileSystemInfos(e.Name);
			if (fl.Length <= 0) return;

			var ii = new ImageItem(fl[0]);
			ii.DeleteClicked += ii_DeleteClicked;
			ii.ItemClicked += ii_ItemClicked;
			imageList.Add(ii);
		}


		private void ii_ItemClicked(object sender, EventArgs e)
		{
			PictureSelected(((ImageItem) sender).fileInfo.FullName, e);
		}

		private void ii_DeleteClicked(object sender, EventArgs e)
		{
			try
			{
				File.Delete(((ImageItem) sender).fileInfo.FullName);
				imageList.Remove((ImageItem) sender);
				ShowFiles();
				Update();
				Invalidate();
			}
			catch
			{
			}
		}

		private void scrollbar_ValueChanged(object sender, EventArgs e)
		{
			int defaultWidth = 50;
			if (imageList.Count > 0)
				defaultWidth = imageList[0].Width;
			firstFileToShow = scrollbar.Position*(defaultWidth/(+20));
			ShowFiles(false);
			Update();
			Invalidate();
		}


		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ShowFiles();
			scrollbar.Location = new Point(Width - scrollbar.Width, 3);
			scrollbar.Size = new Size(20, Height - 6);
		}


		public void ShowFiles()
		{
			ShowFiles(true);
		}


		public void ShowFiles(bool updateScrollbar)
		{
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				if (Controls[i] is ImageItem)
					Controls.RemoveAt(i);
			}
			//Controls.Clear();


			int ox = 10;
			int oy = 10;
			int index = 0;

			foreach (ImageItem ii in imageList)
			{
				if (index >= firstFileToShow)
				{
					if (ox + ii.Width > Width)
					{
						oy += ii.Width + 20;
						if (oy /*+ ii.Height*/> Height)
						{
							break;
						}
						ox = 10;
					}
					ii.Location = new Point(ox, oy);
					ox += ii.Width + 20;
					Controls.Add(ii);
				}
				index++;
			}


			if (updateScrollbar && imageList.Count > 0)
			{
//				if (scrollbar.Parent == null)
				if (!Controls.Contains(scrollbar))
					Controls.Add(scrollbar);
				int itemCount = Width/(imageList[0].Width + 20);
				int rowCount = imageList.Count/itemCount;
				scrollbar.PositionCount = rowCount;
				scrollbar.Position = firstFileToShow/itemCount;
			}
		}


		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				scrollbar.Position--;
			}
			else if (e.Delta < 0)
			{
				scrollbar.Position++;
			}
			scrollbar.Update();
			scrollbar.Invalidate();
			firstFileToShow = scrollbar.Position*(Width/(imageList[0].Width + 20));
			ShowFiles(false);
			Update();
			Invalidate();
		}
	}
}