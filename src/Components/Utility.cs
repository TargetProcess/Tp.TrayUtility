// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace TpTrayUtility.Components
{

	#region RoundedRectangle

	#endregion

	internal class Utility
	{
		public static string GetElapsedTimeString(DateTime startTime, DateTime finishTime)
		{
			long t1 = finishTime.Ticks/TimeSpan.TicksPerMinute;
			long difference = t1 - startTime.Ticks/TimeSpan.TicksPerMinute;
			long hours = difference/60;
			long minutes = difference - hours*60;
			return String.Format("{0}h:{1}m", hours, minutes);
		}

		public static GraphicsPath GetRoundPath(int radius, int offs_x, int offs_y, int width, int height)
		{
			var pth = new GraphicsPath();
			pth.AddArc(offs_x, offs_y, radius, height, 90, 180);
			pth.AddLine(offs_x + radius, offs_y, offs_x + width - radius, offs_y);
			pth.AddArc(offs_x + width - radius, offs_y, radius, height, 270, 180);
			pth.AddLine(pth.GetLastPoint(), pth.PathPoints[0]);
			return pth;
		}
	}

	public static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (var item in enumeration)
			{
				action(item);
			}
		}
	}
}