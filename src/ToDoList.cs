//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tp.MyAssignmentsServiceProxy;
using TpTrayUtility.Components;

namespace TpTrayUtility
{

	#region TpListViewSubItem

	internal class TpListViewSubItem : Control
	{
		public TpListViewSubItem(string text)
		{
			Control _control = new Label {Text = text};
			Controls.Add(_control);
			Size = _control.Size;
		}

		public TpListViewSubItem(Control control)
		{
			Controls.Add(control);
			Size = control.Size;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (Controls.Count > 0)
				Controls[0].Size = Size;
		}

		public string GetText()
		{
			if (Controls.Count > 0)
				return Controls[0].Text;
			return "";
		}
	}

	#endregion

	#region TpListViewItem

	internal class TpListViewItem : Control
	{
		public TpListViewSubItem Add(string text)
		{
			var subItem = new TpListViewSubItem(text);
			Controls.Add(subItem);
			UpdatePositions();
			return subItem;
		}

		public TpListViewSubItem Add(Control control)
		{
			var subItem = new TpListViewSubItem(control);
			Controls.Add(subItem);
			UpdatePositions();
			return subItem;
		}


		public void SetColumnsSize(int[] sizes)
		{
			int index = 0;
			foreach (int size in sizes)
			{
				if (index < Controls.Count)
					Controls[index].Size = new Size(size, Controls[index].Height);
				index++;
			}

			UpdatePositions();
		}

		private void UpdatePositions()
		{
			int pos = 0;
			int maxHeight = 0;
			foreach (Control control in Controls)
			{
				control.Location = new Point(pos, 0);
				pos += control.Width;
				if (maxHeight < control.Height)
					maxHeight = control.Height;
			}
			Size = new Size(pos, maxHeight);
		}

		public string GetTextForColumn(int column)
		{
			if (Controls.Count > column)
				return ((TpListViewSubItem) Controls[column]).GetText();
			return "";
		}
	}

	#endregion

	#region TpListViewHeader

	public enum TPListSortingOrder
	{
		Asc,
		Desc
	}

	public class HeaderClickEventArgs : EventArgs
	{
		public readonly int position;
		public readonly TPListSortingOrder sortingOrder;

		public HeaderClickEventArgs(int pos, TPListSortingOrder order)
		{
			position = pos;
			sortingOrder = order;
		}
	}

	public delegate void HeaderClickEventHandler(object sender, HeaderClickEventArgs args);

	internal class TpListViewHeader : Control
	{
		public class HeaderItem
		{
			public string Text;
			public int Width;
			public int MinWidth;
			public bool isActive;
			public TPListSortingOrder sortingOrder;

			public HeaderItem(string text, int width)
			{
				Text = text;
				Width = width;
				MinWidth = 30;
				isActive = false;
				sortingOrder = TPListSortingOrder.Asc;
			}
		}


		public readonly List<HeaderItem> headerItems = new List<HeaderItem>();
		private HeaderItem currentResizingItem;
		private HeaderItem highLightedItem;

		public event EventHandler ChangeSizeEvent;
		public HeaderClickEventHandler headerClickedEvent;


		public TpListViewHeader()
		{
			Width = 400;
			Height = 20;
		}

		public void AddHeaderItem(string text, int width)
		{
			headerItems.Add(new HeaderItem(text, width));			
		}

		private HeaderItem FindItemSplitter(int pos)
		{
			int offs = 0;
			foreach (var item in headerItems)
			{
				offs += item.Width;
				if (pos > offs - 4 && pos < offs + 4)
					return item;
			}
			return null;
		}

		private HeaderItem FindItem(int pos)
		{
			int offs = 0;
			foreach (HeaderItem item in headerItems)
			{
				if (pos > offs + 4 && pos < offs + item.Width - 4)
					return item;
				offs += item.Width;
			}
			return null;
		}


		private int GetItemPosition(HeaderItem it)
		{
			int offs = 0;
			foreach (var item in headerItems)
			{
				if (item == it)
					return offs;
				offs += item.Width;
			}
			return 0;
		}

		private int GetItemIndex(HeaderItem it)
		{
			int index = 0;
			foreach (var item in headerItems)
			{
				if (item == it)
					return index;
				index++;
			}
			return 0;
		}

