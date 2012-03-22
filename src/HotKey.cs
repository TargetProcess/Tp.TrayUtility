//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility
{

	[Flags]
	public enum KeyModifiers
	{
		None = 0,
		Alt = 1,
		Control = 2,
		Shift = 4,
		Windows = 8
	}


	public class HotKey : IMessageFilter
	{
		#region System		

		private const int WM_HOTKEY = 0x0312;
		private readonly int keyID = 100;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		#endregion

		private IntPtr handle;

		public IntPtr Handle
		{
			get { return handle; }
			set { handle = value; }
		}

		private event EventHandler HotKeyPressed;

		private Keys key;
		private KeyModifiers modifier;

		public HotKey(int _keyID, Keys _key, KeyModifiers _modifier, EventHandler hotKeyPressed)
		{
			keyID = _keyID;
			HotKeyPressed = hotKeyPressed;
			key = _key;
			modifier = _modifier;
			Start();
		}

		public HotKey(IntPtr h, int _keyID, Keys _key, KeyModifiers _modifier, EventHandler hotKeyPressed)
		{
			Handle = h;
			keyID = _keyID;
			HotKeyPressed = hotKeyPressed;
			key = _key;
			modifier = _modifier;
			Start();
		}

		private bool isStarted;

		public void Start()
		{
			if (isStarted) 
				return;

			ReRegisterHotKey();
			Application.AddMessageFilter(this);
			isStarted = true;
		}

		public void Stop()
		{
			if (isStarted)
			{
				Application.RemoveMessageFilter(this);
				UnregisterHotKey(handle, keyID);
				isStarted = false;
			}
		}

		~HotKey()
		{
			Stop();
		}

		private void ReRegisterHotKey()
		{
			UnregisterHotKey(handle, keyID);
			RegisterHotKey(key, modifier);
			return;
		}

		private bool RegisterHotKey(Keys _key, KeyModifiers _modifier)
		{
			return key == Keys.None || RegisterHotKey(handle, keyID, _modifier, _key);
		}

		public bool CanRegistred(Keys _key, KeyModifiers _modifier)
		{
			if (RegisterHotKey(handle, keyID, _modifier, _key))
			{
				UnregisterHotKey(handle, keyID);
				key = _key;
				modifier = _modifier;
				return true;
			}
			return false;
		}


		public bool PreFilterMessage(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_HOTKEY:
					if (m.WParam.ToInt32() == keyID && SettingsManager.HotKeyEnabled)
					{
						HotKeyPressed(this, new EventArgs());
						return true;
					}
					break;
			}
			return false;
		}


		internal void Run(bool p)
		{
			if (p)
				Start();
			else
				Stop();
		}
	}
}