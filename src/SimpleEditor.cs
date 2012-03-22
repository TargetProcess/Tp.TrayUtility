//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Components.Tools;
using TpTrayUtility.Properties;

namespace TpTrayUtility
{
	public partial class SimpleEditor : TpForm
	{
		private readonly ToolControl toolControl = new ToolControl();
		private ImageViewPort viewPort;
		private ImageLibrary library;

		public SimpleEditor()
		{
			InitializeComponent();

			CreateViewport();
			CreateLibrary();

			Controls.Add(viewPort);
			Controls.Add(toolControl);					
			
			viewPort.Select();
			viewPort.SelectTool(TpTool.Select);

			Size = new Size(800, 600);
		}

		private void library_PictureSelected(object sender, EventArgs e)
		{
			Controls.Remove(library);
			Controls.Add(viewPort);
			viewPort.Select();
			OnResize(new EventArgs());

			var bmpFile = new Bitmap((string) sender);
			var copyBitmap = new Bitmap(bmpFile);
			bmpFile.Dispose();
			viewPort.AddPicture(copyBitmap, true);
		}

		private void CreateLibrary()
		{
			library = new ImageLibrary {Location = new Point(1, 70)};
			library.PictureSelected += library_PictureSelected;
		}

		private void CreateViewport()
		{
			viewPort = new ImageViewPort();

			toolControl.AddToolButton(Resources.send_bug, "Submit!").Click += submit_Click;			
			toolControl.AddVerticalSeparator();

			var selectTool = toolControl.AddToolButton(Resources.select_tool, "Selection Tool");
			selectTool.Click += selectionTool_Click;
			selectTool.Togle = true;
			toolControl.AddToolButton(Resources.line_tool, "Line Tool").Click += lineTool_Click;
			toolControl.AddToolButton(Resources.square_tool, "Box Tool").Click += boxTool_Click;
			toolControl.AddToolButton(Resources.free_line_tool, "Free Line Tool").Click += freeLoneTool_Click;

			toolControl.AddToolButton(Resources.text_tool, "Text Tool").Click += textTool_Click;
			toolControl.AddVerticalSeparator();

			toolControl.AddToolButton(Resources.undo, "Undo").Click += undo_Click;
			toolControl.AddToolButton(Resources.redo, "Redo").Click += redo_Click;
			toolControl.AddToolButton(Resources.delete, "Delete").Click += delete_Click;
			toolControl.AddVerticalSeparator();

			toolControl.AddToolButton(Resources.plus, "Zoom In").Click += zoomIn_Click;
			toolControl.AddToolButton(Resources.minus, "Zoom Out").Click += zoomOut_Click;
			toolControl.AddToolButton(Resources.original_size, "Normal").Click += normalSize_Click;
			toolControl.AddVerticalSeparator();

			toolControl.AddToolButton(Resources.save, "Save to File").Click += saveButton_Click;
			toolControl.AddToolButton(Resources.save, "Open").Click += open_Click;
			toolControl.AddVerticalSeparator();

			toolControl.AddToolButton(Resources.new_capture, "Capture another Screen").Click += captureAnotherScreen_Click;
			

			Resizeble = true;
			viewPort.Size = new Size(100, 100);
			viewPort.Location = new Point(1, 70);

			toolControl.Location = new Point(1, 32);
		}

		private void selectionTool_Click(object sender, EventArgs e)
		{
			SetSelectedTool(sender, TpTool.Select);
		}

		private void lineTool_Click(object sender, EventArgs e)
		{
			SetSelectedTool(sender, TpTool.Line);
		}

		private void boxTool_Click(object sender, EventArgs e)
		{
			SetSelectedTool(sender, TpTool.Box);
		}

		private void freeLoneTool_Click(object sender, EventArgs e)
		{
			SetSelectedTool(sender, TpTool.FreeLine);
		}

		private void textTool_Click(object sender, EventArgs e)
		{
			SetSelectedTool(sender, TpTool.TextBox);
		}

