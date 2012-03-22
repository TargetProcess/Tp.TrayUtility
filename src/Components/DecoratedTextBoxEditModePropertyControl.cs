// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Windows.Forms;
using TpTrayUtility.Controls;
using TpTrayUtility.Properties;

namespace TpTrayUtility.Components
{
	public class DecoratedTextBoxEditModePropertyControl : PropertyControlBase
	{
		#region Fields

		private DecoratedTextBox _attachedBox;

		private readonly FontFamilyCollection _fontFamilies;

		private readonly TpComboBox _fontFamilyDd = new TpComboBox();
		private readonly TpComboBox _fontSizeDd = new TpComboBox();
		private readonly ChangeColorControl _colorControl;

		private readonly TogleButton _togleBold = new TogleButton(Resources.b_normal, Resources.b_hover, Resources.b_press);
		private readonly TogleButton _togleItalic = new TogleButton(Resources.i_normal, Resources.i_hover, Resources.i_press);
		private readonly TogleButton _togleUnderline = new TogleButton(Resources.u_normal, Resources.u_hover, Resources.u_press);

		private readonly TogleButton _togleHorizontalLeft = new TogleButton(Resources.tl_normal, Resources.tl_hover,
																		   Resources.tl_press);

		private readonly TogleButton _togleHorizontalCenter = new TogleButton(Resources.tc_normal, Resources.tc_hover,
																			 Resources.tc_press);

		private readonly TogleButton _togleHorizontalRight = new TogleButton(Resources.tr_normal, Resources.tr_hover,
																			Resources.tr_press);

		private string _fontName = "Arial";
		private float _fontSize = 8;
		private Color _fontColor = Color.Black;

		private Font _lastSelectionFont;
		private Color _lastSelectionColor = Color.Black;
		private HorizontalAlignment _lastSelectionAlignment = HorizontalAlignment.Left;

		private SelectedAlignmentDictionary _alignmentDictionary;

		private SelectedFontStyleDictionary _fontStyleDictionary;

		#endregion

		#region Properties

		private bool HasAttachedBox
		{
			get { return _attachedBox != null; }
		}

		private Color SelectedColor
		{
			get { return _colorControl.Color; }
			set
			{
				_colorControl.Color = value;
				_colorControl.Invalidate();
			}
		}

		private string SelectedFontFamilyName
		{
			get { return _fontFamilyDd.SelectedItemText; }
		}

		private FontFamily SelectedFontFamily
		{
			get { return _fontFamilies.FindByName(SelectedFontFamilyName); }
		}

		#endregion

		#region Constructor

		public DecoratedTextBoxEditModePropertyControl()
		{
			_fontFamilies = new FontFamilyCollection();

			_colorControl = AddColorControl();

			BindFontFamilyDropDown();

			BindFotnSizeDropDown();

			BindAlignmentDictionary();

			BindStyleDictionary();

			FillControl(true);

			InitEvents();
		}

		private void BindAlignmentDictionary()
		{
			_alignmentDictionary = new SelectedAlignmentDictionary
			                      	{
			                      		{HorizontalAlignment.Center, _togleHorizontalCenter},
										{HorizontalAlignment.Left, _togleHorizontalLeft},
										{HorizontalAlignment.Right, _togleHorizontalRight}
			                      	};
		}

		private void BindStyleDictionary()
		{
			_fontStyleDictionary = new SelectedFontStyleDictionary
			                   	{
			                  		{FontStyle.Bold, _togleBold},
									{FontStyle.Italic, _togleItalic},
									{FontStyle.Underline, _togleUnderline},
			                  	};
		}

		#endregion

		#region Methods

		public override void AttachTo(Primitive primitive)
		{
			if (primitive == null)
			{
				_attachedBox = null;
			}
			else
			{
				_attachedBox = primitive as DecoratedTextBox;
				_attachedBox.tpText.SelectionChanged += tpText_SelectionChanged;
			}
			FillControl(true);
			Invalidate();
		}

		private void FillControl(bool redrawControls)
		{
			if (HasAttachedBox)
			{
				InitFontDropDowns();

				InitColorControl();

				InitTextAIligmentToggles();

				_attachedBox.tpText.HideSelection = false;
			}

			if(!redrawControls) return;

			Reset();
			AddControls();
		}

