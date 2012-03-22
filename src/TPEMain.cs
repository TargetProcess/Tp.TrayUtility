using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TpTrayUtility.Components;
using TpTrayUtility.Properties;

namespace TpTrayUtility
{
	public partial class TPEMain : Form
	{
		#region Inner entities

		public class FakePrimitive : Primitive
		{
			#region Fields

			private RectangleF currentRectangle = new Rectangle(0, 0, 0, 0);

			#endregion

			#region Primitive implementation

			public override bool isPointIn(PointF point)
			{
				return currentRectangle.Contains(point);
			}

			public override void Draw(Graphics g, float zoom, PointF offsetX, PointF offsetY)
			{
			}

			public override void SetColor(Color fore, Color back)
			{
			}

			public override void CreationMouseDownHandler(PointF point)
			{
				currentRectangle.Location = point;
			}

			public override void CreationMouseUpHandler(PointF point)
			{
				currentRectangle.Size = new SizeF(point.X - currentRectangle.Location.X, point.Y - currentRectangle.Location.Y);
			}

			public void EditFinishing(object sender, EventArgs e)
			{
			}

			public override void ShapeToolLost()
			{
			}

			public override void CreationMouseMoveHandler(PointF point)
			{
				currentRectangle.Size = new SizeF(point.X - currentRectangle.X, point.Y - currentRectangle.Y);
			}

			public override void MouseDoubleClickHandler(PointF point)
			{
			}

			public override Cursor GetCursor(PointF point)
			{
				return Cursors.SizeAll;
			}

			public override RectangleF BoundRect
			{
				get { return currentRectangle; }
				set { currentRectangle = Helpers.NormalizedRect(value.Location, new PointF(value.Right, value.Bottom)); }
			}

			public override bool isResizeble()
			{
				return true;
			}

			public override void MoveBy(float dx, float dy)
			{
				currentRectangle.Offset(-dx, -dy);
			}

			public override bool isValid()
			{
				return currentRectangle.Width > 5 && currentRectangle.Height > 5;
			}

			public override PropertyControlBase GeneratePropertyControl()
			{
				return null;
			}

			public override void OnResize(bool lastMove)
			{
			}

			public override bool CanUndo()
			{
				return false;
			}

			public override void UpdateSpecialPoint(int id, PointF position)
			{
			}

			public override List<ShapeBoxControlPoint> SpecialPoints()
			{
				return new List<ShapeBoxControlPoint>();
			}

			public override void UpdatePropertyFromStaticData()
			{
			}

			#endregion
		}

		public class DesktopCopy
		{
			#region Fields

			public Bitmap bmpScreenshot;
			public Bitmap bmpScreenshotCopy;

			#endregion

			#region Methods

			public void DoCopy(Screen scr)
			{
				bmpScreenshot = new Bitmap(scr.Bounds.Width, scr.Bounds.Height, PixelFormat.Format32bppArgb);
				bmpScreenshotCopy = new Bitmap(scr.Bounds.Width, scr.Bounds.Height, PixelFormat.Format32bppArgb);
				Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
				Graphics gfxScreenshotCopy = Graphics.FromImage(bmpScreenshotCopy);
				gfxScreenshot.CopyFromScreen(scr.Bounds.X, scr.Bounds.Y, 0, 0, scr.Bounds.Size, CopyPixelOperation.SourceCopy);
				using (Brush br = new SolidBrush(Color.FromArgb(64, 0, 0, 0)))
				{
					gfxScreenshot.FillRectangle(br, 0, 0, bmpScreenshot.Width, bmpScreenshot.Height);
				}
				gfxScreenshotCopy.CopyFromScreen(scr.Bounds.X, scr.Bounds.Y, 0, 0, scr.Bounds.Size, CopyPixelOperation.SourceCopy);
			}

			#endregion
		}

		#endregion

		#region Fields

		private DesktopCopy[] _screenCopy;

		private bool _isCapture;

		public static HotKey _bugHotKey;
		public static HotKey _dashboardHotKey;

		private ShapeBoxControl _shapeControl;
		private FakePrimitive _primitive;

		private readonly Magnify2 _magnify2;

		private int _globalScreenOffset;

		private int _dragPointID = -1;
		private Point _lastMousePosition;

		private readonly Timer _iconBlinkTimer = new Timer();
		private bool _iconState;
		private readonly Point _defaultLocation = new Point(-32000, -32000);

		#endregion

		#region Constructor

		public TPEMain()
		{
			InitializeComponent();

			_isCapture = false;
			DoubleBuffered = true;

			_bugHotKey = new HotKey(Handle, 100, (Keys) SettingsManager.HotKeyCaptureBugValue,
									SettingsManager.HotKeyCaptureBugModifier, AddBugPressed);
			_dashboardHotKey = new HotKey(Handle, 101, (Keys) SettingsManager.HotKeyDashboardValue,
									SettingsManager.HotKeyDashboardModifier, toDo_Click);

			_magnify2 = new Magnify2();
			Controls.Add(_magnify2);

			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			ResetScreens();
			
			//In this way black window will not be visible (in a few moments) in the start of the first capturing
			Location = _defaultLocation;
			WindowState = FormWindowState.Minimized;
		}
		
