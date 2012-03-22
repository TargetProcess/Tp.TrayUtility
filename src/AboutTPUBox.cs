//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TpTrayUtility.Components;

namespace TpTrayUtility
{
	internal partial class AboutTPUBox : TpForm
	{
		public AboutTPUBox()
		{
			InitializeComponent();

			//  Initialize the AboutBox to display the product information from the assembly information.
			//  Change assembly information settings for your application through either:
			//  - Project->Properties->Application->Assembly Information
			//  - AssemblyInfo.cs
			Text = String.Format("About {0}", AssemblyTitle);
			label2.Text = String.Format("Version {0}", AssemblyVersion);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0, 38, Width, 38);
			e.Graphics.DrawLine(Pens.DarkGray, 0, 39, Width, 39);
			base.OnPaint(e);
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle
		{
			get
			{
				// Get all Title attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
				// If there is at least one Title attribute
				if (attributes.Length > 0)
				{
					// Select the first one
					var titleAttribute = (AssemblyTitleAttribute) attributes[0];
					// If it is not an empty string, return it
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}
				// If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public string AssemblyDescription
		{
			get
			{
				// Get all Description attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute),
				                                                                          false);
				// If there aren't any Description attributes, return an empty string
				return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute) attributes[0]).Description;
				// If there is a Description attribute, return its value
			}
		}

		public string AssemblyProduct
		{
			get
			{
				// Get all Product attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
				// If there aren't any Product attributes, return an empty string
				return attributes.Length == 0 ? "" : ((AssemblyProductAttribute) attributes[0]).Product;
				// If there is a Product attribute, return its value
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				// Get all Copyright attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
				// If there aren't any Copyright attributes, return an empty string
				return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
				// If there is a Copyright attribute, return its value
			}
		}

		public string AssemblyCompany
		{
			get
			{
				// Get all Company attributes on this assembly
				var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
				// If there aren't any Company attributes, return an empty string
				return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute) attributes[0]).Company;
				// If there is a Company attribute, return its value
			}
		}

		#endregion

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SubmitBugForm.ShellExecute("open", "http://www.targetprocess.com");
		}
	}
}