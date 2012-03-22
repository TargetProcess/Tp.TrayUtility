// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;

namespace TpTrayUtility.Components
{
	public class ShapeBoxControlPoint
	{
		public PointF point;

		public PointF Position
		{
			get { return point; }
			set { point = value; }
		}

		private int id;

		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		public ShapeBoxControlPoint(float x, float y, int _id)
		{
			point = new PointF(x, y);
			id = _id;
		}
	}
}