		#endregion

		#region Methods

		public void ResetScreens()
		{
			_primitive = new FakePrimitive();
			_shapeControl = new ShapeBoxControl(_primitive);

			_screenCopy = new DesktopCopy[Screen.AllScreens.Length];
			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				_screenCopy[i] = new DesktopCopy();
				_screenCopy[i].DoCopy(Screen.AllScreens[i]);
			}
		}

		public void CaptureToLib()
		{
			if (_shapeControl.Primitive.BoundRect.Width > 0 && _shapeControl.Primitive.BoundRect.Height > 0)
			{
				PreparePictrue();
				FormsManager.HideForms();
			}
		}

		public void SwitchToEditor()
		{
			if (_shapeControl.Primitive.BoundRect.Width > 0 && _shapeControl.Primitive.BoundRect.Height > 0)
			{
				Bitmap bmpResult = PreparePictrue();
				FormsManager.GetInstance<SimpleEditor>().Setup(bmpResult);
				FormsManager.GetInstance<SimpleEditor>().ShowEditor();
			}
		}

		public void StartNewMessageBlinking()
		{
			if (_iconBlinkTimer.Enabled)
				return;

			_iconBlinkTimer.Interval = 1000;
			_iconBlinkTimer.Tick += iconBlinkTimer_Tick;
			_iconBlinkTimer.Start();
		}

		public void StopNewMessageBlinking()
		{
			_iconBlinkTimer.Stop();
			notifyIcon1.Icon = Resources.tp;
		}

		public void AddBug()
		{
			//Without this location will not be set in some specified cases (Bug#28542)
			//And fix for Bug#29941
			AdditionalInitForAddBug();

			AddBugCommon();
		}

		private void AdditionalInitForAddBug()
		{
			FormsManager.HideForms();
			ResetScreens();
			Show();
		}

		private void AddBugCommon()
		{
			WindowState = FormWindowState.Normal;
			
			FormsManager.HideForms();
			
//			Update();
//			Invalidate();
			ResetScreens();

			Location = Screen.AllScreens[0].Bounds.Location;
			ClientSize = Screen.AllScreens[0].Bounds.Size;

			for (int i = 1; i < Screen.AllScreens.Length; i++)
			{
				if (Location.X >= Screen.AllScreens[i].Bounds.X)
				{
					Location = Screen.AllScreens[i].Bounds.Location;
				}

				ClientSize = new Size(ClientSize.Width + Screen.AllScreens[i].Bounds.Width,
				                      Math.Max(ClientSize.Height, Screen.AllScreens[i].Bounds.Height));
			}

			_globalScreenOffset = Location.X;

			if (_magnify2.Location.IsEmpty)
			{
				_magnify2.Location = new Point(ClientSize.Width - _magnify2.Size.Width, ClientSize.Height - _magnify2.Size.Height);
			}

			TopMost = true;
			Show();
		}

		private Bitmap PreparePictrue()
		{
			var bmpResult = new Bitmap((int)_shapeControl.Primitive.BoundRect.Width, (int)_shapeControl.Primitive.BoundRect.Height,
			                           PixelFormat.Format32bppArgb);
			var gfxResult = Graphics.FromImage(bmpResult);


			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				RectangleF actualScreen = Screen.AllScreens[i].Bounds;
				actualScreen.Offset(-_globalScreenOffset, 0);
				RectangleF interSection = RectangleF.Intersect(actualScreen, _shapeControl.Primitive.BoundRect);
				if (!interSection.IsEmpty)
				{
					RectangleF rc = interSection;
					rc.Offset(_globalScreenOffset - Screen.AllScreens[i].Bounds.X, 0);
					interSection.Offset(-_shapeControl.Primitive.BoundRect.X, -_shapeControl.Primitive.BoundRect.Y);
					gfxResult.DrawImage(_screenCopy[i].bmpScreenshotCopy, interSection, rc, GraphicsUnit.Pixel);
				}
			}

			string fileName;
			do
			{
				DateTime dtnow = DateTime.Now;
				fileName = String.Format("{0}//sc_{1}_{2}_{3}_{4}.bmp", ImageLibrary.DefaultPath,
				                         Regex.Replace(dtnow.ToShortDateString(), @"[^\w\@-]", ""), dtnow.Hour, dtnow.Minute,
				                         dtnow.Second);
			} while (File.Exists(fileName));

			var fileStream = new FileStream(fileName, FileMode.Create);
			bmpResult.Save(fileStream, ImageFormat.Bmp);
			fileStream.Close();

