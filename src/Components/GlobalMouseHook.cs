// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class GlobalMouseHook : IDisposable
	{
		//Declare the wrapper managed POINT class.
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
		}

		//Declare the wrapper managed MouseHookStruct class.
		[StructLayout(LayoutKind.Sequential)]
		public class MouseHookStruct
		{
			public POINT pt;
			public int hwnd;
			public int wHitTestCode;
			public int dwExtraInfo;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
		                                          IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode,
		                                        IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);


		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_RBUTTONDOWN = 0x204;
		private const int WM_MBUTTONDOWN = 0x207;
		public const int WH_MOUSE = 7;
		public const int WH_MOUSE_LL = 14;

		public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		private int hHook;
		private HookProc MouseHookProcedure;

		public event MouseEventHandler MouseClick = null;

		public void Dispose()
		{
			RemoveHook();
		}

		public void SetHook()
		{
			MouseHookProcedure = new HookProc(MouseHookProc);
// ReSharper disable CSharpWarnings::CS0612
#pragma warning disable 618
			hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId());
#pragma warning restore 618
			// ReSharper restore CSharpWarnings::CS0612
		}

		public void RemoveHook()
		{
			if (hHook > 0)
			{
				UnhookWindowsHookEx(hHook);
			}
		}

		public int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			//Marshall the data from the callback.
			MouseHookStruct MyMouseHookStruct = (MouseHookStruct) Marshal.PtrToStructure(lParam, typeof (MouseHookStruct));

			if (nCode < 0)
			{
				return CallNextHookEx(hHook, nCode, wParam, lParam);
			}
			if (MouseClick != null)
			{
				if (((long) wParam == 161 || (long) wParam == 163 || (long) wParam == 165))
				{
					int x = lParam.ToInt32() & 0xFFFF;
					int y = (lParam.ToInt32() >> 16) & 0xFFFF;
					MouseEventArgs mea = new MouseEventArgs(MouseButtons.None, 0, x, y, 0);
					MouseClick.Invoke(null, mea);
				}
			}
			return CallNextHookEx(hHook, nCode, wParam, lParam);
		}
	} ;
}