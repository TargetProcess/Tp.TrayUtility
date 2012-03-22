using System;
using System.Drawing;

namespace TpTrayUtility.Components
{
	public class Helpers
	{
		public static RectangleF NormalizedRect(PointF p1, PointF p2)
		{
			return new RectangleF(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), 
			                     Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
		}
	}
}