		private void InitColorControl()
		{
			_fontColor = _attachedBox.tpText.SelectionColor;
			SelectedColor = _fontColor;
		}

		private void InitTextAIligmentToggles()
		{
			_togleHorizontalLeft.Togled = _attachedBox.tpText.SelectionAlignment == HorizontalAlignment.Left;
			_togleHorizontalCenter.Togled = _attachedBox.tpText.SelectionAlignment == HorizontalAlignment.Center;
			_togleHorizontalRight.Togled = _attachedBox.tpText.SelectionAlignment == HorizontalAlignment.Right;
		}

		private void InitFontDropDowns()
		{
			if (_attachedBox.tpText.HasDifferentFonts) return;

			_fontName = _attachedBox.tpText.SelectionFont.FontFamily.Name;
			string size = ((int)_attachedBox.tpText.SelectionFont.Size).ToString();
			float.TryParse(size, out _fontSize);

			_fontFamilyDd.SelectItem(_fontName);
			_fontSizeDd.SelectItem(size);
		}

		private void AddControls()
		{
			_fontFamilyDd.Size = new Size(220,23);
			AddControl(_fontFamilyDd, 250);
			_fontSizeDd.Size = new Size(43, 23);
			AddControl(_fontSizeDd, 70);
			AddControl(_colorControl, 50);

			if (_fontFamilyDd.SelectedItem != null)
			{
				var fontFamily = SelectedFontFamily;

				foreach (var pair in _fontStyleDictionary)
				{
					AddFontStyleControlIfAllowed(pair.Value, fontFamily, pair.Key);
				}

				AddControl(new Control(), 10);
			}

			AddControl(_togleHorizontalLeft, 20);
			AddControl(_togleHorizontalCenter, 20);
			AddControl(_togleHorizontalRight, 25);
		}

		private void AddFontStyleControlIfAllowed(TogleButton toggle, FontFamily font, FontStyle style)
		{
			_fontStyleDictionary.ApplyEnabled(font);

			AddControl(toggle, 20);
			if (HasAttachedBox && !_attachedBox.tpText.HasDifferentFonts)
			{
				toggle.Togled = (_attachedBox.tpText.SelectionFont.Style & style) == style;
			}
		}

		private void BindFotnSizeDropDown()
		{
			_fontSizeDd.AddItem("8");
			_fontSizeDd.AddItem("9");
			_fontSizeDd.AddItem("10");
			_fontSizeDd.AddItem("11");
			_fontSizeDd.AddItem("12");
			_fontSizeDd.AddItem("14");
			_fontSizeDd.AddItem("16");
			_fontSizeDd.AddItem("18");
			_fontSizeDd.AddItem("20");
			_fontSizeDd.AddItem("22");
			_fontSizeDd.AddItem("24");
			_fontSizeDd.AddItem("26");
			_fontSizeDd.AddItem("28");
			_fontSizeDd.AddItem("36");
			_fontSizeDd.AddItem("48");
			_fontSizeDd.AddItem("72");
		}

		private void InitEvents()
		{
			_colorControl.ColorChanged += colorControl_ValueChanged;

			_fontFamilyDd.SelectedIndexChanged += fontFamilyDD_ValueChanged;
			_fontSizeDd.SelectedIndexChanged += fontSizeDD_ValueChanged;

			_togleBold.Click += togle_StyleClick;
			_togleItalic.Click += togle_StyleClick;
			_togleUnderline.Click += togle_StyleClick;

			_togleHorizontalLeft.Click += togleHorizontalLeft_Click;
			_togleHorizontalCenter.Click += togleHorizontalCenter_Click;
			_togleHorizontalRight.Click += togleHorizontalRight_Click;
		}

		private void BindFontFamilyDropDown()
		{
			foreach (FontFamily t in _fontFamilies)
			{
				_fontFamilyDd.AddItem(t.Name);
			}

			_fontFamilyDd.SelectItem(_fontFamilies[0].Name);
		}

		private float GetSelectedFontSize()
		{
			return float.Parse(_fontSizeDd.SelectedItemText);
		}

