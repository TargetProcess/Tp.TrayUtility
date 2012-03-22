// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using TpTrayUtility.Controls;

namespace TpTrayUtility.Components
{
	public class SelectionToolPropertyControl : PropertyControlBase
	{
		private Line attachedLine;
		private readonly TpNumericUpDown numControl;
		private readonly ChangeColorControl colorControl;
		private readonly TpComboBoxLine lineDD = new TpComboBoxLine();

		public SelectionToolPropertyControl()
		{
			AddLabel("Width");

			numControl = AddNumControl(1, 5, 1, 40);
			numControl.Value = Line.penSizeGlobal;
			numControl.ValueChanged += numControl_ValueChanged;

			AddLabel("Color");

			colorControl = AddColorControl();
			colorControl.Color = Line.penColorGlobal;
			colorControl.ColorChanged += colorControl_ValueChanged;

			AddLabel("Style");

			AddControl(lineDD, 100);
			lineDD.SelectedIndexChanged += lineDD_ValueChanged;
		}

		private void lineDD_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penStyle = lineDD.SelectedDashStyleItem;
			}
			Line.penStyleGlobal = lineDD.SelectedDashStyleItem;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void numControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penSize = numControl.Value;
			}
			Line.penSizeGlobal = numControl.Value;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (attachedLine != null)
			{
				attachedLine.penColor = colorControl.Color;
			}
			Line.penColorGlobal = colorControl.Color;
			if (PropertyChanged != null)
				PropertyChanged.Invoke(null, new EventArgs());
		}

		public override void AttachTo(Primitive primitive)
		{
			if (primitive == null)
			{
				attachedLine = null;
			}
			else
			{
				attachedLine = primitive as Line;
				numControl.Value = attachedLine.penSize;
				colorControl.Color = attachedLine.penColor;
				lineDD.SelectItem(attachedLine.penStyle);
			}
			Invalidate();
		}
	}
}