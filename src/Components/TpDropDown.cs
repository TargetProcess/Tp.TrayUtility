//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TpTrayUtility.Components
{
	internal delegate bool Compare(TpItem item);

	internal class TpDropDown : Control
	{
		private bool isOpened;
		private readonly TpList _innerList = new TpList();
		private TpItem selectedItem;
		private TpTopMostForm topForm;
		private readonly GlobalMouseHook mouseHook = new GlobalMouseHook();

		private string filterMessage;
		public event EventHandler ValueChanged = null;
		public event EventHandler ListOpening = null;


		public TpDropDown()
		{
			SetStyle(ControlStyles.Selectable, true);
			DoubleBuffered = true;

			Size = new Size(120, 20);
			_innerList.ItemSelected += innerList_ItemSelected;
			_innerList.KeyPress += innerList_KeyPress;
			mouseHook.MouseClick += mouseHook_MouseClick;
		}

		~TpDropDown()
		{
			mouseHook.RemoveHook();
		}


		private void mouseHook_MouseClick(object sender, MouseEventArgs e)
		{
			HideList();
		}

		public TpItem SelectedItem
		{
			get { return selectedItem; }
		}

		protected override void OnResize(EventArgs e)
		{
			Region = new Region(RoundedRectangle.Create(0, 0, Width, Height, 2));
			_innerList.Width = Width;
			base.OnResize(e);
		}

		private void innerList_ItemSelected(object sender, EventArgs e)
		{
			TpItem item = sender as TpItem;
			if (item != null)
			{
				if (e != null)
					SelectItem(item);
				else
					SelectItem(item, true);
			}
		}

		public void SelectItem(TpItem item, bool supress)
		{
			selectedItem = item;

			if (!supress)
				HideList();
			if (ValueChanged != null && !supress)
			{
				ValueChanged.Invoke(this, new EventArgs());
			}
			Invalidate();
		}


		public void SelectItem(Compare function)
		{
			TpItem item = Find(function);
			if (item != null)
			{
				SelectItem(item, false);
			}
		}


		public void SelectItem(Object data)
		{
			TpItem item = Find(data);
			if (item != null)
			{
				SelectItem(item, false);
			}
		}

		public void SelectItem(Object data, bool supress)
		{
			TpItem item = Find(data);
			if (item != null)
			{
				SelectItem(item, supress);
			}
		}


		public int SelectedIndex
		{
			set
			{
				if (value >= 0)
					SelectItem(_innerList.GetAt(value));
			}
			get { return _innerList.SelectedIndex; }
		}

		public int ItemsCount
		{
			set { }
			get { return _innerList.ItemsCount; }
		}

		public void Add(TpItem item)
		{
			_innerList.Add(item);
			ReCalculate();
		}

		private void ReCalculate()
		{
			Controls.Clear();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Color apper;
			Color lower;
			Color apper2;
			Color lower2;
			Pen border1;
			Pen border2;

			if (isMouseDown)
			{
				apper = Color.FromArgb(255, 185, 185, 185);
				lower = Color.FromArgb(255, 190, 190, 190);

				apper2 = Color.FromArgb(255, 177, 177, 177);
				lower2 = Color.FromArgb(255, 163, 163, 163);

				border1 = new Pen(Color.FromArgb(255, 62, 62, 62));
				border2 = new Pen(Color.FromArgb(255, 210, 210, 210));
			}
			else if (isMouseIn)
			{
				apper = Color.FromArgb(255, 242, 242, 242);
				lower = Color.FromArgb(255, 236, 236, 236);

				apper2 = Color.FromArgb(255, 223, 223, 223);
				lower2 = Color.FromArgb(255, 210, 210, 210);

				border1 = new Pen(Color.FromArgb(255, 100, 100, 100));
				border2 = new Pen(Color.FromArgb(255, 250, 250, 250));
			}
			else
			{
				apper = Color.FromArgb(255, 242, 242, 242);
				lower = Color.FromArgb(255, 236, 236, 236);

				apper2 = Color.FromArgb(255, 223, 223, 223);
				lower2 = Color.FromArgb(255, 210, 210, 210);

				border1 = new Pen(Color.FromArgb(255, 140, 140, 140));
				border2 = new Pen(Color.FromArgb(255, 250, 250, 250));
			}

			using (var brush = new LinearGradientBrush(new Point(0, 2), new Point(0, Height/2), apper, lower))
			{
				e.Graphics.FillRectangle(brush, 0, 2, Width, Height/2);
			}
			using (var brush = new LinearGradientBrush(new Point(0, Height/2), new Point(0, Height - 2), apper2, lower2))
			{
				e.Graphics.FillRectangle(brush, 0, Height/2, Width, Height - 2);
			}

			border2.Width = 2;

			GraphicsPath path2 = RoundedRectangle.Create(1, 1, Width - 3, Height - 3, 2);
			e.Graphics.DrawPath(border2, path2);
			GraphicsPath path1 = RoundedRectangle.Create(0, 0, Width - 1, Height - 1, 2);
			e.Graphics.DrawPath(border1, path1);

			border1.Dispose();
			border2.Dispose();

			if (selectedItem != null)
			{
				selectedItem.SpecialDrawingFunction(e.Graphics, new Rectangle(6, 3, Width - 1, Height - 1));
			}

			var triangle = new GraphicsPath();
			int tx = Width - 17;
			int ty = 7;

			triangle.AddLine(tx, ty, tx + 9, ty);
			triangle.AddLine(tx + 9, ty, tx + 5, ty + 7);
			triangle.AddLine(tx + 5, ty + 7, tx, ty);
			e.Graphics.FillPath(Brushes.Black, triangle);
		}

		private bool isMouseDown;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			isMouseDown = true;
			if (!isOpened)
			{
				ShowList();
			}
			else
			{
				HideList();
			}
			Invalidate();
		}

		private bool _isLoadingEntities;
		public bool IsLoadingEntities
		{
			get
			{
				return _isLoadingEntities;
			}
			set
			{
				_isLoadingEntities = value;
				SelectItem(value ? new TpTextItem("Loading Entities...") : null, true);
			}
		}

		private void HideList()
		{
			mouseHook.RemoveHook();
			isOpened = false;
			if (topForm != null)
			{
				topForm.Close();
				topForm = null;
			}
		}

		private void ShowList()
		{
			if (ListOpening != null)
				ListOpening.Invoke(this, null);
			isOpened = true;
			topForm = new TpTopMostForm();
			topForm.Deactivate += (sender, args) => HideList();
			int a = topForm.Height;
			int b = Screen.PrimaryScreen.Bounds.Height;
			Point c = PointToScreen(new Point(0, Height));
			topForm.Controls.Add(_innerList);

			_innerList.Location = new Point(0, 0);
			bool t = _innerList.Focus();
			topForm.Size = _innerList.Size;

			if (PointToScreen(new Point(0, Height)).Y + topForm.Height > Screen.PrimaryScreen.Bounds.Height)
				topForm.Location = PointToScreen(new Point(0, -topForm.Height));
			else
				topForm.Location = PointToScreen(new Point(0, Height));

			mouseHook.SetHook();
			topForm.ShowDialog();
		}		

		private void innerList_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\b')
			{
				if (filterMessage.Length > 0)
					filterMessage = filterMessage.Substring(0, filterMessage.Length - 1);
				else
					HideList();
			}
			else
			{
				if (e.KeyChar >= 32)
					filterMessage += e.KeyChar;
			}
			_innerList.Filter(filterMessage);
			e.Handled = true;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			isMouseDown = false;
			Invalidate();
			base.OnMouseUp(e);
		}


		private bool isMouseIn;

		protected override void OnMouseEnter(EventArgs e)
		{
			isMouseIn = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			isMouseDown = false;
			isMouseIn = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		private TpItem Find(Object data)
		{
			return _innerList.Find(data);
		}

		private TpItem Find(Compare function)
		{
			return _innerList.Find(function);
		}


		internal void Clear()
		{
			_innerList.Clear();
			Controls.Clear();
			selectedItem = null;
		}

		public void Sort(Comparison<TpItem> comparison)
		{
			_innerList.Sort(comparison);
		}		
	}
}