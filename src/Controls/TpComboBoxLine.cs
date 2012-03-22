using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TpTrayUtility.Controls
{
	public class TpComboBoxLine : TpComboBox
	{
		protected override void InitilizeComponent()
		{
			
			Size = new System.Drawing.Size(170, 23);
			

		}
		private TPLineStyle _lineStyles;
		public TPLineStyle LineStyles
		{
			get { return _lineStyles; }
			set
			{
				_lineStyles = value;
				SetLineStyles();
			}
		}

		protected void SetLineStyles()
		{			
			var tpLineStyles = Enum.GetValues(typeof (TPLineStyle)).Cast<TPLineStyle>();
			var items = InnerHost.Items.Cast<ComboBoxItem>();

			items.Where(item => string.IsNullOrEmpty(item.Uid)).ToList().ForEach(i=> InnerHost.Items.Remove(i));

			foreach (var style in tpLineStyles)
			{
				if (((int) style & (int) LineStyles) == 0 || style == TPLineStyle.None) continue;
				var comboBoxItem = items.FirstOrDefault(item => item.Uid.Contains(style.ToString()));
				if(comboBoxItem != null)
				{
					comboBoxItem.Visibility = Visibility.Visible;
					comboBoxItem.IsEnabled = true;
				}
			}			
		}

		public void SelectItem(DashStyle style)
		{
			SelectItem(ToTPLineStyle(style));
		}

		public void SelectItem(TPLineStyle style)
		{			
			InnerHost.SelectedItem = InnerHost.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Uid.Contains(style.ToString()));
		}

		public TPLineStyle SelectedStyleItem
		{
			get
			{
				
				var tpLineStyles = Enum.GetValues(typeof(TPLineStyle)).Cast<TPLineStyle>();
				return tpLineStyles.FirstOrDefault(style => ((ComboBoxItem)InnerHost.SelectedItem).Uid == style.ToString() + "Line");
			}
		}

		public DashStyle SelectedDashStyleItem
		{
			get
			{
				return ToDashStyle(SelectedStyleItem);
			}
		}


		private static TPLineStyle ToTPLineStyle(DashStyle style)
		{
			switch (style)
			{
				case DashStyle.Solid:
					return TPLineStyle.Solid;
				case DashStyle.Dash:
					return TPLineStyle.Dash;
				case DashStyle.Dot:
					return TPLineStyle.Dot;
				case DashStyle.DashDot:
					return TPLineStyle.DashDot;
				case DashStyle.DashDotDot:
					return TPLineStyle.DashDotDot;
				case DashStyle.Custom:
					return TPLineStyle.Arrow;
				default:
					return TPLineStyle.None;
			}
		}

		private  static DashStyle ToDashStyle( TPLineStyle style )
		{
			switch (style)
			{
				case TPLineStyle.None:
					return 0;
				case TPLineStyle.Arrow:
					return DashStyle.Solid;
				case TPLineStyle.Solid:
					return DashStyle.Solid;
				case TPLineStyle.Dot:
					return DashStyle.Dot;
				case TPLineStyle.Dash:
					return DashStyle.Dash;
				case TPLineStyle.DashDot:
					return DashStyle.DashDot;
				case TPLineStyle.DashDotDot:
					return DashStyle.DashDotDot;
				default:
					return 0;
			}
		}
	}



	[Flags]
	public enum TPLineStyle : int
	{
		None		=	0x0,
		Arrow		=	0x1,
		Solid		=	0x2,
		Dot			=	0x4,
		Dash		=	0x8,
		DashDot		=	0x10,
		DashDotDot	=	0x20


	}
}