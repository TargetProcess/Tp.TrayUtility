//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TpTrayUtility.Controls;

namespace TpTrayUtility.Components
{
	public class FreeLine : Primitive
	{
		public static Color penColorGlobal = Color.Red;
		public static int penSizeGlobal = 3;
		public static DashStyle penStyleGlobal = DashStyle.Solid;
		protected Collection<PointF> freeLine;
		protected Pen linePen;

		public Color penColor;
		public int penSize;
		public DashStyle penStyle;


		public FreeLine()
		{
			UpdatePropertyFromStaticData();
			linePen = new Pen(penColor, penSize);
			freeLine = new Collection<PointF>();
		}

		public override RectangleF BoundRect
		{
			get
			{
				if (freeLine.Count == 0)
					return new RectangleF();

				PointF topLeft = freeLine[0];
				PointF bottomRight = freeLine[0];
				foreach (PointF p in freeLine)
				{
					if (p.X < topLeft.X)
						topLeft.X = p.X;
					if (p.Y < topLeft.Y)
						topLeft.Y = p.Y;

					if (p.X > bottomRight.X)
						bottomRight.X = p.X;
					if (p.Y > bottomRight.Y)
						bottomRight.Y = p.Y;
				}
				var rx = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));
				rx.Inflate(2, 2);
				return rx;
			}

			set { }
		}

		public void AddPoint(PointF point)
		{
			freeLine.Add(point);
		}

		public override void SetColor(Color fore, Color back)
		{
		}


		public override bool isPointIn(PointF point)
		{
			return BoundRect.Contains(point);
		}

		public RectangleF GetLastModifiedRect()
		{
			int count = freeLine.Count;
			if (count < 2)
				return new RectangleF();
			RectangleF rect = Helpers.NormalizedRect(freeLine[count - 1], freeLine[count - 2]);
			rect.Offset(-5, -5);
			rect.Inflate(10, 10);
			return rect;
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition)
		{
			if (freeLine.Count < 2)
				return;
			linePen.Width = zoom*penSize;
			linePen.Color = penColor;
			linePen.DashStyle = penStyle;


			if (ImageViewPort.CastShadow)
			{
				var pointsShadow = new Point[freeLine.Count];
				for (int i = 0; i < freeLine.Count; i++)
					pointsShadow[i] = new Point((int) ((freeLine[i].X + 3)*zoom + offset.X),
					                            (int) ((freeLine[i].Y*zoom + 3) + offset.Y));

				using (var pen = new Pen(Color.FromArgb(64, 0, 0, 0)))
				{
					pen.Width = zoom*penSize;
					pen.DashStyle = penStyle;
					g.DrawLines(pen, pointsShadow);
				}
			}


			var points = new Point[freeLine.Count];
			for (int i = 0; i < freeLine.Count; i++)
			{
				points[i] = new Point((int) (freeLine[i].X*zoom + offset.X), (int) (freeLine[i].Y*zoom + offset.Y));
			}
			g.DrawLines(linePen, points);
			/*for (int i = 1; i < freeLine.Count; i++)
			{
				g.DrawLine(linePen, freeLine[i - 1].X * zoom + offsetX, freeLine[i - 1].Y * zoom + offsetY, freeLine[i].X * zoom + offsetX, freeLine[i].Y * zoom + offsetY);
			}*/
		}

		public override void CreationMouseDownHandler(PointF point)
		{
			AddPoint(point);
		}

		public override void CreationMouseUpHandler(PointF point)
		{
		}

		public override void MouseDoubleClickHandler(PointF point)
		{
		}

		public override void ShapeToolLost()
		{
		}

		public override void OnResize(bool lastMove)
		{
		}

		public override bool CanUndo()
		{
			return true;
		}


		public override void CreationMouseMoveHandler(PointF point)
		{
			AddPoint(point);
		}


		public override bool isResizeble()
		{
			return false;
		}

		public override void MoveBy(float dx, float dy)
		{
			for (int i = 0; i < freeLine.Count; i++)
			{
				PointF px = freeLine[i];
				px.X -= dx;
				px.Y -= dy;
				freeLine[i] = px;
			}
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
			return freeLine.Count > 0;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			return new FreeLinePropertyControl();
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
	}

	public class FreeLinePropertyControl : PropertyControlBase
	{
		private readonly ChangeColorControl colorControl;
		private readonly TpComboBoxLine lineDD;
		private readonly TpNumericUpDown numControl;
		private FreeLine attachedLine;

		public FreeLinePropertyControl()
		{
			AddLabel("Width");

			numControl = AddNumControl(1, 5, 1, 40);
			numControl.Value = FreeLine.penSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Color");

			colorControl = AddColorControl();
			colorControl.Color = FreeLine.penColorGlobal;
			colorControl.ColorChanged += colorControl_ValueChanged;

			AddLabel("Style");

			lineDD = new TpComboBoxLine();
			AddControl(lineDD, 100);


			lineDD.LineStyles = TPLineStyle.Solid | TPLineStyle.Dot | TPLineStyle.Dash | TPLineStyle.DashDot |
			                    TPLineStyle.DashDotDot;

			lineDD.SelectItem(FreeLine.penStyleGlobal);

			lineDD.SelectedIndexChanged += lineDD_ValueChanged;
		}

		private void lineDD_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penStyle = lineDD.SelectedDashStyleItem;
			}
			FreeLine.penStyleGlobal = lineDD.SelectedDashStyleItem;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void numControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penSize = numControl.Value;
			}
			FreeLine.penSizeGlobal = numControl.Value;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penColor = colorControl.Color;
			}
			FreeLine.penColorGlobal = colorControl.Color;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		public override void AttachTo(Primitive primitive)
		{
			if (primitive == null)
			{
				attachedLine = null;
			}
			else
			{
				attachedLine = primitive as FreeLine;
				numControl.Value = attachedLine.penSize;
				colorControl.Color = attachedLine.penColor;
				lineDD.SelectItem(attachedLine.penStyle);
			}
			Invalidate();
		}
	}
}