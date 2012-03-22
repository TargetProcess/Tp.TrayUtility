// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class ShapeBoxControl
	{
		private Primitive primitive;

		public Primitive Primitive
		{
			set
			{
				primitive = value;
				if (primitive != null)
				{
					shape = primitive.BoundRect;
					GeneratePoints();
				}
			}
			get { return primitive; }
		}

		public bool Dragging { get; set; }

		private readonly List<ShapeBoxControlPoint> points = new List<ShapeBoxControlPoint>();

		private RectangleF shape;

		public ShapeBoxControl(Primitive prim)
		{
			Primitive = prim;
			GeneratePoints();
		}

		public void GeneratePoints()
		{
			if (primitive == null)
				return;

			points.Clear();
			if (primitive.isResizeble())
			{
				points.Add(new ShapeBoxControlPoint(shape.Left, shape.Top, 0));
				points.Add(new ShapeBoxControlPoint(shape.Left + shape.Width/2, shape.Top, 1));
				points.Add(new ShapeBoxControlPoint(shape.Right, shape.Top, 2));
				points.Add(new ShapeBoxControlPoint(shape.Right, shape.Top + shape.Height/2, 3));
				points.Add(new ShapeBoxControlPoint(shape.Right, shape.Bottom, 4));
				points.Add(new ShapeBoxControlPoint(shape.Left + shape.Width/2, shape.Bottom, 5));
				points.Add(new ShapeBoxControlPoint(shape.Left, shape.Bottom, 6));
				points.Add(new ShapeBoxControlPoint(shape.Left, shape.Top + shape.Height/2, 7));
			}

			points.AddRange(primitive.SpecialPoints());
		}

		public void Draw(Graphics g, PointF offset, float zoom)
		{
			if (primitive == null)
				return;
						
			
			if (primitive.isResizeble())
			{
				const int shapePointCount = 8;
				for (int i = 0; i < shapePointCount; i++)
				{
					PointF nextPoint;
					if (i == shapePointCount - 1)
						nextPoint = points[0].Position;
					else
						nextPoint = points[i + 1].Position;

					int x0 = (int) (points[i].Position.X*zoom + offset.X);
					int y0 = (int) (points[i].Position.Y*zoom + offset.Y);
					int x1 = (int) (nextPoint.X*zoom + offset.X);
					int y1 = (int) (nextPoint.Y*zoom + offset.Y);
					g.DrawLine(Pens.LightGray, x0, y0, x1, y1);
				}

				using (Brush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
				{
					foreach (ShapeBoxControlPoint p in points)
					{
						g.FillEllipse(brush,
						              new Rectangle((int) (p.Position.X*zoom + offset.X - 5), (int) (p.Position.Y*zoom + offset.Y) - 5, 10,
						                            10));
						g.DrawEllipse(Pens.LightGray,
						              new Rectangle((int) (p.Position.X*zoom + offset.X - 5), (int) (p.Position.Y*zoom + offset.Y) - 5, 10,
						                            10));
					}
				}
			}
			else
			{
				int x0 = (int) (shape.Left*zoom + offset.X);
				int y0 = (int) (shape.Top*zoom + offset.Y);
				int x1 = (int) (shape.Width*zoom);
				int y1 = (int) (shape.Height*zoom);

				g.DrawRectangle(Pens.LightGray, new Rectangle(x0, y0, x1, y1));
			}
		}

		public void DrawPointsOnly(Graphics g)
		{
			using (Brush brush = new SolidBrush(Color.FromArgb(64, 0, 0, 0)))
			{
				foreach (var p in points)
				{
					g.FillEllipse(brush, new RectangleF(p.Position.X - 5, p.Position.Y - 5, 10, 10));
					g.DrawEllipse(Pens.LightGray, new RectangleF(p.Position.X - 5, p.Position.Y - 5, 10, 10));
				}
			}
		}

		public int GetPoint(PointF coords)
		{
			foreach (var p in points.Where(p => Distance(p.Position, coords) < 10))
			{
				return p.ID;
			}
			return -1;
		}

		private static double Distance(PointF p, PointF c)
		{
			return Math.Sqrt((p.X - c.X)*(p.X - c.X) + (p.Y - c.Y)*(p.Y - c.Y));
		}

		public void MovePoint(int id, PointF point)
		{
			if (id == 0)
			{
				float dx = shape.X - point.X;
				float dy = shape.Y - point.Y;
				shape.Location = point;
				shape.Size = new SizeF(shape.Width + dx, shape.Height + dy);
			}
			else if (id == 1)
			{
				float dy = shape.Y - point.Y;
				shape.Y = point.Y;
				shape.Size = new SizeF(shape.Width, shape.Height + dy);
			}
			else if (id == 2)
			{
				float dx = point.X - shape.X;
				float dy = shape.Y - point.Y;
				shape.Y = point.Y;
				shape.Size = new SizeF(dx, shape.Height + dy);
			}
			else if (id == 3)
			{
				shape.Size = new SizeF(point.X - shape.X, shape.Height);
			}
			else if (id == 4)
			{
				float dx = point.X - shape.X;
				float dy = point.Y - shape.Y;
				shape.Size = new SizeF(dx, dy);
			}
			else if (id == 5)
			{
				shape.Size = new SizeF(shape.Width, point.Y - shape.Y);
			}
			else if (id == 6)
			{
				float dx = shape.X - point.X;
				shape.X = point.X;
				shape.Size = new SizeF(shape.Width + dx, point.Y - shape.Y);
			}
			else if (id == 7)
			{
				float dx = shape.X - point.X;
				shape.X = point.X;
				shape.Size = new SizeF(shape.Width + dx, shape.Height);
			}
			else
			{
				primitive.UpdateSpecialPoint(id, point);
			}

			GeneratePoints();

			primitive.BoundRect = shape;
		}

		internal bool PointIn(PointF point)
		{
			return shape.Contains(point);
		}

		internal void MoveBy(float dx, float dy)
		{

			if (primitive != null)
			{
				var ddx = primitive.BoundRect.X - dx >= 20 ? dx : primitive.BoundRect.X - 20;
				var ddy = primitive.BoundRect.Y - dy >= 20 ? dy : primitive.BoundRect.Y - 20;
				shape.X -= ddx;
				shape.Y -= ddy;
				primitive.MoveBy(ddx, ddy);
			}
			else
			{
				shape.X -= dx;
				shape.Y -= dy;
			}
				
			GeneratePoints();
		}

		public virtual Cursor GetCursor(PointF point, int dragPointID)
		{
			if (primitive == null)
				return null;

			int overPointID = GetPoint(point);
			if (overPointID == 0 || overPointID == 4 || dragPointID == 0 || dragPointID == 4)
			{
				return Cursors.SizeNWSE;
			}
			if (overPointID == 2 || overPointID == 6 || dragPointID == 2 || dragPointID == 6)
			{
				return Cursors.SizeNESW;
			}
			if (overPointID == 1 || overPointID == 5 || dragPointID == 1 || dragPointID == 5)
			{
				return Cursors.SizeNS;
			}
			if (overPointID == 3 || overPointID == 7 || dragPointID == 3 || dragPointID == 7)
			{
				return Cursors.SizeWE;
			}
			if (overPointID > 0)
			{
				return Cursors.Hand;
			}
			return null;
		}
	}
}