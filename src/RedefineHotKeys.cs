//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility
{
	public partial class RedefineHotKeys : TpForm
	{
		public RedefineHotKeys()
		{
			InitializeComponent();
			hotKeyControl1.SetHotKeyValue(new KeyPressedInfo(SettingsManager.HotKeyCaptureBugValue, SettingsManager.HotKeyCaptureBugModifier));
			hotKeyControl2.SetHotKeyValue(new KeyPressedInfo(SettingsManager.HotKeyDashboardValue, SettingsManager.HotKeyDashboardModifier));

			hotKeyControl1.NewKeyDefined += hotKeyControl1_NewKeyDefined;
			hotKeyControl2.NewKeyDefined += hotKeyControl2_NewKeyDefined;

			TPEMain._dashboardHotKey.Stop();
			TPEMain._bugHotKey.Stop();
		}

		protected override void OnClosed(EventArgs e)
		{
			TPEMain._dashboardHotKey.Run(SettingsManager.HotKeyEnabled);
			TPEMain._bugHotKey.Run(SettingsManager.HotKeyEnabled);
			base.OnClosed(e);
		}


		private void checkBox1_Click(object sender, EventArgs e)
		{
			SettingsManager.HotKeyEnabled = checkBox1.Checked;
		}

		private void hotKeyControl2_NewKeyDefined(object sender, EventArgs e)
		{
			if (TPEMain._dashboardHotKey.CanRegistred((Keys) hotKeyControl2.CurrentKey.KeyValue,
			                                         hotKeyControl2.CurrentKey.Modifiers))
			{
				SettingsManager.HotKeyDashboardValue = hotKeyControl2.CurrentKey.KeyValue;
				SettingsManager.HotKeyDashboardModifier = hotKeyControl2.CurrentKey.Modifiers;
			}
			else
			{
				MessageBox.Show(String.Format("HotKey '{0}' is already in use.", hotKeyControl2.HotKeyName));
				return;
			}
		}

		private void hotKeyControl1_NewKeyDefined(object sender, EventArgs e)
		{
			if (TPEMain._bugHotKey.CanRegistred((Keys) hotKeyControl1.CurrentKey.KeyValue, hotKeyControl1.CurrentKey.Modifiers))
			{
				SettingsManager.HotKeyCaptureBugValue = hotKeyControl1.CurrentKey.KeyValue;
				SettingsManager.HotKeyCaptureBugModifier = hotKeyControl1.CurrentKey.Modifiers;
			}
			else
			{
				MessageBox.Show(String.Format("HotKey '{0}' is already in use.", hotKeyControl1.HotKeyName));
			}
		}


		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}