// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class TpCloseButton : TpBaseButton
	{
		public TpCloseButton()
		{
			ViewNormal = Resources.close_default;
			ViewOver = Resources.close_hover;
			ViewPressed = Resources.close_pressed;
		}
	}
}