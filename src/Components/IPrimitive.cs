//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public interface IPrimitive
	{
		void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition);
		void SetColor(Color fore, Color back);
		bool isPointIn(PointF point);
		void CreationMouseDownHandler(PointF point);
		void CreationMouseUpHandler(PointF point);
		void CreationMouseMoveHandler(PointF point);
		void MouseDoubleClickHandler(PointF point);
		void ShapeToolLost();
		RectangleF BoundRect { set; get; }
		void MoveBy(float dx, float dy);
		bool isResizeble();
		void OnResize(bool lastMove);
		List<ShapeBoxControlPoint> SpecialPoints();
		void UpdateSpecialPoint(int id, PointF position);
		bool isValid();
		PropertyControlBase GeneratePropertyControl();
		void UpdatePropertyFromStaticData();
		Cursor GetCursor(PointF point);
		bool CanUndo();
	}
}