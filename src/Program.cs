//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using TpTrayUtility.Components;

namespace TpTrayUtility
{
	internal static class Program
	{
		[DllImport("user32.dll")]
		public static extern int FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += Application_ThreadException;
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

			var current = Process.GetCurrentProcess();
			if (args.Length > 0 && args[0].Contains("-kill"))
			{
				var instances = Process.GetProcessesByName("TPTray");
				instances.Where(process => process.Id != current.Id).ToList().ForEach(p => p.Kill());				
			}
			else if (FindWindow(null, "TP.Tray") == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.VisualStyleState = VisualStyleState.ClientAndNonClientAreasEnabled;
				
				var retriever = new AssignmentRetreiver();
				var asm = new AssignmentsManager(SettingsManager.FirstTimeRunning);
				
				if (SettingsManager.FirstTimeRunning)
				{
					SettingsManager.FirstTimeRunning = false;
					SettingsManager.HotKeyEnabled = true;
				}
				
				Application.Run(FormsManager.GetInstance<TPEMain>());
			}
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Messenger.ShowUnhandleError();
			if (e.IsTerminating)
			{
				Environment.Exit(1);
			}
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Messenger.ShowUnhandleError();
		}
	}
}