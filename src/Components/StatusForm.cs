//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public partial class StatusForm : TpForm
	{
		public StatusForm()
		{
			InitializeComponent();
			ShowIcon = false;
		}

		public Label Message
		{
			get { return lblMessage; }
		}

		public Button BtnClose
		{
			get { return btnClose; }
		}

		public event EventHandler Hiding;

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (Hiding != null)
			{
				Hiding(sender, e);
			}
			Hide();
		}
	}
}