		private void undo_Click(object sender, EventArgs e)
		{
			viewPort.UndoLastAction();
		}

		private void redo_Click(object sender, EventArgs e)
		{
			viewPort.RedoLastAction();
		}

		private void delete_Click(object sender, EventArgs e)
		{
			viewPort.DeletePrimitive();
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			viewPort.RemoveSelection();

			saveFileDialog.ShowDialog();
			if (saveFileDialog.FileName == "") return;

			var extension = saveFileDialog.Filter.Split("|".ToCharArray())[(saveFileDialog.FilterIndex - 1)*2 + 1];
			var imageCodec = ImageCodecInfo.GetImageEncoders().Single(ici =>  ici.FilenameExtension.ToLower().Contains(extension.ToLower()));

			var bmp = viewPort.RenderToBitmap();
			if (bmp == null) return;

			var wrapper = new BitmapWrapper(bmp);
			var fs = (FileStream)saveFileDialog.OpenFile();
			wrapper.Save(fs, imageCodec, 100);
			wrapper.Dispose();
		}

		private void open_Click(object sender, EventArgs e)
		{
			Controls.Remove(viewPort);
			library.ShowFiles(false);
			Controls.Add(library);
			OnResize(new EventArgs());
		}

		private void captureAnotherScreen_Click(object sender, EventArgs e)
		{
			Back();
		}

		private void submit_Click(object sender, EventArgs e)
		{
			viewPort.RemoveSelection();
			SubmitBug();
		}

		private void zoomIn_Click(object sender, EventArgs e)
		{
			viewPort.ZoomIn();
		}

		private void zoomOut_Click(object sender, EventArgs e)
		{
			viewPort.ZoomOut();
		}

		private void normalSize_Click(object sender, EventArgs e)
		{
			viewPort.ZoomNormal();
		}

		public void Setup(Bitmap bmpToEdit)
		{
			viewPort.AddPicture(bmpToEdit, false);
		}

		protected override void OnResize(EventArgs e)
		{
			if (viewPort.Parent != null)
				viewPort.Size = new Size(Size.Width - 2, Size.Height - 80);
			else
				library.Size = new Size(Size.Width - 2, Size.Height - 80);
			toolControl.Width = Size.Width - 2;
			base.OnResize(e);
		}

		private void HideForm()
		{
			Hide();
		}

		private void SetSelectedTool(object sender, TpTool tool)
		{
			viewPort.RemoveSelection();
			toolControl.UnTogleAll();
			((TpToolButton)sender).Togle = true;
			viewPort.SelectTool(tool);
		}

		private void SubmitBug()
		{
			string fileName = viewPort.CreateTemporaryPicture();
			Cursor = Cursors.WaitCursor;
			var frm = FormsManager.GetInstance<SubmitBugForm>();
			Cursor = Cursors.Arrow;
			if (frm == null) return;

			if (fileName != null)
				frm.AddFile(fileName, true);

			FormsManager.Show(typeof (SubmitBugForm));
		}

		private void Back()
		{
			FormsManager.GetInstance<TPEMain>().AddBug();
		}

		private void SimpleEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				HideForm();
			}

			if (ModifierKeys == Keys.Control && e.KeyCode == Keys.Z)
			{
				viewPort.UndoLastAction();
			}

			if (e.Alt && e.KeyCode == Keys.F4)
			{
				e.Handled = true;
				HideForm();
			}

			if (e.KeyCode == Keys.Delete)
			{
				viewPort.DeletePrimitive();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
//			if (e.Delta > 0)
//				viewPort.ZoomIn(e.Location);
//			else if (e.Delta < 0)
//				viewPort.ZoomOut(e.Location);
		}

		internal void ShowEditor()
		{
			Controls.Remove(library);
			Controls.Add(viewPort);
			viewPort.Select();
			OnResize(new EventArgs());
			FormsManager.Show(typeof (SimpleEditor));
		}

		private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
	}
}