#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Microsoft.UI.Xaml.Controls
{
	#if false || false || false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class Control : global::Microsoft.UI.Xaml.FrameworkElement
	{
		// Skipping already declared property VerticalContentAlignment
		// Skipping already declared property Template
		// Skipping already declared property TabNavigation
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Microsoft.UI.Xaml.Controls.RequiresPointer RequiresPointer
		{
			get
			{
				return (global::Microsoft.UI.Xaml.Controls.RequiresPointer)this.GetValue(RequiresPointerProperty);
			}
			set
			{
				this.SetValue(RequiresPointerProperty, value);
			}
		}
		#endif
		// Skipping already declared property Padding
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  bool IsTextScaleFactorEnabled
		{
			get
			{
				return (bool)this.GetValue(IsTextScaleFactorEnabledProperty);
			}
			set
			{
				this.SetValue(IsTextScaleFactorEnabledProperty, value);
			}
		}
		#endif
		// Skipping already declared property IsFocusEngagementEnabled
		// Skipping already declared property IsFocusEngaged
		// Skipping already declared property IsEnabled
		// Skipping already declared property HorizontalContentAlignment
		// Skipping already declared property Foreground
		// Skipping already declared property FontWeight
		// Skipping already declared property FontStyle
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.UI.Text.FontStretch FontStretch
		{
			get
			{
				return (global::Windows.UI.Text.FontStretch)this.GetValue(FontStretchProperty);
			}
			set
			{
				this.SetValue(FontStretchProperty, value);
			}
		}
		#endif
		// Skipping already declared property FontSize
		// Skipping already declared property FontFamily
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Microsoft.UI.Xaml.ElementSoundMode ElementSoundMode
		{
			get
			{
				return (global::Microsoft.UI.Xaml.ElementSoundMode)this.GetValue(ElementSoundModeProperty);
			}
			set
			{
				this.SetValue(ElementSoundModeProperty, value);
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::System.Uri DefaultStyleResourceUri
		{
			get
			{
				return (global::System.Uri)this.GetValue(DefaultStyleResourceUriProperty);
			}
			set
			{
				this.SetValue(DefaultStyleResourceUriProperty, value);
			}
		}
		#endif
		// Skipping already declared property CornerRadius
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  int CharacterSpacing
		{
			get
			{
				return (int)this.GetValue(CharacterSpacingProperty);
			}
			set
			{
				this.SetValue(CharacterSpacingProperty, value);
			}
		}
		#endif
		// Skipping already declared property BorderThickness
		// Skipping already declared property BorderBrush
		// Skipping already declared property BackgroundSizing
		// Skipping already declared property Background
		// Skipping already declared property DefaultStyleKey
		// Skipping already declared property BackgroundProperty
		// Skipping already declared property BackgroundSizingProperty
		// Skipping already declared property BorderBrushProperty
		// Skipping already declared property BorderThicknessProperty
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty CharacterSpacingProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(CharacterSpacing), typeof(int), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(int)));
		#endif
		// Skipping already declared property CornerRadiusProperty
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty DefaultStyleKeyProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(DefaultStyleKey), typeof(object), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(object)));
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty DefaultStyleResourceUriProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(DefaultStyleResourceUri), typeof(global::System.Uri), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(global::System.Uri)));
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty ElementSoundModeProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(ElementSoundMode), typeof(global::Microsoft.UI.Xaml.ElementSoundMode), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(global::Microsoft.UI.Xaml.ElementSoundMode)));
		#endif
		// Skipping already declared property FontFamilyProperty
		// Skipping already declared property FontSizeProperty
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty FontStretchProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(FontStretch), typeof(global::Windows.UI.Text.FontStretch), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(global::Windows.UI.Text.FontStretch)));
		#endif
		// Skipping already declared property FontStyleProperty
		// Skipping already declared property FontWeightProperty
		// Skipping already declared property ForegroundProperty
		// Skipping already declared property HorizontalContentAlignmentProperty
		// Skipping already declared property IsEnabledProperty
		// Skipping already declared property IsFocusEngagedProperty
		// Skipping already declared property IsFocusEngagementEnabledProperty
		// Skipping already declared property IsTemplateFocusTargetProperty
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty IsTemplateKeyTipTargetProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.RegisterAttached(
			"IsTemplateKeyTipTarget", typeof(bool), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(bool)));
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty IsTextScaleFactorEnabledProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(IsTextScaleFactorEnabled), typeof(bool), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(bool)));
		#endif
		// Skipping already declared property PaddingProperty
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static global::Microsoft.UI.Xaml.DependencyProperty RequiresPointerProperty { get; } = 
		Microsoft.UI.Xaml.DependencyProperty.Register(
			nameof(RequiresPointer), typeof(global::Microsoft.UI.Xaml.Controls.RequiresPointer), 
			typeof(global::Microsoft.UI.Xaml.Controls.Control), 
			new FrameworkPropertyMetadata(default(global::Microsoft.UI.Xaml.Controls.RequiresPointer)));
		#endif
		// Skipping already declared property TabNavigationProperty
		// Skipping already declared property TemplateProperty
		// Skipping already declared property VerticalContentAlignmentProperty
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.Control()
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Control()
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngagementEnabled.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngagementEnabled.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngaged.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngaged.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.RequiresPointer.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.RequiresPointer.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontSize.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontSize.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontFamily.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontFamily.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontWeight.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontWeight.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStyle.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStyle.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStretch.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStretch.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CharacterSpacing.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CharacterSpacing.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Foreground.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Foreground.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsTextScaleFactorEnabled.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsTextScaleFactorEnabled.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsEnabled.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsEnabled.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.TabNavigation.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.TabNavigation.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Template.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Template.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Padding.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Padding.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.HorizontalContentAlignment.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.HorizontalContentAlignment.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.VerticalContentAlignment.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.VerticalContentAlignment.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Background.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.Background.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BackgroundSizing.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BackgroundSizing.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderThickness.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderThickness.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderBrush.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderBrush.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleResourceUri.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleResourceUri.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.ElementSoundMode.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.ElementSoundMode.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CornerRadius.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CornerRadius.set
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FocusEngaged.add
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FocusEngaged.remove
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FocusDisengaged.add
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FocusDisengaged.remove
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsEnabledChanged.add
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsEnabledChanged.remove
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.RemoveFocusEngagement()
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.ApplyTemplate()
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleKey.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleKey.set
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.GetTemplateChild(string)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerEntered(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerPressed(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerMoved(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerReleased(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerExited(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerCaptureLost(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerCanceled(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnPointerWheelChanged(Microsoft.UI.Xaml.Input.PointerRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnTapped(Microsoft.UI.Xaml.Input.TappedRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnDoubleTapped(Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnHolding(Microsoft.UI.Xaml.Input.HoldingRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnRightTapped(Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnManipulationStarting(Microsoft.UI.Xaml.Input.ManipulationStartingRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnManipulationInertiaStarting(Microsoft.UI.Xaml.Input.ManipulationInertiaStartingRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnManipulationStarted(Microsoft.UI.Xaml.Input.ManipulationStartedRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnManipulationDelta(Microsoft.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnManipulationCompleted(Microsoft.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnKeyUp(Microsoft.UI.Xaml.Input.KeyRoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnKeyDown(Microsoft.UI.Xaml.Input.KeyRoutedEventArgs)
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		protected virtual void OnPreviewKeyDown( global::Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Controls.Control", "void Control.OnPreviewKeyDown(KeyRoutedEventArgs e)");
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		protected virtual void OnPreviewKeyUp( global::Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Controls.Control", "void Control.OnPreviewKeyUp(KeyRoutedEventArgs e)");
		}
		#endif
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnGotFocus(Microsoft.UI.Xaml.RoutedEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnLostFocus(Microsoft.UI.Xaml.RoutedEventArgs)
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		protected virtual void OnCharacterReceived( global::Microsoft.UI.Xaml.Input.CharacterReceivedRoutedEventArgs e)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Controls.Control", "void Control.OnCharacterReceived(CharacterReceivedRoutedEventArgs e)");
		}
		#endif
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnDragEnter(Microsoft.UI.Xaml.DragEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnDragLeave(Microsoft.UI.Xaml.DragEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnDragOver(Microsoft.UI.Xaml.DragEventArgs)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.OnDrop(Microsoft.UI.Xaml.DragEventArgs)
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngagementEnabledProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsFocusEngagedProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.RequiresPointerProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontSizeProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontFamilyProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontWeightProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStyleProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.FontStretchProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CharacterSpacingProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.ForegroundProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsTextScaleFactorEnabledProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsEnabledProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.TabNavigationProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.TemplateProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.PaddingProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.HorizontalContentAlignmentProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.VerticalContentAlignmentProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BackgroundProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BackgroundSizingProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderThicknessProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.BorderBrushProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleKeyProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.DefaultStyleResourceUriProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.ElementSoundModeProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.CornerRadiusProperty.get
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsTemplateFocusTargetProperty.get
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.GetIsTemplateFocusTarget(Microsoft.UI.Xaml.FrameworkElement)
		// Skipping already declared method Microsoft.UI.Xaml.Controls.Control.SetIsTemplateFocusTarget(Microsoft.UI.Xaml.FrameworkElement, bool)
		// Forced skipping of method Microsoft.UI.Xaml.Controls.Control.IsTemplateKeyTipTargetProperty.get
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static bool GetIsTemplateKeyTipTarget( global::Microsoft.UI.Xaml.DependencyObject element)
		{
			return (bool)element.GetValue(IsTemplateKeyTipTargetProperty);
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public static void SetIsTemplateKeyTipTarget( global::Microsoft.UI.Xaml.DependencyObject element,  bool value)
		{
			element.SetValue(IsTemplateKeyTipTargetProperty, value);
		}
		#endif
		// Skipping already declared event Microsoft.UI.Xaml.Controls.Control.FocusDisengaged
		// Skipping already declared event Microsoft.UI.Xaml.Controls.Control.FocusEngaged
		// Skipping already declared event Microsoft.UI.Xaml.Controls.Control.IsEnabledChanged
	}
}
