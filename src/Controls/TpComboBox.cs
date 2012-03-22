//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Shapes;
using TpTrayUtility.WpfControls;
using Control = System.Windows.Forms.Control;

namespace TpTrayUtility.Controls
{
	public class TpComboBox : Control
	{
		private WpfComboBox _comboBox;
		private ElementHost _elementHost;

		public event EventHandler DropDownOpened;
		public event SelectionChangedEventHandler SelectionChanged;
		public event EventHandler SelectedIndexChanged;

		public TpComboBox()
		{
			Initilize();			
		}

		protected ComboBox InnerHost 
		{ 
			get
			{
				return _comboBox.Host;
			} 
		}
		
		private void Initilize()
		{
			_comboBox = new WpfComboBox();
			_elementHost = new ElementHost();
			Controls.Add(_elementHost);
			_elementHost.Child = _comboBox;
			_elementHost.AutoSize = false;
			_comboBox.Host.DropDownOpened += Host_DropDownOpened;
			_comboBox.Host.SelectionChanged += Host_SelectionChanged;
			InitilizeComponent();
		}

		protected virtual void InitilizeComponent()
		{
			InnerHost.Items.Clear();
		}


		private void Host_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(SelectionChanged!=null)
			{
				SelectionChanged(sender, e);
			}
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(sender, EventArgs.Empty);
		}

		private void Host_DropDownOpened(object sender, EventArgs e)
		{
			if (DropDownOpened != null)
			{
				DropDownOpened(sender, EventArgs.Empty);
			}
		}

		public ItemCollection Items
		{
			get { return _comboBox.Host.Items; }
		}

		public ComboBoxItem SelectedItem
		{
			get { return (ComboBoxItem) _comboBox.Host.SelectedItem; }
		}

		public string SelectedItemText
		{
			get
			{				
				if (_comboBox.Host.SelectedItem != null)
					return ((ComboBoxItem) _comboBox.Host.SelectedItem).Content.ToString();
				return null;
			}
		}

		public new Size Size
		{
			get
			{
				return new Size((int)_comboBox.Host.Width, (int)_comboBox.Host.Height);
			}
			set
			{
				base.Size = value;
				_elementHost.Size = value;
				_comboBox.Width = _comboBox.Host.Width = value.Width;
				_comboBox.Height = _comboBox.Host.Height = value.Height;
				Update();
			}
		}

		
		private bool _isLoadingEntities;

		public bool IsLoadingEntities
		{
			get { return _isLoadingEntities; }
			set
			{
				if (value)
				{
					AddItem("Loading Entities");					
				}
				else
				{
					Items.Clear();
				}
				_isLoadingEntities = value;
			}
		}

		public int SelectedIndex
		{
			get
			{
				return _comboBox.Host.SelectedIndex;
			}
			set
			{
				_comboBox.Host.SelectedIndex = value;
			}
		}

		public void AddItem(string text, string value)
		{
			_comboBox.Host.Items.Add(new ComboBoxItem {Content = text, Uid = value, Width = _comboBox.Host.Width});
		}

		public void AddItem(string text)
		{
			_comboBox.Host.Items.Add(new ComboBoxItem {Content = text});
		}

		public void SelectItem(string text)
		{
			_comboBox.Host.SelectedItem = _comboBox.Host.Items.Cast<ComboBoxItem>().FirstOrDefault(item => (string) item.Content == text);
		}

		public void Clear()
		{
			lock (_comboBox.Host.Items)
			{
				_comboBox.Host.Items.Clear();	
			}			
		}

		
		public void SortByName()
		{			
			Sort((x, y) => String.Compare(x.Content.ToString(), y.Content.ToString()));
		}

		public void Sort(Comparison<ComboBoxItem> pattern)
		{
			var list = _comboBox.Host.Items.Cast<ComboBoxItem>().ToList();
			list.Sort(pattern);
			_comboBox.Host.Items.Clear();
			list.ForEach(l => _comboBox.Host.Items.Add(l));			
		}
	}
}