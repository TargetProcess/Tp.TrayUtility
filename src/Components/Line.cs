//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TpTrayUtility.Controls;

namespace TpTrayUtility.Components
{
	public class Line : Primitive
	{
		protected Pen linePen;
		protected PointF begin;
		protected PointF end;

		public static Color penColorGlobal = Color.Red;
		public static int penSizeGlobal = 3;
		public static DashStyle penStyleGlobal = DashStyle.Solid;
		public static LineCap penBeginCapGlobal = LineCap.ArrowAnchor;
		public static LineCap penEndCapGlobal = LineCap.Flat;


		public Color penColor;
		public int penSize;
		public DashStyle penStyle;
		public LineCap penBeginCap;
		public LineCap penEndCap;


		public Line()
		{
			UpdatePropertyFromStaticData();
			linePen = new Pen(penColor, 1);
		}

		public override bool isPointIn(PointF point)
		{
			return BoundRect.Contains(point);
		}

		public RectangleF GetLastModifiedRect()
		{
			return Helpers.NormalizedRect(begin, end);
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition)
		{
			linePen.Width = zoom*penSize;

			linePen.DashStyle = penStyle;
			linePen.StartCap = penBeginCap;
			linePen.EndCap = penEndCap;

			if (ImageViewPort.CastShadow)
			{
				linePen.Color = Color.FromArgb(64, 0, 0, 0);
				g.DrawLine(linePen, (end.X + 3)*zoom + offset.X, (end.Y + 3)*zoom + offset.Y, (begin.X + 3)*zoom + offset.X,
				           (begin.Y + 3)*zoom + offset.Y);
			}

			linePen.Color = penColor;

			g.DrawLine(linePen, end.X*zoom + offset.X, end.Y*zoom + offset.Y, begin.X*zoom + offset.X, begin.Y*zoom + offset.Y);
		}

		public override void SetColor(Color fore, Color back)
		{
		}

		public override void CreationMouseDownHandler(PointF point)
		{
			begin = point;
			end = point;
		}

		public override void CreationMouseUpHandler(PointF point)
		{
		}


		public override void MouseDoubleClickHandler(PointF point)
		{
		}


		public override void CreationMouseMoveHandler(PointF point)
		{
			end = point;
		}


		public override void ShapeToolLost()
		{
		}

		public override RectangleF BoundRect
		{
			get
			{
				RectangleF rx = Helpers.NormalizedRect(begin, end);
				rx.Inflate(2, 2);
				return rx;
			}

			set { }
		}


		public override bool isResizeble()
		{
			return false;
		}

		public override void MoveBy(float dx, float dy)
		{
			begin.X -= dx;
			begin.Y -= dy;
			end.X -= dx;
			end.Y -= dy;
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
			return begin.X != end.X || begin.Y != end.Y;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			return new LinePropertyControl();
		}

		public override void UpdatePropertyFromStaticData()
		{
			penColor = penColorGlobal;
			penSize = penSizeGlobal;
			penStyle = penStyleGlobal;
			penBeginCap = penBeginCapGlobal;
			penEndCap = penEndCapGlobal;
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


	internal class TpLineStyleItem : TpItem
	{
		public class TpPenStyle
		{
			public DashStyle style;
			public LineCap beginCap;
			public LineCap endCap;

			public TpPenStyle(DashStyle _style, LineCap _beginCap, LineCap _endCap)
			{
				style = _style;
				beginCap = _beginCap;
				endCap = _endCap;
			}

			public bool isEqual(TpPenStyle st)
			{
				return style == st.style && beginCap == st.beginCap && endCap == st.endCap;
			}
		}

		public TpPenStyle style;

		public TpLineStyleItem(TpPenStyle _style)
		{
			style = _style;
		}

		public TpLineStyleItem(DashStyle _style)
		{
			style = new TpPenStyle(_style, LineCap.Flat, LineCap.Flat);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (Pen pen = new Pen(Brushes.Black, 5))
			{
				pen.DashStyle = style.style;
				pen.StartCap = style.beginCap;
				pen.EndCap = style.endCap;
				e.Graphics.DrawLine(pen, 2, Height/2 - 1, Width - 4, Height/2 - 1);
			}
			base.OnPaint(e);
		}

		public override void SpecialDrawingFunction(Graphics g, Rectangle rc)
		{
			using (Pen pen = new Pen(Brushes.Black, 5))
			{
				pen.DashStyle = style.style;
				pen.StartCap = style.beginCap;
				pen.EndCap = style.endCap;
				g.DrawLine(pen, rc.Left + 2, rc.Height/2 - 1, rc.Width - 24, rc.Height/2 - 1);
			}
		}


		public override TpItem Clone()
		{
			TpLineStyleItem item = new TpLineStyleItem(style);
			return item;
		}

		public override bool Contain(object data)
		{
			TpPenStyle st = data as TpPenStyle;
			if (st != null)
				return style.isEqual((TpPenStyle) data);
			else
				return style.style == (DashStyle) data;
		}
	}


	public class LinePropertyControl : PropertyControlBase
	{
		private Line attachedLine;
		private readonly TpNumericUpDown numControl;
		private readonly ChangeColorControl colorControl;
		private readonly TpComboBoxLine lineDD = new TpComboBoxLine();

		public LinePropertyControl()
		{
			AddLabel("Width");

			numControl = AddNumControl(1, 10, 1, 40);
			numControl.Value = Line.penSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Color");

			colorControl = AddColorControl();
			colorControl.Color = Line.penColorGlobal;
			colorControl.ColorChanged += colorControl_ValueChanged;

			AddLabel("Style");


			AddControl(lineDD, 150);

			lineDD.LineStyles = TPLineStyle.Arrow | TPLineStyle.Solid | TPLineStyle.Dot | TPLineStyle.Dash | TPLineStyle.DashDot |
			                    TPLineStyle.DashDotDot;

			lineDD.SelectItem(Line.penStyleGlobal);
			lineDD.SelectedIndexChanged += lineDD_ValueChanged;
		}

		private void lineDD_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penStyle = lineDD.SelectedDashStyleItem;
				attachedLine.penBeginCap = lineDD.SelectedStyleItem == TPLineStyle.Arrow ? LineCap.ArrowAnchor : LineCap.Flat;
				attachedLine.penEndCap = LineCap.Flat;
			}
			Line.penStyleGlobal = lineDD.SelectedDashStyleItem;
			Line.penBeginCapGlobal = lineDD.SelectedStyleItem == TPLineStyle.Arrow ? LineCap.ArrowAnchor : LineCap.Flat;
			Line.penEndCapGlobal = LineCap.Flat;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}


		private void numControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penSize = numControl.Value;
			}
			Line.penSizeGlobal = numControl.Value;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penColor = colorControl.Color;
			}
			Line.penColorGlobal = colorControl.Color;
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
				attachedLine = primitive as Line;
				numControl.Value = attachedLine.penSize;
				colorControl.Color = attachedLine.penColor;
				if (attachedLine.penBeginCap == LineCap.ArrowAnchor)
					lineDD.SelectItem(TPLineStyle.Arrow);
				else
					lineDD.SelectItem(attachedLine.penStyle);
			}
			Invalidate();
		}
	}
}