//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public abstract class Primitive : IPrimitive
	{
		

		public abstract void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition);
		public abstract void SetColor(Color fore, Color back);
		public abstract bool isPointIn(PointF point);
		public abstract void CreationMouseDownHandler(PointF point);
		public abstract void CreationMouseUpHandler(PointF point);
		public abstract void CreationMouseMoveHandler(PointF pointF);
		public abstract void MouseDoubleClickHandler(PointF point);
		public abstract void ShapeToolLost();
		public abstract RectangleF BoundRect { get; set; }
		public abstract void MoveBy(float dx, float dy);
		public abstract bool isResizeble();
		public abstract void OnResize(bool lastMove);
		public abstract List<ShapeBoxControlPoint> SpecialPoints();
		public abstract void UpdateSpecialPoint(int id, PointF position);
		public abstract bool isValid();
		public abstract PropertyControlBase GeneratePropertyControl();
		public abstract void UpdatePropertyFromStaticData();
		public abstract Cursor GetCursor(PointF point);
		public abstract bool CanUndo();

		public virtual void Draw(Graphics g, float zoom, PointF offset, PointF zoomPosition, Point tbOffset)
		{
			Draw(g, zoom, offset, zoomPosition);
		}

		public virtual void OnFormResize()
		{}

		public Rectangle GetZoomBoundRect(float zoom )
		{
			return new Rectangle((int)(BoundRect.X * zoom), (int)(BoundRect.Y * zoom), (int)(BoundRect.Width * zoom), (int)(BoundRect.Height * zoom));
		}
	}
}

//protected abstract Rectangle CheckZoomBorders(Graphics g, Rectangle normalCoordinates, float zoom)
//        {
//            return CheckZoomBorders(g, normalCoordinates, zoom, new Point());
//        }

//        protected abstract Rectangle CheckZoomBorders(Graphics g, Rectangle normalCoordinates, float zoom, Point offset)
//        {
//            var x0 = (int) (normalCoordinates.Left*zoom) + offset.X;
//            var y0 = (int) (normalCoordinates.Top*zoom) + offset.Y;
//            var x1 = (int) (normalCoordinates.Width*zoom);
//            var y1 = (int) (normalCoordinates.Height*zoom);

//            //if (g.VisibleClipBounds.Width > x0 + x1 && x0 < 0 && x1 < g.VisibleClipBounds.Width)
//            //    x0 = 0;
//            //if (g.VisibleClipBounds.Width < x0 + x1 && x1 < g.VisibleClipBounds.Width)
//            //    x0 = (int)(g.VisibleClipBounds.Width - x1);
//            //if (g.VisibleClipBounds.Height > y0 + y1 && y0 < 0 && y1 < g.VisibleClipBounds.Height)
//            //    y0 = 0;
//            //if (g.VisibleClipBounds.Height < y0 + y1 && y1 < g.VisibleClipBounds.Height)
//            //    y0 = (int)(g.VisibleClipBounds.Height - y1);

//            return new Rectangle(x0, y0, x1, y1);
//        }