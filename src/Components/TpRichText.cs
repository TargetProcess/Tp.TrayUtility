// 
// Copyright (c) 2005-2011 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public class TpRichText : RichTextBox
	{
		public EventHandler FinishEditingHandler;

		public TpRichText()
		{
			Multiline = true;
			BorderStyle = BorderStyle.None;
			ScrollBars = RichTextBoxScrollBars.None;
			OnSelectionChangeHandler = e => base.OnSelectionChanged(e);
			BringToFront();
		}

		public bool HasDifferentFonts
		{
			get
			{
				//Note: use this hardcode to determine when selected text with different font-size
				//Logic with changing selection and comparation font of each character is not working correctly - it breaks up selection mechanism
				return SelectionFont == null || SelectionFont.Size == 13;
			}
		}

		public static FontStyle GetDefaultStyle(FontFamily fontFamily)
		{
			if (fontFamily.IsStyleAvailable(FontStyle.Regular))
				return FontStyle.Regular;
			if (fontFamily.IsStyleAvailable(FontStyle.Bold))
				return FontStyle.Bold;
			if (fontFamily.IsStyleAvailable(FontStyle.Italic))
				return FontStyle.Italic;
			if (fontFamily.IsStyleAvailable(FontStyle.Underline))
				return FontStyle.Underline;
			if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
				return FontStyle.Strikeout;
			return FontStyle.Regular;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.Return)
					FinishEditing();
			}

			base.OnKeyDown(e);
		}

		private void FinishEditing()
		{
			if (FinishEditingHandler != null)
			{
				FinishEditingHandler.Invoke(this, new EventArgs());
			}
		}

		private void SetFont(Func<FontStyle> getStyle, Func<FontFamily> getFamily, Func<float> getSize)
		{
			SelectionFont = GetFont(getFamily(), getStyle(), getSize());
		}

		private Font GetFont(FontFamily family, FontStyle style, float size)
		{
			if (family.IsStyleAvailable(style))
			{
				return new Font(family, size, style);
			}
			var styleWithDefault = style |= GetDefaultStyle(family);
			if (family.IsStyleAvailable(styleWithDefault))
			{
				return new Font(family, size, style);
			}

			return new Font(family, size, SelectionFont.Style);
		}

		private Action<EventArgs> OnSelectionChangeHandler;

		private void SetFontToSelection(Func<FontFamily> getFamily, Func<float> getSize, Func<FontStyle> getStyle)
		{
			if (!HasDifferentFonts)
			{
				SetFont(getStyle, getFamily, getSize);

				return;
			}

			OnSelectionChangeHandler = e => { };

			var selStart = SelectionStart;
			var selEnd = SelectionStart + SelectionLength;

			for (var i = selStart; i < selEnd; i++)
			{
				Select(i, 1);

				SetFont(getStyle, getFamily, getSize);
			}

			Select(selStart, selEnd);

			OnSelectionChangeHandler = e => base.OnSelectionChanged(e);
		}

		protected override void OnSelectionChanged(EventArgs e)
		{
			OnSelectionChangeHandler(e);
		}

		public void SetFontToSelection(FontFamily family, FontStyle style)
		{
			SetFontToSelection(() => family, () => SelectionFont.Size, () => style);
		}

		public void SetFontToSelection(FontStyle style)
		{
			SetFontToSelection(() => SelectionFont.FontFamily, () => SelectionFont.Size, () => style);
		}

		public void SetFontToSelection(float size)
		{
			SetFontToSelection(() => SelectionFont.FontFamily, () => size, () => SelectionFont.Style);
		}
	}
}