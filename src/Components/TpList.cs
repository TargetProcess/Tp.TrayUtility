//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal class TpList : Control
	{
		private readonly List<TpItem> items = new List<TpItem>();
		private int topIndex;
		private readonly TpScrollBar scrollBar;

		public event EventHandler ItemSelected = null;

		public TpList()
		{
			scrollBar = new TpScrollBar(TpScrollBar.Style.Vertical);
			scrollBar.ValueChanged += scrollBar_ValueChanged;
			ReCalculate();
		}

		private void scrollBar_ValueChanged(object sender, EventArgs e)
		{
			topIndex = scrollBar.Position;
			ReCalculate();
			Invalidate();
		}

		public void Reset()
		{
			scrollBar.Position = 0;
			ReCalculate();
			Invalidate();
		}

		public void Add(TpItem item)
		{
			item.Width = Width - 20;
			items.Add(item);
			item.ItemClicked += item_ItemClicked;
			if (items.Count - 6 >= 0)
				scrollBar.PositionCount = items.Count - 7;
			ReCalculate();
		}

		private void item_ItemClicked(object sender, EventArgs e)
		{
			UnselectAll();
			((TpItem) sender).Selected = true;
			if (ItemSelected != null)
			{
				ItemSelected.Invoke(sender, e);
			}
			Invalidate();
		}

		public void ReCalculate()
		{
			Size = new Size(Width, 20*6);

			Controls.Clear();

			int outIndex = 0;
			int outHeight = 0;
			foreach (var item in items.Where(item => !item.Filtered))
			{
				if (outIndex >= topIndex && outHeight < Height)
				{
					item.Location = new Point(0, outHeight);
					outHeight += item.Height;
					Controls.Add(item);
				}
				outIndex++;
			}

			scrollBar.Location = new Point(Width - 20, 0);
			scrollBar.Size = new Size(20, Height);
			scrollBar.PositionCount = outIndex - 6;			
			Controls.Add(scrollBar);
		}

		internal TpItem Find(object data)
		{
			var tmpItem = data as TpItem;
			foreach (var item in items)
			{
				if (tmpItem != null)
				{
					if (tmpItem == item)
						return item;
				}
				else
				{
					if (item.Contain(data))
						return item;
				}
			}
			return null;
		}

		internal TpItem Find(Compare function)
		{
			return items.FirstOrDefault(item => function(item));
		}


		private static bool Filter(TpItem item)
		{
			return !item.Filtered;
		}

		public int SelectedIndex
		{
			set
			{
				UnselectAll();
				List<TpItem> filteredItems = items.FindAll(Filter);

				int si = value;
				if (filteredItems.Count == 0) return;
				if (value >= filteredItems.Count) si = filteredItems.Count - 1;
				if (si < 0) si = 0;
				filteredItems[si].Selected = true;

				if (si < topIndex)
					topIndex = si;
				if (si > topIndex + 5)
					topIndex = si - 5;

				scrollBar.Position = topIndex;
				ReCalculate();
				if (ItemSelected != null)
				{
					ItemSelected.Invoke(filteredItems[si], null);
				}
			}

			get { return (items.FindAll(Filter)).FindIndex(item => item.Selected); }
		}

		public void UnselectAll()
		{
			foreach (TpItem item in items)
				item.Selected = false;
		}

		public int ItemsCount
		{
			set { }
			get { return items.Count; }
		}

		public TpItem GetAt(int index)
		{
			return items[index];
		}

		internal void Clear()
		{
			items.Clear();
			Controls.Clear();
		}

		public void Filter(string p)
		{
			if (p == null) return;
			p = p.ToLower();
			foreach (var item in items)
			{
				var tx = item as TpTextItem;
				if (tx != null && p.Length > 0)
				{
					string tmp = ((string) tx.Tag).ToLower();
					item.Filtered = !tmp.Contains(p);
				}
				else
				{
					item.Filtered = false;
				}
			}
			SelectedIndex = 0;
		}

		public void Sort(Comparison<TpItem> comparison)
		{
			items.Sort(comparison);
			ReCalculate();
		}

		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down)
			{
				SelectedIndex++;
			}
			if (e.KeyCode == Keys.Up)
			{
				SelectedIndex--;
			}
			if (e.KeyCode == Keys.PageUp)
			{
				SelectedIndex -= 6;
			}
			if (e.KeyCode == Keys.PageDown)
			{
				SelectedIndex += 6;
			}
			if (e.KeyCode == Keys.Enter)
			{
				var filteredItems = items.FindAll(Filter);
				int index = SelectedIndex;
				if (ItemSelected != null && index >= 0 && index < filteredItems.Count)
				{
					ItemSelected.Invoke(filteredItems[index], new EventArgs());
				}
			}

			base.OnKeyDown(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta < 0)
			{
				SelectedIndex++;
			}
			else if (e.Delta > 0)
			{
				SelectedIndex--;
			}
			base.OnMouseWheel(e);
		}
	}
}