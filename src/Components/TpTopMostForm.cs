//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class TpTopMostForm : Form
	{
		private Label lblLoadingEntities;

		public TpTopMostForm()
		{
			FormBorderStyle = FormBorderStyle.None;
			MinimizeBox = false;
			MaximizeBox = false;
			ControlBox = false;
			ShowInTaskbar = false;
			Capture = true;

			Initialize();
		}

		public void ResetScroll()
		{
			HorizontalScroll.Value = 0;
		}

		private void Initialize()
		{
			lblLoadingEntities = new Label {Text = @"Loading Entities...", Visible = false};
			Controls.Add(lblLoadingEntities);
		}

		public bool IsLoadingEntities
		{
			get { return lblLoadingEntities.Visible; }
			set { lblLoadingEntities.Visible = value; }
		}		
	}
}