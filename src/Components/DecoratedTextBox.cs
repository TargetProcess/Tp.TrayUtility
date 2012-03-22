//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class DecoratedTextBox : Primitive
	{
		public static Color borderColorGlobal = Color.Red;
		public static int borderSizeGlobal = 1;
		public static Color backColorGlobal = Color.White;

		public static string fontNameGlobal = "Arial";
		public static int fontSizeGlobal = 12;
		public static Color fontColorGlobal = Color.Black;
		private readonly ShapeBoxControlPoint arrowPoint = new ShapeBoxControlPoint(0, 0, 100);
		private bool EditMode;
		private Pen arrowPen;
		public Color backColor;
		private Brush barBrush;
		private Pen barPen;

		public Color borderColor;
		public int borderSize;
		private RectangleF currentRectangle = new RectangleF(0, 0, 0, 0);
		public Color fontColor;

		public string fontName;
		public int fontSize;
		private Bitmap textAreaImage;
		private RectangleF textAreaRect = new RectangleF(0, 0, 0, 0);
		public TpRichText tpText = new TpRichText();

		private bool needToUpdateTb = true;


		public DecoratedTextBox()
		{
			UpdatePropertyFromStaticData();
			tpText.FinishEditingHandler += EditFinishing;
			ImageViewPort._viewPortInstance.Controls.Add(tpText);
			tpText.Visible = false;

			SetColor(Color.Red, Color.White);

			var fontFamily = new FontFamily("Arial");
			TextFont = new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);
			tpText.Font = TextFont;

			textAreaImage = new Bitmap(1, 1);
			textAreaImage.SetPixel(0, 0, backColor);
		}

		public Font TextFont { get; set; }

		public override RectangleF BoundRect
		{
			get
			{
				RectangleF rect = currentRectangle;
				rect.Inflate(2, 2);
				return rect;
			}

			set
			{
				RectangleF rect = Helpers.NormalizedRect(value.Location, new PointF(value.Right, value.Bottom));
				rect.Inflate(-2, -2);
				currentRectangle = rect;
			}
		}

		public override void UpdatePropertyFromStaticData()
		{
			borderColor = borderColorGlobal;
			borderSize = borderSizeGlobal;
			backColor = backColorGlobal;
		}

		public override bool isPointIn(PointF point)
		{
			return currentRectangle.Contains(point);
		}

		public GraphicsPath Create(int x, int y, int width,
								   int height, int radius, bool corners, Point arrow)
		{
			int xw = x + width;
			int yh = y + height;
			int xwr = xw - radius;
			int yhr = yh - radius;
			int xr = x + radius;
			int yr = y + radius;
			int r2 = radius * 2;
			int xwr2 = xw - r2 - 1;
			int yhr2 = yh - r2 - 1;
			int xwHalf = x + width / 2;
			int yhHalf = y + height / 2;

			int arrowW = Math.Min(width / 4, width / 2 - radius - 5);
			int arrowH = Math.Min(height / 4, height / 2 - radius - 5);

			var center = new Point(x + width / 2, y + height / 2);

			int segment;

			decimal dx = arrow.X - center.X;
			decimal dy = arrow.Y - center.Y;
			decimal wx = x - center.X;
			decimal wy = y - center.Y;

			if (dy == 0) dy = 1;
			if (wy == 0) wy = 1;


			if (dx <= 0)
			{
				if (dy < 0 && Math.Abs(dx / dy) < Math.Abs(wx / wy))
				{
					segment = 7;
				}
				else if (dy < 0)
				{
					segment = 6;
				}
				else if (Math.Abs(dy) < Math.Abs(dx))
				{
					segment = 5;
				}
				else
				{
					segment = 4;
				}
			}
			else
			{
				if (dy < 0 && Math.Abs(dx / dy) < Math.Abs(wx / wy))
				{
					segment = 0;
				}
				else if (dy < 0)
				{
					segment = 1;
				}
				else if (Math.Abs(dy) < Math.Abs(dx))
				{
					segment = 2;
				}
				else
				{
					segment = 3;
				}
			}


			var graphicsPath = new GraphicsPath();
			graphicsPath.StartFigure();

			//Top Left Corner
			if (corners)
			{
				graphicsPath.AddArc(x, y, r2, r2, 180, 90);
			}
			else
			{
				graphicsPath.AddLine(x, yr, x, y);
				graphicsPath.AddLine(x, y, xr, y);
			}

			//Top Edge
			if (segment == 7)
			{
				graphicsPath.AddLine(xr, y, xwHalf - arrowW, y);
				graphicsPath.AddLine(xwHalf - arrowW, y, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xwHalf, y);
			}
			else
			{
				graphicsPath.AddLine(xr, y, xwHalf, y);
			}

			if (segment == 0)
			{
				graphicsPath.AddLine(xwHalf, y, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xwHalf + arrowW, y);
				graphicsPath.AddLine(xwHalf + arrowW, y, xwr, y);
			}
			else
			{
				graphicsPath.AddLine(xwHalf, y, xwr, y);
			}


			//Top Right Corner
			if (corners)
			{
				graphicsPath.AddArc(xwr2, y, r2, r2, 270, 90);
			}
			else
			{
				graphicsPath.AddLine(xwr, y, xw, y);
				graphicsPath.AddLine(xw, y, xw, yr);
			}

			//Right Edge
			if (segment == 1)
			{
				graphicsPath.AddLine(xw, yr, xw, yhHalf - arrowH);
				graphicsPath.AddLine(xw, yhHalf - arrowH, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xw, yhHalf);
			}
			else
			{
				graphicsPath.AddLine(xw, yr, xw, yhHalf);
			}

			if (segment == 2)
			{
				graphicsPath.AddLine(xw, yhHalf, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xw, yhHalf + arrowH);
				graphicsPath.AddLine(xw, yhHalf + arrowH, xw, yhr);
			}
			else
			{
				graphicsPath.AddLine(xw, yhHalf, xw, yhr);
			}
			//p.AddLine(xw, yr, xw, yhr);

			//Bottom Right Corner
			if (corners)
			{
				graphicsPath.AddArc(xwr2, yhr2, r2, r2, 0, 90);
			}
			else
			{
				graphicsPath.AddLine(xw, yhr, xw, yh);
				graphicsPath.AddLine(xw, yh, xwr, yh);
			}

			//Bottom Edge
			if (segment == 3)
			{
				graphicsPath.AddLine(xwr, yh, xwHalf + arrowW, yh);
				graphicsPath.AddLine(xwHalf + arrowW, yh, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xwHalf, yh);
			}
			else
			{
				graphicsPath.AddLine(xwr, yh, xwHalf, yh);
			}

			if (segment == 4)
			{
				graphicsPath.AddLine(xwHalf, yh, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, xwHalf - arrowW, yh);
				graphicsPath.AddLine(xwHalf - arrowW, yh, xr, yh);
			}
			else
			{
				graphicsPath.AddLine(xwHalf, yh, xr, yh);
			}


			//Bottom Left Corner
			if (corners)
			{
				graphicsPath.AddArc(x, yhr2, r2, r2, 90, 90);
			}
			else
			{
				graphicsPath.AddLine(xr, yh, x, yh);
				graphicsPath.AddLine(x, yh, x, yhr);
			}

			//Left Edge
			if (segment == 5)
			{
				graphicsPath.AddLine(x, yhr, x, yhHalf + arrowH);
				graphicsPath.AddLine(x, yhHalf + arrowH, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, x, yhHalf);
			}
			else
			{
				graphicsPath.AddLine(x, yhr, x, yhHalf);
			}

			if (segment == 6)
			{
				graphicsPath.AddLine(x, yhHalf, arrow.X, arrow.Y);
				graphicsPath.AddLine(arrow.X, arrow.Y, x, yhHalf - arrowH);
				graphicsPath.AddLine(x, yhHalf - arrowH, x, yr);
			}
			else
			{
				graphicsPath.AddLine(x, yhHalf, x, yr);
			}

			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition, Point tbOffset)
		{
			RectangleF rect = Helpers.NormalizedRect(currentRectangle.Location,
													new PointF(currentRectangle.Right, currentRectangle.Bottom));

			int x0 = (int)(rect.Left * zoom + offset.X);
			int y0 = (int)(rect.Top * zoom + offset.Y);
			var x1 = (int)(rect.Width * zoom);
			var y1 = (int)(rect.Height * zoom);
			textAreaRect = new Rectangle(x0, y0, x1, y1);

			if (x1 == 0 || y1 == 0)
				return;

			var arrow = new Point((int)(arrowPoint.point.X * zoom + offset.X), (int)(arrowPoint.point.Y * zoom + offset.Y));

			if (ImageViewPort.CastShadow)
			{
				GraphicsPath pth2 = Create(x0 + (int)(3 * zoom), y0 + (int)(3 * zoom), (int)textAreaRect.Width, (int)textAreaRect.Height, 7,
										   true, arrow);
				using (Brush backBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)))
				{
					g.FillPath(backBrush, pth2);
				}
			}

			var pth = Create(x0, y0, (int)textAreaRect.Width, (int)textAreaRect.Height, 7, true, arrow);
			using (Brush backBrush = new SolidBrush(backColor))
			{
				using (var pen = new Pen(borderColor))
				{
					pen.Width = borderSize;
					g.FillPath(backBrush, pth);
					g.DrawPath(pen, pth);
				}
			}

			var tmpRect = new Rectangle((int)textAreaRect.Location.X + 4, (int)textAreaRect.Location.Y + 4,
											(int)textAreaRect.Size.Width - 8, (int)textAreaRect.Size.Height - 8);

			if (EditMode)
			{
				if(needToUpdateTb)
				{
					tpText.Size = new Size(tmpRect.Width, tmpRect.Height);

					tpText.Location = new Point(tmpRect.Location.X + tbOffset.X, tmpRect.Location.Y + tbOffset.Y);
					needToUpdateTb = false;
				}
			}
			else if (textAreaImage != null)
			{
				g.DrawImageUnscaledAndClipped(textAreaImage, tmpRect);
			}
		}

		public override void OnFormResize()
		{
			needToUpdateTb = true;
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition)
		{
		}

		public override void SetColor(Color fore, Color back)
		{
			barPen = new Pen(fore, 6.0f);
			arrowPen = new Pen(fore, 6.0F) { StartCap = LineCap.Flat, EndCap = LineCap.ArrowAnchor };

			barBrush = new SolidBrush(back);
		}

		public override void CreationMouseDownHandler(PointF point)
		{
			arrowPoint.Position = new PointF(point.X + 20, point.Y - 50);
			currentRectangle.Location = point;
		}

		public override void CreationMouseUpHandler(PointF point)
		{
			RectangleF rect = Helpers.NormalizedRect(currentRectangle.Location, point);
			currentRectangle = rect;
			tpText.BackColor = backColor;
		}

		private void EditFinishing(object sender, EventArgs e)
		{
			SwitchToView();
		}

		public override void ShapeToolLost()
		{
			SwitchToView();
		}

		private void SwitchToEdit()
		{
			needToUpdateTb = true;
			EditMode = true;
			tpText.Visible = true;
			tpText.ReadOnly = false;
			tpText.BackColor = backColor;
			tpText.Focus();

			User32.ShowCaret(tpText.Handle);
			
			ImageViewPort._viewPortInstance.UpdateCurrentTool();
		}

		private void SwitchToView()
		{
			if (EditMode && tpText.Width > 0 && tpText.Height > 0)
			{
				EditMode = false;
				tpText.ReadOnly = true;
				tpText.DeselectAll();
				User32.HideCaret(tpText.Handle);
				Control ctrl = tpText;
				textAreaImage = new Bitmap(ctrl.Width, ctrl.Height);
				ImageViewPort._viewPortInstance.RemoveAllProperties();
				GDI32.DrawControlToBitMap(ctrl, textAreaImage, new Rectangle(0, 0, ctrl.Width, ctrl.Height));
				ImageViewPort._viewPortInstance.UpdateCurrentTool();
				
				if (tpText.Parent != null)
					tpText.Parent.Select();

				tpText.Size = new Size(0, 0);
			}
		}


		public override void CreationMouseMoveHandler(PointF point)
		{
			currentRectangle.Size = new SizeF(point.X - currentRectangle.X, point.Y - currentRectangle.Y);
		}

		public override void MouseDoubleClickHandler(PointF point)
		{
			if (!EditMode)
				SwitchToEdit();
		}

		public override Cursor GetCursor(PointF point)
		{
			return Cursors.Hand;
		}


		public override bool isResizeble()
		{
			return true;
		}

		public override void MoveBy(float dx, float dy)
		{
			if (EditMode) return;
			currentRectangle.Offset(-dx, -dy);
			arrowPoint.point = new PointF(arrowPoint.point.X - dx, arrowPoint.point.Y - dy);//Analog Offset(-dx, -dy) for PointF

		}

		public override List<ShapeBoxControlPoint> SpecialPoints()
		{
			var points = new List<ShapeBoxControlPoint> { arrowPoint };
			return points;
		}

		public override void UpdateSpecialPoint(int id, PointF position)
		{
			arrowPoint.Position = position;
		}

		public override bool isValid()
		{
			return currentRectangle.Width > 5 && currentRectangle.Height > 5;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			if (EditMode)
				return new DecoratedTextBoxEditModePropertyControl();
			return new DecoratedTextBoxPropertyControl();
		}

		public override void OnResize(bool lastMove)
		{
			needToUpdateTb = true;
			tpText.Show();
			
			if (!EditMode)
			{
				SwitchToEdit();
			}

			if(tpText.Parent != null)
			{
				tpText.Parent.Select();
			}
		}

		public override bool CanUndo()
		{
			return !EditMode;
		}
	}
}