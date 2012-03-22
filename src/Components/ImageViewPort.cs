//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TpTrayUtility.Components.Tools;

namespace TpTrayUtility.Components
{
	internal class ImageViewPort : ScrollableControl
	{
		#region Fields

		public static ImageViewPort _viewPortInstance;
		private static readonly Color CustomBackColor = Color.FromArgb(255, 144, 144, 144);
		private static readonly Color OffCanvasColor = Color.FromArgb(255, 48, 48, 48);

		private static bool _castShadow = true;

		private readonly Collection<Primitive> _allPainting = new Collection<Primitive>();
		private readonly NavigationControl _navigationControl;
		private readonly Collection<Primitive> _redoStack = new Collection<Primitive>();

		private readonly ShapeBoxControl _shapeControl;

		private readonly float[] _zooms = {
		                                  	0.1f, 0.15f, 0.25f, 0.33f, 0.5f, 0.66f, 0.75f, 0.90f, 1.0f, 1.1f, 1.25f, 1.5f, 2.0f
		                                  	, 3.0f, 4.0f, 6.0f, 8.0f
		                                  };


		private PointF _currentDelta;
		private Primitive _currentPrimitive;
		private int _dragPointID = -1;
		private bool _isPenDown;
		private PointF _lastMousePosition;
		private float _offsetX;
		private float _offsetY;
		private bool _resizeMode;		
		private TpTool _selectedTool;
		private float _zoom = 1.0f;
		private PointF _zoomLocation = new PointF(0, 0);

		public static bool CastShadow
		{
			get { return _castShadow; }
			set { _castShadow = value; }
		}

		#endregion

		#region Constructor

		public ImageViewPort()
		{
			_viewPortInstance = this;
			_currentPrimitive = new SelectionTool();
			DoubleBuffered = true;
			_shapeControl = new ShapeBoxControl(null);
			_navigationControl = new NavigationControl();						
		
			SetStyle(
				ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
				ControlStyles.DoubleBuffer, true);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			if(SelectionExists && SelectedPrimitive is DecoratedTextBox)
				return;
			
			Select();
		}
		

		protected override void OnScroll(ScrollEventArgs se)
		{
			ClearSelection();
			base.OnScroll(se);
			Invalidate();
		}

		#endregion

		#region Properties

		private PointF PictureOffset
		{
			get { return new PointF(-_offsetX, -_offsetY); }
		}

		private bool SelectionExists
		{
			get { return SelectedPrimitive != null; }
		}

		private Primitive SelectedPrimitive
		{
			get { return _shapeControl.Primitive; }
			set { _shapeControl.Primitive = value; }
		}

		#endregion

		#region Methods

		public void SelectTool(TpTool tool)
		{
			_selectedTool = tool;
			ToolChanged();
		}

		public void MoveViewPort(int x, int y)
		{
			_offsetX = x;
			_offsetY = y;
			Update();
			Invalidate();
		}

		public void UndoLastAction()
		{
			if (_allPainting.Count > 0)
			{
				if (_allPainting[_allPainting.Count - 1].CanUndo())
				{
					if (SelectedPrimitive == _allPainting[_allPainting.Count - 1])
					{
						ResetSelectedPrimitive();
					}
					_redoStack.Add(_allPainting[_allPainting.Count - 1]);
					_allPainting.RemoveAt(_allPainting.Count - 1);
				}
			}
			Invalidate();
			Select();
		}

		public void RedoLastAction()
		{
			if (_redoStack.Count > 0)
			{
				_allPainting.Add(_redoStack[_redoStack.Count - 1]);
				_redoStack.RemoveAt(_redoStack.Count - 1);
			}
			Invalidate();
			Select();
		}

		public void AddPicture(Bitmap bmp, bool isSelected)
		{
			var pict = new Picture(bmp);
			pict.MoveBy(-20, -20);
			_allPainting.Add(pict);
			_redoStack.Clear();

			if (isSelected)
				SelectedPrimitive = pict;
			UpdateCurrentTool();
			_currentPrimitive = null;

			Invalidate();
		}

