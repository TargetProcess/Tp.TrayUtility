//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	public static class Messenger
	{
		public static DialogResult ShowError(string error)
		{
			return MessageBox.Show(error, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static DialogResult ShowIncorrectVersionError()
		{
			return ShowError("Version of the TP.Tray is not the same as version of the TargetProcess to which it connected.");
		}

		public static DialogResult ShowWarning(string warning)
		{
			return MessageBox.Show(warning, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static DialogResult ShowUnhandleError()
		{
			return ShowError("An unexpected error occurred. The application will be closed.");
		}
	}
}