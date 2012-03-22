// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace TpTrayUtility.Components
{
	public class FontFamilyCollection : List<FontFamily>
	{
		public FontFamilyCollection()
			: base(new InstalledFontCollection().Families)
		{
		}

		public FontFamily FindByName(string fontFamilyName)
		{
			return this.Where(f => f.Name == fontFamilyName).Single();
		}
	}
}