		public Bitmap RenderToBitmap()
		{
			var rect = CalculateGlobalBounds(false);
			if (rect.Width > 0 && rect.Height > 0)
			{
				var bmp = new Bitmap((int) rect.Width, (int) rect.Height);

				var graphics = Graphics.FromImage(bmp);
				graphics.Clear(CustomBackColor);

				graphics.SmoothingMode = SmoothingMode.AntiAlias;


				_allPainting.ForEach(primitive => DrawPrimitive(primitive, graphics, 1.0f, new PointF(-rect.Left, -rect.Top)));


				return bmp;
			}

			return null;
		}

		private void DrawPrimitive(Primitive primitive, Graphics graphics, float zoom, PointF offset)
		{
			primitive.Draw(graphics, zoom, offset, _zoomLocation, AutoScrollPosition);
		}

		public void RenderToGraphics(Graphics g, float zoom)
		{
			var rect = CalculateGlobalBounds();
			if (rect.Width > 0 && rect.Height > 0)
			{
				g.Clear(CustomBackColor);
				g.SmoothingMode = SmoothingMode.AntiAlias;

				foreach (var primitive in _allPainting)
					DrawPrimitive(primitive, g, zoom, new Point((int) (-rect.Left*zoom), (int) (-rect.Top*zoom)));
			}
		}

		public void ZoomIn(Point location)
		{
			_zoomLocation = location;
			int i = 0;
			for (; i < _zooms.Length; i++)
			{
				if (_zoom <= _zooms[i])
					break;
			}

			if (i + 1 < _zooms.Length)
			{
				_zoom = _zooms[i + 1];
			}

			AfterZoomRerender();
		}

		private void NormalizeEx()
		{
			_allPainting.ForEach(primitive => primitive.MoveBy(-_currentDelta.X/_zoom, -_currentDelta.Y/_zoom));
		}

		public void ZoomOut(Point location)
		{
			
			_zoomLocation = location;
			int i = 0;
			for (; i < _zooms.Length; i++)
			{
				if (_zoom <= _zooms[i])
					break;
			}

			if (i > 0)
			{
				_zoom = _zooms[i - 1];
			}

			AfterZoomRerender();
		}

		public void ZoomNormal()
		{
			_zoom = 1.0f;

			AfterZoomRerender();
		}

		private void ClearSelection()
		{
			if (SelectionExists)
			{
				SelectedPrimitive.ShapeToolLost();
				SelectedPrimitive = null;
				UpdateCurrentTool();
			}
		}

		private void AfterZoomRerender()
		{
			ClearSelection();																	
			Invalidate();
			NormalizeEx();
			Invalidate();
			Select();
		}

		public void DeletePrimitive()
		{
			if (!SelectionExists || !SelectedPrimitive.CanUndo()) return;

			RemoveAllProperties();

			_allPainting.Remove(SelectedPrimitive);
			ResetSelectedPrimitive();
			Invalidate();
		}

		public void ToolChanged()
		{
			_currentPrimitive = null;
			RemoveAllProperties();

			PropertyControlBase propertyControl = null;

			switch (_selectedTool)
			{
				case TpTool.FreeLine:
					propertyControl = new FreeLinePropertyControl();
					break;
				case TpTool.Line:
					propertyControl = new LinePropertyControl();
					break;
				case TpTool.TextBox:
					propertyControl = new DecoratedTextBoxPropertyControl();
					break;
				case TpTool.Box:
					propertyControl = new BoxPropertyControl();
					break;
			}

			AddProperties(propertyControl);
			UpdatePropertiesView();
			Focus();
		}