			return bmpResult;
		}

		#endregion

		#region Events

		protected override void OnActivated(EventArgs e)
		{
			TopMost = true;
			base.OnActivated(e);
		}

		protected override void OnDeactivate(EventArgs e)
		{
			TopMost = false;
			base.OnDeactivate(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				if (_screenCopy[i].bmpScreenshot != null)
				{
					e.Graphics.DrawImageUnscaled(_screenCopy[i].bmpScreenshot,
					                             Screen.AllScreens[i].Bounds.Location.X - _globalScreenOffset,
					                             Screen.AllScreens[i].Bounds.Location.Y);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				RectangleF actualScreen = Screen.AllScreens[i].Bounds;
				actualScreen.Offset(-_globalScreenOffset, 0);
				RectangleF interSection = RectangleF.Intersect(actualScreen,
				                                             Helpers.NormalizedRect(_shapeControl.Primitive.BoundRect.Location,
				                                                                    new PointF(
				                                                                    	_shapeControl.Primitive.BoundRect.Right,
				                                                                    	_shapeControl.Primitive.BoundRect.Bottom)));
				if (!interSection.IsEmpty)
				{
					RectangleF rc = interSection;
					rc.Offset(_globalScreenOffset - Screen.AllScreens[i].Bounds.X, 0);
					e.Graphics.DrawImage(_screenCopy[i].bmpScreenshotCopy, interSection, rc, GraphicsUnit.Pixel);
				}
			}

			_shapeControl.DrawPointsOnly(e.Graphics);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				SwitchToEditor();
			}
			base.OnMouseDoubleClick(e);
		}

		private void AddBugPressed(object sender, EventArgs e)
		{
			AddBug();
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == WindowState)
			{
				notifyIcon1.Visible = true;
				Hide();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void MainMenu_ExitClick(object sender, EventArgs e)
		{
			Close();
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			Cursor cursor = _shapeControl.GetCursor(e.Location, _dragPointID);
			if (cursor != null)
			{
				Cursor = cursor;
			}
			else if (_shapeControl.Primitive.isPointIn(e.Location))
			{
				Cursor = Cursors.SizeAll;
			}
			else
			{
				Cursor = Cursors.Cross;
			}

			if (e.Button == MouseButtons.Left)
			{
				if (_isCapture)
				{
					int dx = ((_lastMousePosition.X - e.X));
					int dy = ((_lastMousePosition.Y - e.Y));
					_shapeControl.MoveBy(dx, dy);
				}
				else
				{
					if (_dragPointID >= 0)
					{
						_shapeControl.MovePoint(_dragPointID, e.Location);
						_shapeControl.Primitive.OnResize(false);
					}
					else
					{
						_primitive.CreationMouseUpHandler(e.Location);
						_shapeControl.Primitive = _primitive;
					}
				}

				Invalidate();
				Update();
			}
			_lastMousePosition = e.Location;

			_magnify2.UpdatePicture(null, e.Location.X + _globalScreenOffset, e.Location.Y, (int)_primitive.BoundRect.Width,
			                       (int)_primitive.BoundRect.Height);
			_magnify2.Invalidate();
			_magnify2.Update();
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			_dragPointID = _shapeControl.GetPoint(e.Location);
			if (_dragPointID >= 0)
				return;
			if (_shapeControl.Primitive.isPointIn(e.Location))
			{
				_isCapture = true;
			}
			else
			{
				_primitive.CreationMouseDownHandler(e.Location);
			}
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;

			if (_isCapture)
			{
				_isCapture = false;
			}
			else if (_dragPointID == -1)
			{
				_primitive.CreationMouseUpHandler(e.Location);
				_primitive.BoundRect = _primitive.BoundRect;
			}
			else
			{
				_dragPointID = -1;
			}
			_shapeControl.Primitive = _primitive;

			Invalidate();
			Update();
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 || e.KeyChar == ' ')
			{
				SwitchToEditor();
			}

			if (e.KeyChar == 27)
			{
				FormsManager.HideForms();
			}
		}

		private void toolStripTextBox1_Click(object sender, EventArgs e)
		{
			AddBug();
		}

		private void toolStripTextBox2_Click(object sender, EventArgs e)
		{
			FormsManager.GetInstance<SimpleEditor>().ShowEditor();
		}

		private void toolStripMenuItemSetting_Click(object sender, EventArgs e)
		{
			FormsManager.Show(typeof (SecurityForm), false, true);
		}

		private void toolStripMenuAbout_Click(object sender, EventArgs e)
		{
			FormsManager.Show(typeof (AboutTPUBox), false, true);
		}

		private void toolStripMenuItemAddBug_Click(object sender, EventArgs e)
		{
			FormsManager.Show(typeof (SubmitBugForm), true, true);
		}

		private void toDo_Click(object sender, EventArgs e)
		{
			FormsManager.Show(typeof (frmDashboard), false, true);
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			FormsManager.Show(typeof (RedefineHotKeys), false, true);
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			FormsManager.GetInstance<SimpleEditor>().ShowEditor();
		}

		private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void iconBlinkTimer_Tick(object sender, EventArgs e)
		{
			var resources = new ComponentResourceManager(typeof (TPEMain));
			notifyIcon1.Icon = _iconState ? Resources.IconMessage : Resources.tp;

			_iconState = !_iconState;
		}

		#endregion

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
		{
			AssignmentsManager.Instance.ReadNextMessage();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			contextMenuStrip1.Enabled = Enabled;
		}

		public void SetEnableNotifyIcon(bool isEnable)
		{
			if (isEnable)
				notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
			else
				notifyIcon1.MouseDoubleClick -= notifyIcon1_MouseDoubleClick;
		}
	}
}