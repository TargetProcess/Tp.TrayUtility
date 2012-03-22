// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Drawing;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class TpMaxiMiniButton : TpBaseButton
	{
		public TpMaxiMiniButton()
		{
		}

		private bool _minimize = false;
		public bool Minimize 
		{
			get { return _minimize; }
			set
			{
				_minimize = value;
				if (_minimize)
				{
					Offset = new Point(0,2);
					ViewNormal = Resources.resize_min_default;
					ViewOver = Resources.resize_min_hover;
					ViewPressed = Resources.resize_min_pressed;
				}
				else
				{
					Offset = new Point(0, 0);
					ViewNormal = Resources.resize_default;
					ViewOver = Resources.resize_hover;
					ViewPressed = Resources.resize_pressed;
				}
				_minimize = value;
				Invalidate();
			}
		}
	}
}