		public void UpdateCurrentTool()
		{
			RemoveAllProperties();
			if (!SelectionExists) return;
			

			PropertyControlBase property = SelectedPrimitive.GeneratePropertyControl();

			if (property == null) return;

			property.PropertyChanged += PropertyControlValueChanged;
			property.AttachTo(SelectedPrimitive);
			AddProperties(property);
			UpdatePropertiesView();
		}

		public void RemoveAllProperties()
		{
			var ctrl = FindPorpertyControl();
			if (ctrl != null)
				Controls.Remove(ctrl);
		}

		public void AddProperties(PropertyControlBase pb)
		{
			if (pb == null)
				return;
			Controls.Add(pb);
			UpdatePropertyControlPosition();
			pb.BringToFront();
		}

		public void UpdatePropertiesView()
		{
			Invalidate();
		}

		public void RemoveSelection()
		{
			if (SelectionExists)
				SelectedPrimitive.ShapeToolLost();
		}

		internal string CreateTemporaryPicture()
		{
			var fileName = Path.GetTempFileName();
			var bmp = RenderToBitmap();
			if (bmp != null)
			{
				var wrapper = new BitmapWrapper(bmp);
				wrapper.Save(fileName, "image/jpeg", 100);
				return fileName;
			}

			return null;
		}

		protected RectangleF CalculateGlobalBounds()
		{
			return CalculateGlobalBounds(true);
		}


		protected RectangleF CalculateGlobalBounds(bool inflate)
		{
			var rect = new RectangleF(0, 0, 0, 0);
			if (_allPainting.Count > 0)
			{
				rect = _allPainting[0].BoundRect;
				for (int i = 1; i < _allPainting.Count; i++)
					rect = RectangleF.Union(rect, _allPainting[i].BoundRect);
			}
			if (rect.Width > 0 && rect.Height > 0 && inflate)
				rect.Inflate(20, 20);
			return rect;
		}

		protected PointF MouseToReal(Point mousePos)
		{
			return MouseToReal(new PointF(mousePos.X, mousePos.Y));
		}

		protected PointF MouseToReal(PointF mousePos)
		{
			PointF p = PictureOffset;
			return new PointF((mousePos.X/_zoom - p.X/_zoom) - AutoScrollPosition.X/_zoom,
			                  (mousePos.Y/_zoom - p.Y/_zoom) - AutoScrollPosition.Y/_zoom);
		}

		private Primitive GetPrimitiveAt(PointF pt)
		{
			for (int i = _allPainting.Count - 1; i >= 0; i--)
			{
				if (_allPainting[i].isPointIn(pt))
					return _allPainting[i];
			}
			return null;
		}

		private void Normalization()
		{
			RectangleF rc = CalculateGlobalBounds();
			_offsetX = (int) (rc.Left*_zoom) - (Width - rc.Width)/2;
			_offsetY = (int) (rc.Top*_zoom) - (Height - rc.Height)/2;
		}

		private void ResetSelectedPrimitive()
		{
			SelectedPrimitive = null;
		}

		private static TpTool PrimitiveTool(Primitive primitive)
		{
			if (primitive is FreeLine)
				return TpTool.FreeLine;
			if (primitive is Line)
				return TpTool.Line;
			if (primitive is DecoratedTextBox)
				return TpTool.TextBox;
			if (primitive is Box)
				return TpTool.Box;

			return TpTool.Select;
		}

