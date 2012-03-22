//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Components.Tools;
using TpTrayUtility.Properties;

namespace TpTrayUtility
{
	public class TpCheckBox : CheckBox
	{
		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			pevent.Graphics.DrawImageUnscaled(Checked ? Resources.checkbox_on : Resources.checkbox_off, 0, 0);
			pevent.Graphics.DrawString(Text, UI.DefaultFont, new SolidBrush(Color.Gray), 20, 0);			
		}
	}


	public class KeyPressedInfo
	{
		public KeyPressedInfo()
		{
			Modifiers = 0;
			KeyValue = 0;
			KeyCode = Keys.None;
		}

		public KeyPressedInfo(KeyPressedInfo inf)
		{
			Modifiers = inf.Modifiers;
			KeyValue = inf.KeyValue;
			KeyCode = inf.KeyCode;
		}

		public KeyModifiers Modifiers;
		public int KeyValue;
		public Keys KeyCode;

		public KeyPressedInfo(int keyValue, KeyModifiers modifiers)
		{
			KeyValue = keyValue;
			Modifiers = modifiers;
			KeyCode = (Keys) keyValue;
		}


		public bool isEqual(KeyPressedInfo info)
		{
			return KeyValue == info.KeyValue && Modifiers == info.Modifiers && KeyCode == info.KeyCode;
		}
	}

	public partial class HotKeyControl : Control
	{
		public String HotKeyName
		{
			get { return Text; }
		}

		

		private KeyPressedInfo _currentKey = new KeyPressedInfo();

		private readonly TpButton clearButton = new TpButton();

		public event EventHandler<EventArgs> NewKeyDefined = delegate { };

		public HotKeyControl()
		{
			clearButton.ButtonType = ButtonType.Default;
			clearButton.Size = new Size(40, 28);
			clearButton.Text = Resources.HotKeyControl_HotKeyControl_Clear;
			clearButton.Click += clearButton_Click;
			Controls.Add(clearButton);						
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			_currentKey = new KeyPressedInfo();
			
			UpdateKeyInfo(_currentKey);
			NewKeyDefined.Invoke(this, null);
		}		

		public void SetHotKeyValue(KeyPressedInfo keyPressedInfo)
		{
			_currentKey = keyPressedInfo;
			UpdateKeyInfo(_currentKey);
		}

		public KeyPressedInfo CurrentKey
		{
			get { return _currentKey; }
		}


		protected override void OnKeyDown(KeyEventArgs e)
		{
			_currentKey.KeyValue = 0;
			_currentKey.Modifiers = 0;
			_currentKey.KeyCode = e.KeyCode;
			
			if (e.KeyValue >= 32 || e.KeyValue == 13)
				_currentKey.KeyValue = e.KeyValue;

			if (e.Shift)
				_currentKey.Modifiers = _currentKey.Modifiers | KeyModifiers.Shift;

			if (e.Alt)
				_currentKey.Modifiers = _currentKey.Modifiers | KeyModifiers.Alt;

			if (e.Control)
				_currentKey.Modifiers = _currentKey.Modifiers | KeyModifiers.Control;

			UpdateKeyInfo(_currentKey);

			e.SuppressKeyPress = true;
			e.Handled = true;
			
		}

		protected override void OnClick(EventArgs e)
		{
			Focus();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			Invalidate();
			base.OnLostFocus(e);
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (_currentKey.KeyValue == 0)
				_currentKey.Modifiers = 0;
			
			UpdateKeyInfo(_currentKey);

			if (e.KeyValue == _currentKey.KeyValue)
			{				
				NewKeyDefined.Invoke(this, null);
			}

			e.SuppressKeyPress = true;
			e.Handled = true;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.FromArgb(255, 255, 255, 255));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			var sf = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
			e.Graphics.DrawString(Text, UI.SmallFont, Brushes.Black, new Rectangle(0, 0, Width - 60, Height), sf);

			if (!Focused) return;

			using (var border = RoundedRectangle.Create(1, 1, Width - 6 - clearButton.Width, Height - 4, 5))
			{
				using (var white = new Pen(Brushes.Green))
				{
					white.Width = 1;
					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
					e.Graphics.DrawPath(white, border);
				}
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{			
			base.OnSizeChanged(e);
			clearButton.Location = new Point(Width - clearButton.Width + 2, -3);
		}

		private void UpdateKeyInfo(KeyPressedInfo currentKey)
		{
			var value = new StringBuilder();

			if ((currentKey.Modifiers & KeyModifiers.Shift) > 0)
				value.Append("Shift");
			
			if ((currentKey.Modifiers & KeyModifiers.Alt) > 0)
			{
				if(!value.IsEmpty())					
					value.Append(" + ");
				value.Append("Alt");
			}

			if ((currentKey.Modifiers & KeyModifiers.Control) > 0)
			{
				if (!value.IsEmpty())
					value.Append(" + ");
				value.Append("Control");				
			}

			if (currentKey.KeyCode.IsSystemKey())
			{
				if (!value.IsEmpty())
					value.Append(" + ");				
				value.Append(currentKey.KeyCode.GetString());					
			}
			else if (currentKey.KeyValue > 0)
			{
				if (!value.IsEmpty())
					value.Append(" + ");
				value.Append(Convert.ToChar(MapVirtualKeyEx((uint) currentKey.KeyValue, 2, GetKeyboardLayout(0))).ToString());									
			}

			Text = value.ToString();
			Invalidate();
		}

		[DllImport("user32.dll")]
		private static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, int dwhkl);
		//[DllImport("user32.dll")]
		//private static extern uint MapVirtualKey(uint uCode, uint uMapType);


		[DllImport("user32.dll")]
		private static extern int GetKeyboardLayout(uint threadId);
		

		private void captureBugHotKey_KeyUp(object sender, KeyEventArgs e)
		{
			
		}

		private void captureBugHotKey_KeyDown(object sender, KeyEventArgs e)
		{
			
		}		
	}
}