// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility.SubmitBug
{
	public class AttachmentControlItem : Control
	{
		public string fileName;
		private Image thumbNail;
		private readonly TpTextBox screenName;
		private readonly TpCloseButton closeButton = new TpCloseButton();

		public event EventHandler<EventArgs> DeleteClicked = delegate { };

		public AttachmentControlItem(string _fileName)
		{
			fileName = _fileName;
			Size = new Size(200, 180);
			Init();
			screenName = new TpTextBox {Location = new Point(10, 150), Size = new Size(180, 20)};

			closeButton.Location = new Point(Width - closeButton.Width - 10, 7);
			closeButton.Click += closeButton_Click;
			Controls.Add(closeButton);
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			DeleteClicked(this, e);
		}

		private void Init()
		{
			try
			{
				using (var bmp = new Bitmap(fileName))
				{
					var aspectRatio = Math.Min(150.0f/bmp.Width, 150.0f/bmp.Height);
					if (aspectRatio > 1.0f)
						aspectRatio = 1.0f;

					thumbNail = new Bitmap((int) (bmp.Width*aspectRatio), (int) (bmp.Height*aspectRatio));
					var gfxTmp = Graphics.FromImage(thumbNail);
					gfxTmp.SmoothingMode = SmoothingMode.AntiAlias;
					gfxTmp.DrawImage(bmp, 0, 0, thumbNail.Width, thumbNail.Height);
				}
			}
			catch //it can be not a picture...
			{
				thumbNail = null;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (thumbNail != null)
			{
				e.Graphics.DrawImageUnscaled(thumbNail, (Width - thumbNail.Width)/2, 150 - thumbNail.Height);
			}
			else
			{
				var sf = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};

				e.Graphics.DrawString("No Picture!", UI.DefaultFont, Brushes.Gray, new Rectangle(0, 0, 200, 150), sf);
			}
		}
	}
}