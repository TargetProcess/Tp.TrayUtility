//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class FormsManager
	{
		private class FormInformation
		{
			private readonly Form form;
			private readonly bool keepAllive;
			private readonly Type formType;

			public FormInformation(Form form, bool keepAllive, Type formType)
			{
				this.form = form;
				this.keepAllive = keepAllive;
				this.formType = formType;
			}

			public Form Form
			{
				get { return form; }
			}

			public bool KeepAllive
			{
				get { return keepAllive; }
			}

			public Type FormType
			{
				get { return formType; }
			}
		}


		private static readonly List<FormInformation> registredForms;

		static FormsManager()
		{
			registredForms = new List<FormInformation>();
		}


		private static int GetRegistredIndex(Type formType)
		{
			for (int i = 0; i < registredForms.Count; i++)
			{
				if (registredForms[i].FormType == formType)
					return i;
			}
			return -1;
		}

		public static T GetInstance<T>() where T : Form
		{
			int index = GetRegistredIndex(typeof (T));
			if (index >= 0)
				return registredForms[index].Form as T;
			return Add(typeof (T)) as T;
		}

		public static Form CreateInstance(Type type, bool holdInstance)
		{
			Form form = (Form) Activator.CreateInstance(type);
			if (!form.IsDisposed)
			{
				form.FormClosing += formToShow_FormClosing;
				FormInformation formInfo = new FormInformation(form, holdInstance, type);
				registredForms.Add(formInfo);
				return form;
			}
			return null;
		}

		public static Form Add(Type type)
		{
			return CreateInstance(type, true);
		}

		public static Form Show(Type type)
		{
			return Show(type, true);
		}

		public static Form Show(Type type, bool holdInstance)
		{
			return Show(type, holdInstance, true);
		}

		public static Form Show(Type type, bool holdInstance, bool hideAll)
		{
			if (hideAll)
				HideForms();
			Form formToShow = GetFormToShow(holdInstance, type);
			if (formToShow != null)
				formToShow.Show();
			return formToShow;
		}

		public static Form ShowDialog(Type type, bool holdInstance, bool hideAll)
		{
			if (hideAll)
				HideForms();
			Form formToShow = GetFormToShow(holdInstance, type);
			formToShow.ShowDialog();
			return formToShow;
		}


		private static Form GetFormToShow(bool holdInstance, Type type)
		{
			Form formToShow;
			int index = GetRegistredIndex(type);
			if (index >= 0)
			{
				if (!holdInstance)
				{
					registredForms[index].Form.Close();					
					formToShow = CreateInstance(type, false);
				}
				else
				{
					formToShow = registredForms[index].Form;
				}
			}
			else
			{
				formToShow = CreateInstance(type, holdInstance);
			}
			return formToShow;
		}

		private static void formToShow_FormClosing(object sender, FormClosingEventArgs e)
		{
			int index = GetRegistredIndex(sender.GetType());
			if (index >= 0)
			{
				((Form) (sender)).FormClosing -= formToShow_FormClosing;
				registredForms.RemoveAt(index);
			}
		}

		public static void HideForms()
		{
			int i = 0;
			while (i < registredForms.Count)
			{
				if (registredForms[i].KeepAllive)
				{
					registredForms[i].Form.Hide();
					registredForms[i].Form.Invalidate();
					registredForms[i].Form.Update();
					i++;
				}
				else
				{
					registredForms[i].Form.FormClosing -= formToShow_FormClosing;
					registredForms[i].Form.Close();
					registredForms.RemoveAt(i);
				}
			}
		}

		public static void DisableForms()
		{
			SetEnablingToForms(false);
		}

		public static void EnableForms()
		{
			SetEnablingToForms(true);
		}

		private static void SetEnablingToForms(bool isEnable)
		{
			foreach (var form in registredForms)
			{
				form.Form.Enabled = isEnable;
			}
			GetInstance<TPEMain>().SetEnableNotifyIcon(isEnable);
			
		}

		public static void Hide(Type formType)
		{
			foreach (var form in registredForms.Where(form => form.FormType == formType))
			{
				form.Form.Hide();

				if (!form.KeepAllive)
				{
					form.Form.Close();
					registredForms.Remove(form);
				}
			}
		}
	}
}