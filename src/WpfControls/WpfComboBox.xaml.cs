using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TpTrayUtility.WpfControls
{
	/// <summary>
	/// Interaction logic for UserControl2.xaml
	/// </summary>
	public partial class WpfComboBox : UserControl
	{
		public WpfComboBox()
		{
			InitializeComponent();

			comboBox1.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
		}

		public void AddItem(string item)
		{
			comboBox1.Items.Add(item);
		}

		public ItemCollection Items
		{
			get
			{
				return comboBox1.Items;
			}
		}

		public ComboBox Host
		{
			get { return comboBox1; }
		}
	}
}