		protected void DrawHeaderItem(Graphics g, int offs, HeaderItem item)
		{
			var rect = new Rectangle(offs + 2, 0, item.Width - 2, Height - 1);

			if (item.isActive)
			{
				g.FillRectangle(Brushes.LightBlue, rect);
			}

			g.DrawRectangle(Pens.Black, rect);

			var textDotsSize = TextRenderer.MeasureText("...", Font, new Size(100, 20), TextFormatFlags.SingleLine);
			var textSize = TextRenderer.MeasureText(item.Text, Font, new Size(item.Width, 20), TextFormatFlags.SingleLine);

			if (textSize.Width > item.Width)
			{
				g.DrawString(item.Text, Font, Brushes.Black,
				             new Rectangle(offs + 2, 0, item.Width - 2 - textDotsSize.Width, Height - 1),
				             new StringFormat(StringFormatFlags.NoWrap));
				g.DrawString("...", Font, Brushes.Black,
				             new Rectangle(offs + item.Width - 2 - textDotsSize.Width, 0, textDotsSize.Width, Height - 1),
				             new StringFormat(StringFormatFlags.NoWrap));
			}
			else
			{
				g.DrawString(item.Text, Font, Brushes.Black, rect, new StringFormat(StringFormatFlags.NoWrap));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			int offs = 0;
			foreach (var item in headerItems)
			{
				DrawHeaderItem(e.Graphics, offs, item);
				offs += item.Width;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (currentResizingItem == null)
			{
				ResetActiveFlag();
				var highLight = FindItem(e.X);
				if (highLight != null && highLight != highLightedItem)
				{
					highLight.isActive = true;
					highLightedItem = highLight;
					Invalidate();
				}

				Cursor = Cursors.Arrow;
				if (FindItemSplitter(e.X) != null)
					Cursor = Cursors.VSplit;
			}
			else
			{
				int width = e.X - GetItemPosition(currentResizingItem);
				if (width > 30)
					currentResizingItem.Width = width;
				Invalidate();
			}
			base.OnMouseMove(e);
		}

		private void ResetActiveFlag()
		{
			foreach (HeaderItem item in headerItems)
			{
				item.isActive = false;
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			var item = FindItem(e.X);
			if (item != null && headerClickedEvent != null)
			{
				item.sortingOrder = item.sortingOrder == TPListSortingOrder.Asc 
					? TPListSortingOrder.Desc 
					: TPListSortingOrder.Asc;

				headerClickedEvent.Invoke(this, new HeaderClickEventArgs(GetItemIndex(item), item.sortingOrder));
			}
			base.OnMouseClick(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			currentResizingItem = FindItemSplitter(e.X);
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			ChangeSizeEvent(this, new EventArgs());
			currentResizingItem = null;
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			ResetActiveFlag();
			currentResizingItem = null;
			base.OnMouseLeave(e);
			Invalidate();
		}


		public void Set(string[] texts, int[] widths)
		{
			for (int i = 0; i < texts.Length; i++)
			{
				AddHeaderItem(texts[i], widths.Length > i ? widths[i] : 50);
			}
		}


		public int[] GetSizes()
		{
			var array = new int[headerItems.Count];
			int i = 0;
			foreach (var item in headerItems)
			{
				array[i] = item.Width;
				i++;
			}
			return array;
		}	
	}

	#endregion

	#region TpListView

	internal class TpListView : Panel
	{
		private readonly TpListViewHeader header;

		public TpListView()
		{
			AutoScroll = true;
			header = new TpListViewHeader();
			Controls.Add(header);
			header.ChangeSizeEvent += header_ChangeSizeEvent;
			header.headerClickedEvent += header_ClickToSort;
		}

		private void header_ChangeSizeEvent(object sender, EventArgs e)
		{
			UpdateControl();
		}


		private TPListSortingOrder currentSortingOrder = TPListSortingOrder.Asc;
		private int currentSortingColumnt;

		private int CompareViewItem(TpListViewItem item1, TpListViewItem item2)
		{
			string text1;
			string text2;

			if (currentSortingOrder == TPListSortingOrder.Asc)
			{
				text1 = item1.GetTextForColumn(currentSortingColumnt);
				text2 = item2.GetTextForColumn(currentSortingColumnt);
			}
			else
			{
				text2 = item1.GetTextForColumn(currentSortingColumnt);
				text1 = item2.GetTextForColumn(currentSortingColumnt);
			}

			return string.Compare(text1, text2);
		}

		private void header_ClickToSort(object sender, HeaderClickEventArgs e)
		{
			currentSortingOrder = e.sortingOrder;
			currentSortingColumnt = e.position;

			var tmpList = new List<TpListViewItem>();

			for (int i = Controls.Count - 1; i > 0; i--)
			{
				if (Controls[i] is TpListViewItem)
				{
					tmpList.Add((TpListViewItem) Controls[i]);
					Controls.RemoveAt(i);
				}
			}

			tmpList.Sort(CompareViewItem);

			foreach (TpListViewItem item in tmpList)
				Controls.Add(item);
		}

		public void UpdateControl()
		{
			var sz = header.GetSizes();
			SetColumnsSize(sz);
			UpdatePositions();
		}


		public void AddHeader(string[] texts, int[] widths)
		{
			header.Set(texts, widths);
			Width = header.Width;
		}


		public TpListViewItem Add(string[] data)
		{
			var viewItem = new TpListViewItem();
			foreach (string text in data)
				viewItem.Add(text);
			Controls.Add(viewItem);
			UpdateControl();
			return viewItem;
		}

		public TpListViewItem AddRow()
		{
			var viewItem = new TpListViewItem();
			Controls.Add(viewItem);
			UpdatePositions();
			return viewItem;
		}


		private void UpdatePositions()
		{
			int pos = header.Height;
			foreach (var control in Controls.OfType<TpListViewItem>())
			{
				control.Location = new Point(0, pos);
				pos += control.Height;
			}
		}

		protected void SetColumnsSize(int[] sizes)
		{
			foreach (Control control in Controls)
				if (control is TpListViewItem)
					((TpListViewItem) control).SetColumnsSize(sizes);
		}

		public void ClearAllItems()
		{
			Controls.Clear();
			Controls.Add(header);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			header.Width = Width;
			int[] sz = header.GetSizes();
			SetColumnsSize(sz);
			Invalidate();
		}
	}

	#endregion

	public class TaskTimerControl : Label
	{
		private readonly Timer timer;
		private DateTime startTime;
		private DateTime stopTime;

		public DateTime StartTime
		{
			get { return startTime; }
		}

		public DateTime StopTime
		{
			get { return stopTime; }
		}


		public TaskTimerControl()
		{
			timer = new Timer();
			timer.Tick += RefreshTimer;
			timer.Interval = 1;
		}

		public void Start()
		{
			startTime = DateTime.Now;
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
			stopTime = DateTime.Now;
		}

		public void Continue()
		{
			timer.Start();
		}

		private void RefreshTimer(object sender, EventArgs e)
		{
			timer.Interval = 30000;
			Text = Utility.GetElapsedTimeString(startTime, DateTime.Now);
			Invalidate();
		}
	}


	public partial class ToDoList : Form
	{
		private readonly TpListView todoList = new TpListView();

		private readonly ContextMenuStrip localStrip = new ContextMenuStrip();

		private bool isUpdatedData;
		private MyAssignments latestMyAssignments;

		private readonly Timer timer;

		private TaskTimerControl timeControl;


		public ToDoList()
		{
			Size = new Size(800, 400);
			todoList.Size = ClientSize;
			todoList.AddHeader(new[] {"ID", "Name", "Severity", "Status", "Spent Time", "Remain Time"},
			                   new[] {50, 300, 100, 50, 50, 50});			
			AssignmentRetreiver.assignmentsHandler += ReceiveNewAssignments;

			timer = new Timer();
			timer.Tick += RefreshToDoList;
			timer.Interval = 500;
			timer.Start();			
		}

		private void ReceiveNewAssignments(object sender, AssignmentRetreiverEventArgs args)
		{
			latestMyAssignments = args.Assignments;
			isUpdatedData = true;
		}


		private void RefreshToDoList(object sender, EventArgs e)
		{
			if (!isUpdatedData)
				return;

			timer.Stop();

			RefreshToDoList();
			isUpdatedData = false;
			timer.Start();
		}

		private void RefreshToDoList()
		{
			todoList.ClearAllItems();
			foreach (var assignment in latestMyAssignments.Assignables)
			{
				var item = todoList.AddRow();
				item.Add(assignment.AssignableID.ToString());
				item.Add(assignment.Name);
				item.Add(assignment.SeverityName);

				var state = new LinkLabel {Text = assignment.EntityStateName, Tag = assignment};
				state.Click += state_Click;
				item.Add(state);

				item.Add(assignment.TimeSpent.ToString());
				item.Add(assignment.TimeRemain.ToString());

				if (timeControl == null)
				{
					var start = new LinkLabel {Text = @"Start", Tag = assignment};
					start.Click += startTimeRecord;
					item.Add(start);
				}
				else if (((AssignableSimpleDTO) timeControl.Tag).ID == assignment.ID)
				{
					item.Add(timeControl);
				}
			}
			todoList.UpdateControl();
		}

		private void startTimeRecord(object sender, EventArgs e)
		{
			var assignable = (AssignableSimpleDTO) ((LinkLabel) sender).Tag;
			timeControl = new TaskTimerControl {Tag = assignable};
			timeControl.Start();
			timeControl.Click += timeControl_Click;
			RefreshToDoList();
		}

		private void timeControl_Click(object sender, EventArgs e)
		{
			timeControl.Stop();
			
			var timeForm = new TimeDescriptionForm
			               	{
			               		StartDate = timeControl.StartTime,
			               		FinishDate = timeControl.StopTime,
			               		AssignableID = ((AssignableSimpleDTO) timeControl.Tag).ID.Value
			               	};
			if (timeForm.ShowDialog() == DialogResult.OK)
			{
				timeControl = null;
			}
			else
			{
				timeControl.Continue();
			}
		}

		private void state_Click(object sender, EventArgs e)
		{
			localStrip.Items.Clear();
			var assignable = (AssignableSimpleDTO) ((LinkLabel) sender).Tag;
			if (assignable.NextStates != null && assignable.NextStates.Length > 0)
			{
				foreach (var entityState in assignable.NextStates)
				{
					localStrip.Items.Add(entityState.Name);
				}
			}

			localStrip.Show(Cursor.Position);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			todoList.Size = ClientSize;
		}
	}
}