		private void UpdatePropertyControlPosition()
		{
			var propControl = FindPorpertyControl();
			if (propControl != null)
			{
				propControl.Location = new Point(10, Height - 80);
				propControl.Size = new Size(Width - 20, 60);
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{			
			base.OnLayout(levent);			
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{			
			base.OnInvalidated(e);
		}
		private PropertyControlBase FindPorpertyControl()
		{
			return Controls.OfType<PropertyControlBase>().FirstOrDefault();
		}

		#endregion

		#region Events

		// Do not remove this commented block. Written due research. More correct relocate behavior while zoom //Yury
		//if (e.Graphics.VisibleClipBounds.Width > canvas.X + canvas.Width && canvas.X < 0 && canvas.Width < e.Graphics.VisibleClipBounds.Width)
		//    canvas.X = 0;
		//if (e.Graphics.VisibleClipBounds.Width < canvas.X + canvas.Width && canvas.Width < e.Graphics.VisibleClipBounds.Width)
		//    canvas.X = (int)(e.Graphics.VisibleClipBounds.Width - canvas.Width);
		//if (e.Graphics.VisibleClipBounds.Height > canvas.Y + canvas.Height && canvas.Y < 0 && canvas.Height < e.Graphics.VisibleClipBounds.Height)
		//    canvas.Y = 0;
		//if (e.Graphics.VisibleClipBounds.Height < canvas.Y + canvas.Height && canvas.Height < e.Graphics.VisibleClipBounds.Height)
		//    canvas.Y = (int)(e.Graphics.VisibleClipBounds.Height - canvas.Height);

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			//e.Graphics.DrawString(string.Format("x:{0} y:{1}", _offsetX, _offsetY), new Font("Arial", 12), new SolidBrush(Color.White), 300, 300);
			//e.Graphics.DrawString(string.Format("X:{0} Y:{1} Width:{2} Height:{3}", _scrollAreaWidth, _scrollAreaHeight, AutoScrollPosition.X, AutoScrollPosition.Y), new Font("Arial", 12), new SolidBrush(Color.White), 40, 40);
			//e.Graphics.DrawString(string.Format("x:{0} y:{1}", MousePosition.X, MousePosition.Y), new Font("Arial", 12), new SolidBrush(Color.White), 40, 40);

			RectangleF canvas = GetCanvas();
			AutoScrollMinSize = new Size((int)(canvas.Width), (int)(canvas.Height));
			UpdateBounds();
			AutoScrollMargin = new Size();
			if (canvas.Width <= ClientSize.Width && canvas.Height <= ClientSize.Height)
			{
				AutoScrollPosition = new Point(0, 0);
				AutoScroll = false;
				HorizontalScroll.Visible = false;
				VerticalScroll.Visible = false;
				HScroll = false;
				VScroll = false;
			}
			else
			{
				AutoScroll = true;
			}
			
			e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
			canvas = GetCanvas();
			
			_currentDelta = GetDeltaForCanvas(canvas);
			PointF offset = PictureOffset;

			canvas.Offset(offset);

			var delta = _currentDelta;

			using (Brush br = new SolidBrush(CustomBackColor))
			{
				e.Graphics.FillRectangle(br, new RectangleF(canvas.X + delta.X, canvas.Y + delta.Y, canvas.Width, canvas.Height));
			}
			
			foreach (var primitive in _allPainting)
			{
				DrawPrimitive(primitive, e.Graphics, _zoom, new PointF(offset.X + delta.X, offset.Y + delta.Y));
			}

			if (_currentPrimitive != null)
			{
				DrawPrimitive(_currentPrimitive, e.Graphics, _zoom, new PointF(offset.X + delta.X, offset.Y + delta.Y));
			}

			if (_shapeControl.Primitive != null)
			{
				_shapeControl.Primitive = _shapeControl.Primitive;
				_shapeControl.Draw(e.Graphics, new PointF(offset.X + delta.X, offset.Y + delta.Y), _zoom);
			}

			var property = FindPorpertyControl();
			if (property != null)
			{
				property.Zoom = (int) (_zoom*100);
			}

			NormalizeEx();
			
			UpdatePropertyControlPosition();
			
		}

		private Size GetRealClientArea(Point location)
		{
			int width = ClientSize.Width;
			int height = ClientSize.Height;

			if (location.X < 0)
				width = Math.Abs(location.X) + ClientSize.Width;
			if (location.Y < 0)
				height = Math.Abs(location.Y) + ClientSize.Height;

			return new Size(width, height);
		}

		private PointF GetDeltaForCanvas(RectangleF boundRect)
		{
			return GetDelta(boundRect, new PointF(0, 0));
		}

		private PointF GetDeltaForPrimitive(RectangleF boundRect)
		{
			return GetDelta(boundRect, new PointF(20, 20));
		}

		private PointF GetDelta(RectangleF boundRect, PointF canvasOffset)
		{
			float deltaX = boundRect.X < canvasOffset.X ? canvasOffset.X - boundRect.X : 0;
			float deltaY = boundRect.Y < canvasOffset.Y ? canvasOffset.Y - boundRect.Y : 0;

			deltaX = boundRect.Width + boundRect.X > ClientSize.Width - canvasOffset.X
			         	? ClientSize.Width - canvasOffset.X - boundRect.Width - boundRect.X
			         	: deltaX;

			deltaY = boundRect.Height + boundRect.Y > ClientSize.Height - canvasOffset.Y
			         	? ClientSize.Height - canvasOffset.Y - boundRect.Height - boundRect.Y
			         	: deltaY;

			deltaX = boundRect.Width > ClientSize.Width ? -boundRect.X : deltaX;
			deltaY = boundRect.Height > ClientSize.Height ? -boundRect.Y : deltaY;


			return new PointF(deltaX, deltaY);
		}

		private void CalculateScrollPostions()
		{
		}

		private RectangleF GetCanvas()
		{
			RectangleF rect = CalculateGlobalBounds();

			return new RectangleF((rect.X*_zoom), ((rect.Y + 0)*_zoom), (rect.Width*_zoom),
			                      (rect.Height*_zoom));
		}

		public void PropertyControlValueChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Primitive overPrimitive = GetPrimitiveAt(MouseToReal(e.Location));
				if (overPrimitive != null)
				{
					SelectedPrimitive = overPrimitive;
					_shapeControl.Dragging = true;
					UpdateCurrentTool();
					Invalidate();
					overPrimitive.MouseDoubleClickHandler(MouseToReal(e.Location));
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (_resizeMode)
				{
					_resizeMode = false;
					if(SelectionExists)
						SelectedPrimitive.OnResize(true);
				}

				_dragPointID = -1;

				_isPenDown = false;
				_shapeControl.Dragging = false;
				if (SelectionExists)
				{
					SelectedPrimitive.MoveBy(-(int) (_currentDelta.X/_zoom), -(int) (_currentDelta.Y/_zoom));
					_currentDelta = new Point();
				}
				if (_currentPrimitive != null)
				{
					_currentPrimitive.CreationMouseUpHandler(MouseToReal(e.Location));

					if (_currentPrimitive.isValid())
					{
						_allPainting.Add(_currentPrimitive);
						_redoStack.Clear();
						SelectedPrimitive = _currentPrimitive;
						UpdateCurrentTool();
					}

					_currentPrimitive = null;

					Invalidate();
				}
				CalculateScrollPostions();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if(!HorizontalScroll.Visible && !VerticalScroll.Visible) return;

			ClearSelection();
			base.OnMouseWheel(e);
			Invalidate();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey && GetPrimitiveAt(MouseToReal(PointToClient(Cursor.Position))) != null)
				Cursor = Cursors.Hand;
			base.OnKeyDown(e);
		}

		protected override void OnResize(EventArgs e)
		{
			if(SelectionExists)
			{
				SelectedPrimitive.OnFormResize();
			}

			_navigationControl.Location = new Point(Width - _navigationControl.Width - 10,
			                                        Height - _navigationControl.Height - 10);
			base.OnResize(e);
			NormalizeEx();
			UpdatePropertyControlPosition();
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			pevent.Graphics.Clear(OffCanvasColor);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{			
			Primitive overPrimitive = GetPrimitiveAt(MouseToReal(e.Location));

			Cursor cursor = _shapeControl.GetCursor(MouseToReal(e.Location), _dragPointID);
			if (cursor != null)
			{
				Cursor = cursor;
			}
			else if (_shapeControl != null && _shapeControl.PointIn(MouseToReal(e.Location)))
			{
				if (SelectionExists)
					Cursor = SelectedPrimitive.GetCursor(MouseToReal(e.Location));
			}
			else if (overPrimitive != null && ModifierKeys == Keys.Shift)
			{
				Cursor = Cursors.Hand;
			}
			else
			{
				Cursor = Cursors.Default;
			}

			if (e.Button == MouseButtons.Left)
			{
				if (_dragPointID >= 0 && SelectionExists)
				{
					_shapeControl.MovePoint(_dragPointID, MouseToReal(e.Location));
					SelectedPrimitive.OnResize(false);
					_resizeMode = true;
					Invalidate();
				}
				else if (_shapeControl.Dragging && SelectionExists)
				{
					float dx = ((_lastMousePosition.X - e.X)/_zoom);
					float dy = ((_lastMousePosition.Y - e.Y)/_zoom);
					
					_shapeControl.MoveBy(dx, dy);
					Invalidate();
				}
				else if (_isPenDown && _currentPrimitive != null)
				{
					_currentPrimitive.CreationMouseMoveHandler(MouseToReal(e.Location));
					Invalidate();
				}
			}
			if (e.Button == MouseButtons.Right)
			{
				float dx = _lastMousePosition.X - e.X;
				float dy = _lastMousePosition.Y - e.Y;
				PointF tpt = new PointF(_offsetX + dx, _offsetY + dy);
				RectangleF rc = CalculateGlobalBounds();
				rc.Offset(-Width, -Height);
				rc.Size = new SizeF(rc.Width + Width*2, rc.Height + Height*2);
				if (rc.Contains(tpt))
				{
					_offsetX += dx;
					_offsetY += dy;
					Invalidate();
				}
			}

			//if (SelectionExists)
			//    NormalizeEx();
			_lastMousePosition = e.Location;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			_dragPointID = _shapeControl.GetPoint(MouseToReal(e.Location));
			if (_dragPointID >= 0)
			{
				return;
			}

			Primitive overPrimitive = GetPrimitiveAt(MouseToReal(e.Location));
			if (overPrimitive != null &&
			    (ModifierKeys == Keys.Shift ||
			     (SelectedPrimitive == overPrimitive && _selectedTool == PrimitiveTool(overPrimitive)) ||
			     _selectedTool == TpTool.Select))
			{
				if (SelectionExists)
					SelectedPrimitive.ShapeToolLost();
				SelectedPrimitive = overPrimitive;
				_shapeControl.Dragging = true;
				UpdateCurrentTool();
				Invalidate();
			}
			else
			{
				if (ModifierKeys == Keys.Shift)
					return;
				if (SelectionExists)
				{
					SelectedPrimitive.ShapeToolLost();
					ResetSelectedPrimitive();
					Invalidate();
				}
				_isPenDown = true;
				if (_selectedTool == TpTool.FreeLine)
				{
					_currentPrimitive = new FreeLine();
				}
				else if (_selectedTool == TpTool.Line)
				{
					_currentPrimitive = new Line();
				}
				else if (_selectedTool == TpTool.TextBox)
				{
					_currentPrimitive = new DecoratedTextBox();
				}
				else if (_selectedTool == TpTool.Box)
				{
					_currentPrimitive = new Box();
				}
				else if (_selectedTool == TpTool.Select)
				{
					_currentPrimitive = null;
				}

				if (_currentPrimitive != null)
					_currentPrimitive.CreationMouseDownHandler(MouseToReal(e.Location));
			}

			if (!SelectionExists)
				RemoveAllProperties();
		}

		#endregion

		public void ZoomIn()
		{
			ZoomIn(new Point());
		}

		public void ZoomOut()
		{
			ZoomOut(new Point());
		}
	}
}