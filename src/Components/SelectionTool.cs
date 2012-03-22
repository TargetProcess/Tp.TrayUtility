//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class SelectionTool : Primitive
	{
		public SelectionTool()
		{
			UpdatePropertyFromStaticData();
		}

		public override RectangleF BoundRect
		{
			get { return new Rectangle(); }

			set { }
		}

		public override bool isPointIn(PointF point)
		{
			return BoundRect.Contains(point);
		}

		public Rectangle GetLastModifiedRect()
		{
			return new Rectangle();
		}

		public override void Draw(Graphics g, float zoom, PointF offsetX, PointF offsetY)
		{
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


		public override bool isResizeble()
		{
			return false;
		}

		public override void MoveBy(float dx, float dy)
		{
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
			return false;
		}

		public override PropertyControlBase GeneratePropertyControl()
		{
			return new ArrowPropertyControl();
		}

		public override void UpdatePropertyFromStaticData()
		{
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
			return false;
		}
	}
}