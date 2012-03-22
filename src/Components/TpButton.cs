//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class TpButton : Button
	{
		public class Settings
		{
			public Color Normal1;
			public Color Normal2;
			public Color Over1;
			public Color Over2;
			public Color Pressed1;
			public Color Pressed2;
			public Color FontColor;
			public Color Border;
			public Color BorderOver;

			public static Settings Default()
			{
				var settings = new Settings
				               	{
				               		Normal1 = Color.FromArgb(255, 180, 230, 160),
				               		Normal2 = Color.FromArgb(255, 85, 140, 35),
				               		Over1 = Color.FromArgb(255, 190, 235, 175),
				               		Over2 = Color.FromArgb(255, 105, 160, 75),
				               		Pressed1 = Color.FromArgb(255, 50, 100, 48),
				               		Pressed2 = Color.FromArgb(255, 100, 156, 70),
				               		Border = Color.FromArgb(255, 76, 100, 70),
				               		BorderOver = Color.FromArgb(255, 90, 120, 80),
				               		FontColor = Color.White
				               	};
				return settings;
			}

			public static Settings Grey()
			{
				var settings = new Settings
				               	{
				               		Normal1 = Color.FromArgb(255, 177, 177, 177),
				               		Normal2 = Color.FromArgb(255, 177, 177, 177),
				               		Over1 = Color.FromArgb(255, 203, 203, 203),
				               		Over2 = Color.FromArgb(255, 203, 203, 203),
				               		Pressed1 = Color.FromArgb(255, 11, 11, 11),
				               		Pressed2 = Color.FromArgb(255, 64, 64, 64),
				               		Border = Color.FromArgb(255, 44, 44, 44),
				               		BorderOver = Color.FromArgb(255, 44, 44, 44),
				               		FontColor = Color.White
				               	};
				return settings;
			}

			public static Settings Red()
			{
				return Grey();
			}
		}


		private bool isOver;
		private bool isPressed;
		private double animation;
		private Settings settings = Settings.Red();
		private readonly Timer timer;

		public TpButton()
		{
			timer = new Timer();
			timer.Tick += TimerMethod;
			timer.Interval = 10;
		}

		public ButtonType ButtonType
		{
			set
			{
				switch (value)
				{
					case ButtonType.Default:
						settings = Settings.Default();
						break;
					case ButtonType.Red:
						settings = Settings.Red();
						break;
					case ButtonType.Grey:
						settings = Settings.Grey();
						break;
				}
			}

			get { return ButtonType.Default; }
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			Region = new Region(RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 2));
			base.OnSizeChanged(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Draw(e.Graphics);
		}

		protected Color CalculateColor(Color start, Color finish, double value)
		{
			var r = (int) (start.R + (finish.R - start.R)*value);
			var g = (int) (start.G + (finish.G - start.G)*value);
			var b = (int) (start.B + (finish.B - start.B)*value);
			return Color.FromArgb(r, g, b);
		}

		protected void Draw(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;


			if (isPressed)
			{
				using (
					var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), settings.Pressed1,
					                                    settings.Pressed2))
				{
					g.FillRectangle(brush, 0, 0, Width, Height);
				}
			}
			else if (isOver)
			{
				using (
					var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), settings.Over1,
					                                                    settings.Over2))
				{
					g.FillRectangle(brush, 0, 0, Width, Height);
				}
			}
			else
			{
				using (
					var brush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height), settings.Normal1,
					                                                    settings.Normal2))
				{
					g.FillRectangle(brush, 0, 0, Width, Height);
				}
			}

			using (var p = new Pen(Brushes.Black))
			{
				p.Color = isOver ? settings.BorderOver : settings.Border;
				g.DrawPath(p, RoundedRectangle.Create(0, 0, Width - 2, Height - 2, 2));
			}


			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			var sf = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
			using (var brush = new SolidBrush(settings.FontColor))
			{
				g.DrawString(Text, UI.DefaultFont, brush, new Rectangle(0, 0, Width, Height), sf);
			}
		}

		public void TimerMethod(Object state, EventArgs myEventArgs)
		{
			animation += 0.1;
			Invalidate();
			if (animation >= 1.0)
				timer.Stop();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			animation = 0;
			timer.Start();
			isOver = true;
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			timer.Stop();
			isOver = false;
			isPressed = false;
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			timer.Stop();
			isPressed = true;
			base.OnMouseDown(mevent);
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			isPressed = false;
			base.OnMouseUp(mevent);
		}
	}
}