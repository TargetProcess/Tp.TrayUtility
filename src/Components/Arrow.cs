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
	public class Arrow : Primitive
	{

		protected Pen linePen;
		protected PointF begin;
		protected PointF end;

		public static Color penColorGlobal = Color.Red;
		public static int penSizeGlobal = 1;
		public static DashStyle penStyleGlobal = DashStyle.Solid;

		public Color penColor;
		public int penSize;
		public DashStyle penStyle;


		public Arrow()
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
			linePen.Width = zoom * penSize;
			linePen.Color = penColor;
			linePen.DashStyle = penStyle;

			g.DrawLine(linePen, begin.X*zoom + offset.X, begin.Y*zoom + offset.Y, end.X*zoom + offset.X, end.Y*zoom + offset.Y);
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
			return new ArrowPropertyControl();
		}

		public override void UpdatePropertyFromStaticData()
		{
			penColor = penColorGlobal;
			penSize = penSizeGlobal;
			penStyle = penStyleGlobal;
		}

		public override Cursor GetCursor(PointF point)
		{
			return Cursors.SizeAll;
		}

		public override void OnResize(bool lastMove)
		{
		}

		public override bool CanUndo()
		{
			return true;
		}
	}


	internal class TpArrowStyleItem : TpItem
	{
		//TpPenStyle style;
		public DashStyle style;

		public TpArrowStyleItem(DashStyle _style)
		{
			style = _style;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using (var pen = new Pen(Brushes.Black, 3))
			{
				pen.DashStyle = style;
				e.Graphics.DrawLine(pen, 2, Height / 2 - 1, Width - 4, Height / 2 - 1);
			}
			base.OnPaint(e);
		}

		public override TpItem Clone()
		{
			var item = new TpLineStyleItem(style);
			return item;
		}

		public override bool Contain(object data)
		{
			if ((DashStyle)data == style)
				return true;
			return false;
		}
	}


	public class ArrowPropertyControl : PropertyControlBase
	{
		private Line attachedLine;
		private readonly TpNumericUpDown numControl;
		private readonly ChangeColorControl colorControl;
		private readonly TpComboBoxLine lineDD = new TpComboBoxLine();

		public ArrowPropertyControl()
		{
			AddLabel("Width");

			numControl = AddNumControl(1, 5, 1, 40);
			numControl.Value = Line.penSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Color");

			colorControl = AddColorControl();
			colorControl.Color = Line.penColorGlobal;
			colorControl.ColorChanged += colorControl_ValueChanged;

			AddLabel("Style");


			AddControl(lineDD, 100);

			lineDD.LineStyles = TPLineStyle.Solid | TPLineStyle.Dot | TPLineStyle.Dash | TPLineStyle.DashDot |
			                    TPLineStyle.DashDotDot;			

			lineDD.SelectItem(Line.penStyleGlobal);

			lineDD.SelectedIndexChanged += lineDD_ValueChanged;
		}

		private void lineDD_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penStyle = lineDD.SelectedDashStyleItem;
			}
			Line.penStyleGlobal = lineDD.SelectedDashStyleItem;
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
				lineDD.SelectItem(attachedLine.penStyle);
			}
			Invalidate();
		}
	}
}