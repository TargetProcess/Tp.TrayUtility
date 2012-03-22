// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class GDI32
	{
		[DllImport("gdi32.dll")]
		public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
		                                IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

		public enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020
		} ;

		public static void DrawControlToBitMap(Control srcControl, Bitmap destBitmap, Rectangle destBounds)
		{
			
			using (var srcGraph = srcControl.CreateGraphics())
			{
				var srcHdc = srcGraph.GetHdc();
				User32.SendMessage(srcControl.Handle, User32.WM.WM_PRINT, (int) srcHdc, 30);
				using (var destGraph = Graphics.FromImage(destBitmap))
				{
					var destHdc = destGraph.GetHdc();
					BitBlt(destHdc, destBounds.X, destBounds.Y, destBounds.Width, destBounds.Height,
					       srcHdc, 0, 0, TernaryRasterOperations.SRCCOPY);
					destGraph.ReleaseHdc(destHdc);
				}
				srcGraph.ReleaseHdc(srcHdc);
			}
		}
	}
}