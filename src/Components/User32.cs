// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Runtime.InteropServices;

namespace TpTrayUtility.Components
{
	public static class User32
	{
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, WM Msg, int wParam, int lParam);

		public enum WM
		{
			WM_PRINT = 0x0317
		}

		[DllImport("user32.dll", EntryPoint = "HideCaret")]
		public static extern long HideCaret(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "HideCaret")]
		public static extern long ShowCaret(IntPtr hwnd);
	}
}