		private void ApplyAlignmentsToggle(HorizontalAlignment alignment)
		{
			_alignmentDictionary.ApplyToggling(alignment);

			_attachedBox.tpText.SelectionAlignment = alignment;
			
			Invalidate();
		}

		private void ApplyStylesToggling()
		{
			_fontStyleDictionary.ApplyToggling(_attachedBox.tpText.SelectionFont);
		
		}

		#endregion

		#region Events

		private void togleHorizontalLeft_Click(object sender, EventArgs e)
		{
			ApplyAlignmentsToggle(HorizontalAlignment.Left);
		}

		private void togleHorizontalCenter_Click(object sender, EventArgs e)
		{
			ApplyAlignmentsToggle(HorizontalAlignment.Center);
		}

		private void togleHorizontalRight_Click(object sender, EventArgs e)
		{
			ApplyAlignmentsToggle(HorizontalAlignment.Right);
		}

		private void togle_StyleClick(object sender, EventArgs e)
		{
			var style = _fontStyleDictionary.GetStyle();

			if (!_attachedBox.tpText.HasDifferentFonts && !_attachedBox.tpText.SelectionFont.FontFamily.IsStyleAvailable(style))
			{
				Messenger.ShowWarning(string.Format("Font '{0}' does not support style '{1}'.", SelectedFontFamily.Name, style));
			}
			else
			{
				_attachedBox.tpText.SetFontToSelection(style);
			}

			
			ApplyStylesToggling();

			Invalidate();
		}

		private void colorControl_ValueChanged(object sender, EventArgs e)
		{
			if (!HasAttachedBox) return;

			_fontColor = SelectedColor;
			_attachedBox.tpText.SelectionColor = _fontColor;
		}

		private void fontSizeDD_ValueChanged(object sender, EventArgs e)
		{
			if (!HasAttachedBox) return;

			_fontSize = GetSelectedFontSize();

			_attachedBox.tpText.SetFontToSelection(_fontSize);

			Invalidate();
		}

		private void fontFamilyDD_ValueChanged(object sender, EventArgs e)
		{
			if (!HasAttachedBox) return;

			_fontName = _fontFamilyDd.SelectedItemText;

			var family = SelectedFontFamily;

			FontStyle style = _fontStyleDictionary.FindBy(family);

			_attachedBox.tpText.SetFontToSelection(family, style);

			_fontStyleDictionary.ApplyEnabled(family);

			ApplyStylesToggling();

			Invalidate();
		}

		private void tpText_SelectionChanged(object sender, EventArgs e)
		{
			//if (!WasStylesChanged) return;

			_attachedBox.tpText.SelectionChanged -= tpText_SelectionChanged;
			
			FillControl(false);
			
			_attachedBox.tpText.SelectionChanged += tpText_SelectionChanged;

			UpdateLastSelections();

			UpdateToggles();

			InvalidateToggles();
		}

		private void UpdateLastSelections()
		{
			_lastSelectionFont = _attachedBox.tpText.SelectionFont;
			_lastSelectionColor = _attachedBox.tpText.SelectionColor;
			_lastSelectionAlignment = _attachedBox.tpText.SelectionAlignment;
		}

		private void InvalidateToggles()
		{
			_alignmentDictionary.InvalidateAll();
			_fontStyleDictionary.InvalidateAll();
		}

		private void UpdateToggles()
		{
			_fontStyleDictionary.ApplyEnabled(SelectedFontFamily);
			_alignmentDictionary.ApplyToggling(_attachedBox.tpText.SelectionAlignment);
			

			if (_attachedBox.tpText.HasDifferentFonts)
			{
				_fontStyleDictionary.UntoggleAll();
			}
			else
			{
				_fontStyleDictionary.ApplyToggling(_attachedBox.tpText.SelectionFont);
			}
		}

		private bool WasStylesChanged
		{
			get
			{
				return _attachedBox.tpText.SelectionFont == null || !_attachedBox.tpText.SelectionFont.Equals(_lastSelectionFont) ||
				       _attachedBox.tpText.SelectionColor != _lastSelectionColor ||
				       _attachedBox.tpText.SelectionAlignment != _lastSelectionAlignment;
			}
		}

		#endregion
	}
}