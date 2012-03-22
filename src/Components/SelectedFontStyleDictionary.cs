// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TpTrayUtility.Components
{
	public class SelectedFontStyleDictionary : SelectedDictionary<FontStyle>
	{
		public FontStyle FindBy(FontFamily fontFamily)
		{
			FontStyle style = TpRichText.GetDefaultStyle(fontFamily);

			foreach (var pair in this)
			{
				if (pair.Value.Togled && fontFamily.IsStyleAvailable(pair.Key))
					style = (FontStyle) ((int) style | (int) pair.Key);
			}

			return style;
		}

		public FontStyle GetStyle()
		{
			FontStyle result = 0;

			foreach (var pair in this.Where(s => s.Value.Togled))
			{
				result |= pair.Key;
			}

			return result;
		}

		public void ApplyEnabled(FontFamily fontFamily)
		{
			var firstLevel = GetFirstLevelAvailibleStyles(fontFamily);
			
			this.ForEach(pair => pair.Value.Visible = firstLevel.Select(f => f.Key).Contains(pair.Key));

			if(firstLevel.Count == Count)
			{
				return;
			}

			if(firstLevel.Count == 1)
			{
				var firstStyle = firstLevel.First();

				GetSecondLevelAvailibleStyles(fontFamily, firstStyle.Key)
					.ForEach(pair => pair.Value.Visible = true);
			}
			else if(firstLevel.Count > 0)
			{
				firstLevel.First().Value.Visible = true;
			}
			else
			{
				this.ForEach(pair => pair.Value.Visible = false);
			}
		}

		private List<KeyValuePair<FontStyle, TogleButton>> GetFirstLevelAvailibleStyles(FontFamily family)
		{
			return this
				.Where(pair => family
					.IsStyleAvailable(pair.Key))
					.ToList();
		}

		private List<KeyValuePair<FontStyle, TogleButton>> GetSecondLevelAvailibleStyles(FontFamily family, FontStyle style)
		{
			return this
				.Where(pair => pair.Key != style)
				.Where(pair => family.IsStyleAvailable(style |= pair.Key))
				.ToList();
		}

		public void ApplyToggling(Font font)
		{
			if (font != null)
			{
				this.ForEach(pair => pair.Value.Togled = (pair.Key & font.Style) == pair.Key);
			}
		}

		

		public void UntoggleAll()
		{
			this.ForEach(pair => pair.Value.Togled = false);
		}
	}
}