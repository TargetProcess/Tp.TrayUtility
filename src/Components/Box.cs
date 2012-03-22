//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class Box : Primitive
	{
		private RectangleF currentRectangle = new RectangleF(0, 0, 0, 0);

		public static Color borderColorGlobal = Color.Red;
		public static int borderSizeGlobal = 1;

		public Color borderColor;
		public int borderSize;


		public Box()
		{
			UpdatePropertyFromStaticData();
			SetColor(Color.Red, Color.White);
		}

		public override void UpdatePropertyFromStaticData()
		{
			borderColor = borderColorGlobal;
			borderSize = borderSizeGlobal;
		}

		private double Distance(Point p1, Point p2)
		{
			return Math.Sqrt((p1.X - p2.X)*(p1.X - p2.X) + (p1.Y - p2.Y)*(p1.Y - p2.Y));
		}

		public override bool isPointIn(PointF point)
		{
			return currentRectangle.Contains(point);
		}

		public override void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition)
		{
			var rect = Helpers.NormalizedRect(currentRectangle.Location,
			                                  new PointF(currentRectangle.Right, currentRectangle.Bottom));

			int x0 = (int) (rect.Left*zoom + offset.X);
			int y0 = (int) (rect.Top*zoom + offset.Y);
			int x1 = (int) (rect.Width*zoom);
			int y1 = (int) (rect.Height*zoom);

			if (x1 == 0 || y1 == 0)
				return;


			using (var pen = new Pen(borderColor))
			{
				if (ImageViewPort.CastShadow)
				{
					using (var pen2 = new Pen(Color.FromArgb(64, 0, 0, 0)))
					{
						pen2.Width = borderSize;
						g.DrawRectangle(pen2, x0 + (int) (3*zoom), y0 + (int) (3*zoom), x1, y1);
					}
				}

				pen.Width = borderSize;
				g.DrawRectangle(pen, x0, y0, x1, y1);
			}
		}

		public override void SetColor(Color fore, Color back)
		{
		}

		public override void CreationMouseDownHandler(PointF point)
		{
			currentRectangle.Location = point;
		}

		public override void CreationMouseUpHandler(PointF point)
		{
			var rect = Helpers.NormalizedRect(currentRectangle.Location, point);
			currentRectangle = rect;
		}

		public void EditFinishing(object sender, EventArgs e)
		{
			SwitchToView();
		}

		public override void ShapeToolLost()
		{
			SwitchToView();
		}

		private void SwitchToEdit()
		{
			UpdateTextControlSize();
		}

		private void UpdateTextControlSize()
		{
		}

		private void SwitchToView()
		{
		}


		public override void CreationMouseMoveHandler(PointF point)
		{
			currentRectangle.Size = new SizeF(point.X - currentRectangle.X, point.Y - currentRectangle.Y);
		}

		public override void MouseDoubleClickHandler(PointF point)
		{
			SwitchToEdit();
		}

		public override Cursor GetCursor(PointF point)
		{
			return Cursors.Hand;
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
			var points = new List<ShapeBoxControlPoint>();
			return points;
		}

		public override void UpdateSpecialPoint(int id, PointF position)
		{
		}

		public override bool isValid()
		{
			return currentRectangle.Width > 5 && currentRectangle.Height > 5;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			return new BoxPropertyControl();
		}

		public override void OnResize(bool lastMove)
		{
		}

		public override bool CanUndo()
		{
			return true;
		}
	}


	public class BoxPropertyControl : PropertyControlBase
	{
		private Box attachedBox;
		private readonly TpNumericUpDown numControl;
		private readonly ChangeColorControl colorBorderControl;

		public BoxPropertyControl()
		{
			AddLabel("Border Width");

			numControl = AddNumControl(1, 5, 1, 40);
			numControl.Value = Box.borderSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Border Color");

			colorBorderControl = AddColorControl();
			colorBorderControl.Color = Box.borderColorGlobal;
			colorBorderControl.ColorChanged += colorControl_ValueChanged;
		}


		private void numControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedBox != null)
			{
				attachedBox.borderSize = numControl.Value;
			}
			Box.borderSizeGlobal = numControl.Value;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedBox != null)
			{
				attachedBox.borderColor = colorBorderControl.Color;
			}
			Box.borderColorGlobal = colorBorderControl.Color;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}


		public override void AttachTo(Primitive primitive)
		{
			if (primitive == null)
			{
				attachedBox = null;
			}
			else
			{
				attachedBox = primitive as Box;
				numControl.Value = attachedBox.borderSize;
				colorBorderControl.Color = attachedBox.borderColor;
			}
			Invalidate();
		}
	}
}