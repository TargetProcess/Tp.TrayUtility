//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace TpTrayUtility.Components
{
	public class SettingsManager
	{
		public enum Keys
		{
			PasswordStored,
			UseIntegratedAuth,
			TargetProcessPath,
			UserName,
			UserPassword,
			LastProject,
			HotKeyCaptureBugValue,
			HotKeyCaptureBugModifier,
			HotKeyDashboardValue,
			HotKeyDashboardModifier,
			KnownAssignments,
			FirstTimeRunning,
			HotKeysEnabled,
			ForceLogin,
		}

		public static string APP_KEY = "HKEY_CURRENT_USER\\Software\\TargetProcess\\TP.Tray";

		static SettingsManager()
		{
			Registry.CurrentUser.CreateSubKey(GetCleanKey());
		}

		public static void ClearSettings()
		{
			var key = GetCleanKey();
			Registry.CurrentUser.CreateSubKey(key);
			Registry.CurrentUser.DeleteSubKey(key);
			Registry.CurrentUser.CreateSubKey(key);
		}

		private static string GetCleanKey()
		{
			return APP_KEY.Replace("HKEY_CURRENT_USER\\", string.Empty);
		}

		private static string GetValue(Keys keys, string defaultValue)
		{
			return (string) Registry.GetValue(APP_KEY, keys.ToString(), defaultValue);
		}

		private static void SetValue(Keys keys, object value)
		{
			if (value == null)
				value = string.Empty;

			Registry.SetValue(APP_KEY, keys.ToString(), value.ToString());
		}

		public static bool PasswordStored
		{
			get { return Boolean.Parse(GetValue(Keys.PasswordStored, false.ToString())); }
			set { SetValue(Keys.PasswordStored, value); }
		}

		public static bool UseIntegratedAuth
		{
			get { return Boolean.Parse(GetValue(Keys.UseIntegratedAuth, false.ToString())); }
			set { SetValue(Keys.UseIntegratedAuth, value); }
		}


		public static bool FirstTimeRunning
		{
			get { return Boolean.Parse(GetValue(Keys.FirstTimeRunning, true.ToString())); }
			set { SetValue(Keys.FirstTimeRunning, value); }
		}

		public static bool HotKeyEnabled
		{
			get { return Boolean.Parse(GetValue(Keys.HotKeysEnabled, true.ToString())); }
			set { SetValue(Keys.HotKeysEnabled, value); }
		}


		public static string UserName
		{
			get
			{
				return UseIntegratedAuth ? WindowsIdentity.GetCurrent().Name : GetValue(Keys.UserName, String.Empty);
			}
			set { SetValue(Keys.UserName, value); }
		}

		public static string UserPassword
		{
			get { return GetValue(Keys.UserPassword, String.Empty); }
			set { SetValue(Keys.UserPassword, value); }
		}

		public static int HotKeyCaptureBugValue
		{
			get { return Int32.Parse(GetValue(Keys.HotKeyCaptureBugValue, "66")); } //Ctrl + Alt + B  (B == "66")
			set { SetValue(Keys.HotKeyCaptureBugValue, value.ToString()); }
		}

		public static KeyModifiers HotKeyCaptureBugModifier
		{
			get { return (KeyModifiers) Enum.Parse(typeof (KeyModifiers), GetValue(Keys.HotKeyCaptureBugModifier, "3")); } //Ctrl + Alt + D (Ctrl + Alt == 3)
			set { SetValue(Keys.HotKeyCaptureBugModifier, ((int) value).ToString()); }
		}

		public static int HotKeyDashboardValue
		{
			get { return Int32.Parse(GetValue(Keys.HotKeyDashboardValue, "68")); } //Ctrl + Alt + D  (D == "68")
			set { SetValue(Keys.HotKeyDashboardValue, value.ToString()); }
		}

		public static KeyModifiers HotKeyDashboardModifier
		{
			get { return (KeyModifiers) Enum.Parse(typeof (KeyModifiers), GetValue(Keys.HotKeyDashboardModifier, "3")); } //Ctrl + Alt + B (Ctrl + Alt == 3)
			set { SetValue(Keys.HotKeyDashboardModifier, ((int) value).ToString()); }
		}

		public static string LastProject
		{
			get { return GetValue(Keys.LastProject, String.Empty); }
			set { SetValue(Keys.LastProject, value); }
		}

		public static string TargetProcessPath
		{
			get
			{
				string value = GetValue(Keys.TargetProcessPath, "http://");

				if ((!value.EndsWith("/") && !value.EndsWith("\\")))
					value += "/";

				return value;
			}
			set { SetValue(Keys.TargetProcessPath, value); }
		}


		public static List<AssignmentData> KnownAssignments
		{
			get
			{
				var value = new List<AssignmentData>();
				try
				{
					if (File.Exists(String.Format("{0}/assignment.xml", DataFolder())))
					{
						using (var fs = new FileStream(String.Format("{0}/assignment.xml", DataFolder()), FileMode.Open))
						{
							var xs = new XmlSerializer(typeof (List<AssignmentData>));
							value = (List<AssignmentData>) xs.Deserialize(fs);
						}
					}
				}
				catch
				{
				}
				return value;
			}

			set
			{
				Directory.CreateDirectory(DataFolder());

				using (var fs = new FileStream(String.Format("{0}/assignment.xml", DataFolder()), FileMode.Create))
				{
					var xs = new XmlSerializer(typeof (List<AssignmentData>));
					xs.Serialize(fs, value);
				}
			}
		}

		public static List<AssignmentData> UnreadAssignments
		{
			get
			{
				var value = new List<AssignmentData>();
				try
				{
					if (File.Exists(String.Format("{0}/unassignment.xml", DataFolder())))
					{
						using (var fs = new FileStream(String.Format("{0}/unassignment.xml", DataFolder()), FileMode.Open))
						{
							var xs = new XmlSerializer(typeof (List<AssignmentData>));
							value = (List<AssignmentData>) xs.Deserialize(fs);
						}
					}
				}
				catch
				{
				}
				return value;
			}

			set
			{
				Directory.CreateDirectory(DataFolder());

				using (var fs = new FileStream(String.Format("{0}/unassignment.xml", DataFolder()), FileMode.Create))
				{
					var xs = new XmlSerializer(typeof (List<AssignmentData>));
					xs.Serialize(fs, value);
				}
			}
		}


		private static string DataFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/TargetProcess/";			
		}


		public static bool ForceLogin
		{
			get { return Boolean.Parse(GetValue(Keys.ForceLogin, true.ToString())); }
			set { SetValue(Keys.ForceLogin, value); }
		}
	}
}