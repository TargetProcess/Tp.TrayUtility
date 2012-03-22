//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class Picture : Primitive
	{
		public static Color penColorGlobal = Color.Red;
		public static int penSizeGlobal = 1;
		public static DashStyle penStyleGlobal = DashStyle.Solid;

		public Color penColor;
		public int penSize;
		public DashStyle penStyle;


		private readonly Bitmap originalPicture;
		private RectangleF currentRectangle;

		public Picture(Bitmap originPict)
		{
			originalPicture = originPict;
			currentRectangle = new Rectangle(0, 0, originPict.Width, originPict.Height);
			UpdatePropertyFromStaticData();
		}

		public override bool isPointIn(PointF point)
		{
			return BoundRect.Contains(point);
		}

		public Rectangle GetLastModifiedRect()
		{
			return new Rectangle();
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition)
		{
			var rect = Helpers.NormalizedRect(currentRectangle.Location,
			                                        new PointF(currentRectangle.Right, currentRectangle.Bottom));


			var x0 = (int) (rect.Left*zoom) + offset.X;
			var y0 = (int) (rect.Top*zoom) + offset.Y;
			var x1 = (int) (rect.Width*zoom);
			var y1 = (int) (rect.Height*zoom);


			new Size((int) (currentRectangle.Width*zoom), (int) (currentRectangle.Height*zoom));
			if (ImageViewPort.CastShadow)
			{
				using (Brush brush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)))
				{
					g.FillRectangle(brush, new RectangleF(x0 + (int) (3*zoom), y0 + (int) (3*zoom), x1, y1));
				}
			}
			g.DrawImage(originalPicture, new RectangleF(x0, y0, x1, y1));
		}

		public override void SetColor(Color fore, Color back)
		{
		}

		public override void CreationMouseDownHandler(PointF point)
		{
		}

		public override void CreationMouseUpHandler(PointF point)
		{
		}


		public override void MouseDoubleClickHandler(PointF point)
		{
		}


		public override void CreationMouseMoveHandler(PointF point)
		{
		}


		public override void ShapeToolLost()
		{
		}

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


		public override bool isResizeble()
		{
			return true;
		}

		public override void MoveBy(float dx, float dy)
		{
			currentRectangle.Offset(-dx, -dy);
		}

		public override List<ShapeBoxControlPoint> SpecialPoints()
		{
			return new List<ShapeBoxControlPoint>();
		}

		public override void UpdateSpecialPoint(int id, PointF position)
		{
		}

		public override bool isValid()
		{
			return true;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			return null;
		}

		public override void UpdatePropertyFromStaticData()
		{
			penColor = penColorGlobal;
			penSize = penSizeGlobal;
			penStyle = penStyleGlobal;
		}

		public override Cursor GetCursor(PointF point)
		{
			return Cursors.Hand;
		}

		public override void OnResize(bool lastMove)
		{
		}

		public override bool CanUndo()
		{
			return true;
		}
	}
}