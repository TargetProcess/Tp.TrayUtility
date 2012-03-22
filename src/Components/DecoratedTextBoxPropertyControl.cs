// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;

namespace TpTrayUtility.Components
{
	public class DecoratedTextBoxPropertyControl : PropertyControlBase
	{
		private DecoratedTextBox attachedBox;
		private readonly TpNumericUpDown numControl;
		private readonly ChangeColorControl colorBorderControl;
		private readonly ChangeColorControl colorBackControl;

		public DecoratedTextBoxPropertyControl()
		{
			AddLabel("Border Width");

			numControl = AddNumControl(1, 5, 1, 40);
			numControl.Value = DecoratedTextBox.borderSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Border Color");

			colorBorderControl = AddColorControl();
			colorBorderControl.Color = DecoratedTextBox.borderColorGlobal;
			colorBorderControl.ColorChanged += colorControl_ValueChanged;

			AddLabel("Background Color");

			colorBackControl = AddColorControl();
			colorBackControl.Color = DecoratedTextBox.backColorGlobal;
			colorBackControl.ColorChanged += colorControl_ValueChanged2;
		}


		private void numControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedBox != null)
			{
				attachedBox.borderSize = numControl.Value;
			}
			DecoratedTextBox.borderSizeGlobal = numControl.Value;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedBox != null)
			{
				attachedBox.borderColor = colorBorderControl.Color;
			}
			DecoratedTextBox.borderColorGlobal = colorBorderControl.Color;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged2(object sender, EventArgs e)
		{
			if (attachedBox != null)
			{
				attachedBox.backColor = colorBackControl.Color;
			}
			DecoratedTextBox.backColorGlobal = colorBackControl.Color;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}


		public override void AttachTo(Primitive primitive)
		{
			if (primitive == null)
			{
				attachedBox = null;
			}
			else
			{
				attachedBox = primitive as DecoratedTextBox;
				numControl.Value = attachedBox.borderSize;
				colorBorderControl.Color = attachedBox.borderColor;
				colorBackControl.Color = attachedBox.backColor;
			}
			Invalidate();
		}
	}
}