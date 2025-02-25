﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Uno.Extensions;
using Uno.Foundation.Logging;
using Uno.UI;
using Uno.UI.DataBinding;
using Windows.UI.Xaml.Media.Animation;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Uno;

#if XAMARIN_ANDROID
using View = Android.Views.View;
using ViewGroup = Android.Views.ViewGroup;
using Font = Android.Graphics.Typeface;
using Android.Graphics;
#elif XAMARIN_IOS
using View = UIKit.UIView;
using ViewGroup = UIKit.UIView;
using Color = UIKit.UIColor;
using Font = UIKit.UIFont;
using UIKit;
#elif __MACOS__
using View = AppKit.NSView;
using ViewGroup = AppKit.NSView;
using Color = AppKit.NSColor;
using Font = AppKit.NSFont;
using AppKit;
#else
using View = Windows.UI.Xaml.UIElement;
#endif

namespace Windows.UI.Xaml.Controls
{
	[ContentProperty(Name = "Content")]
	public partial class ContentControl : Control, IEnumerable
	{
		private View? _contentTemplateRoot;

		/// <summary>
		/// Will be set to either the result of ContentTemplateSelector or to ContentTemplate, depending on which is used
		/// </summary>
		private DataTemplate? _dataTemplateUsedLastUpdate;

		private bool _canCreateTemplateWithoutParent;

		/// <summary>
		/// Flag to determine if the current content has been overridden.
		/// This is only in use when <see cref="IsContentPresenterBypassEnabled"/> is true.
		/// </summary>
		private bool _localContentDataContextOverride;

		protected override bool CanCreateTemplateWithoutParent { get { return _canCreateTemplateWithoutParent; } }

#nullable disable // Public members should stay nullable-oblivious for now to stay consistent with WinUI
		public ContentControl()
		{
			DefaultStyleKey = typeof(ContentControl);

			InitializePartial();
		}

		partial void InitializePartial();

		#region Content DependencyProperty

		public virtual object Content
		{
			get
			{
				if (this.IsDependencyPropertySet(ContentProperty))
				{
					return GetValue(ContentProperty);
				}
				else if (ContentTemplate != null)
				{
					return DataContext;
				}
				else
				{
					// Return null to be sure that the Content will be empty and prevent the type to be dispayed.
					return null;
				}
			}
			set { SetValue(ContentProperty, value); }
		}

		public static DependencyProperty ContentProperty { get; } =
			DependencyProperty.Register(
				"Content",
				typeof(object),
				typeof(ContentControl),
				new FrameworkPropertyMetadata(
					defaultValue: null,
					// Don't propagate DataContext to Content qua Content, only propagate it via the visual tree. Prevents spurious
					// propagation in case that default style and template is only applied once the control enters the visual tree
					// (ie if created in code by new SomeControl())
					// NOTE: There's a case we currently don't support: if the Content is a DependencyObject but *not* a FrameworkElement, then
					// the DataContext won't get propagated and any bindings won't get updated.
					FrameworkPropertyMetadataOptions.ValueDoesNotInheritDataContext,
					propertyChangedCallback: (s, e) => ((ContentControl)s)?.OnContentChanged(e.OldValue, e.NewValue)
				)
			);

		#endregion

		#region ContentTemplate DependencyProperty

		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ContentTemplate.  This enables animation, styling, binding, etc...
		public static DependencyProperty ContentTemplateProperty { get; } =
			DependencyProperty.Register(
				"ContentTemplate",
				typeof(DataTemplate),
				typeof(ContentControl),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.ValueDoesNotInheritDataContext,
					(s, e) => ((ContentControl)s)?.OnContentTemplateChanged(e.OldValue as DataTemplate, e.NewValue as DataTemplate)
				)
			);
		#endregion

		#region ContentTemplateSelector DependencyProperty

		public DataTemplateSelector ContentTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
			set { SetValue(ContentTemplateSelectorProperty, value); }
		}

		public static DependencyProperty ContentTemplateSelectorProperty { get; } =
			DependencyProperty.Register(
				"ContentTemplateSelector",
				typeof(DataTemplateSelector),
				typeof(ContentControl),
				new FrameworkPropertyMetadata(
					null,
					(s, e) => ((ContentControl)s)?.OnContentTemplateSelectorChanged(e.OldValue as DataTemplateSelector, e.NewValue as DataTemplateSelector)
				)
			);
		#endregion

		protected virtual void OnContentChanged(object oldValue, object newValue)
		{
			if (IsContentPresenterBypassEnabled)
			{
				if (newValue is View
					// This case is to support the ability for the content
					// control to be templated while having a Content as a view.
					// The template then needs to TemplateBind to the Content to function
					// properly.
					&& Template == null
				)
				{
					// If the content is a view, no need to delay the inclusion in the visual tree
					ContentTemplateRoot = newValue as View;
				}
				else if (oldValue != null && newValue == null)
				{
					// The content is being reset, remove the existing content properly.
					ContentTemplateRoot = null;
				}

				if (newValue != null)
				{
					SetUpdateTemplate();
				}
			}
		}

		protected virtual void OnContentTemplateChanged(DataTemplate oldTemplate, DataTemplate newTemplate)
		{
			if (IsContentPresenterBypassEnabled)
			{
				if (ContentTemplateRoot != null)
				{
					ContentTemplateRoot = null;
				}

				SetUpdateTemplate();
			}
			else if (CanCreateTemplateWithoutParent)
			{
				SetUpdateControlTemplate();
			}
		}

		protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector dataTemplateSelector1, DataTemplateSelector dataTemplateSelector2)
		{
			if (IsContentPresenterBypassEnabled)
			{
				// In case there's code that happen to be here.
			}
		}
#nullable enable

		private void SetUpdateTemplate()
		{
			if (this.HasParent() || CanCreateTemplateWithoutParent)
			{
				UpdateContentTemplateRoot();
				SyncDataContext();
				this.InvalidateMeasure();
			}
		}

		partial void UnregisterContentTemplateRoot();

#nullable disable // Public members should stay nullable-oblivious for now to stay consistent with WinUI
		public virtual View ContentTemplateRoot
		{
			get
			{
				return _contentTemplateRoot;
			}

			protected set
			{
				var previousValue = _contentTemplateRoot;

				if (previousValue != null)
				{
					ResetContentDataContextOverride();
					UnregisterContentTemplateRoot();

					UpdateContentTransitions(this.ContentTransitions, null);
				}

				_contentTemplateRoot = value;

				if (_contentTemplateRoot != null)
				{
					RegisterContentTemplateRoot();

					UpdateContentTransitions(null, this.ContentTransitions);
				}
			}
		}

		#region Transitions Dependency Property

		public TransitionCollection ContentTransitions
		{
			get { return (TransitionCollection)this.GetValue(ContentTransitionsProperty); }
			set { this.SetValue(ContentTransitionsProperty, value); }
		}

		public static DependencyProperty ContentTransitionsProperty { get; } =
			DependencyProperty.Register("ContentTransitions", typeof(TransitionCollection), typeof(ContentControl), new FrameworkPropertyMetadata(null, OnContentTransitionsChanged));
#nullable enable

		private static void OnContentTransitionsChanged(object dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var control = dependencyObject as ContentControl;

			if (control != null)
			{
				var oldValue = (TransitionCollection)args.OldValue;
				var newValue = (TransitionCollection)args.NewValue;

				control.UpdateContentTransitions(oldValue, newValue);
			}
		}

		#endregion

		private void UpdateContentTransitions(TransitionCollection? oldValue, TransitionCollection? newValue)
		{
			var contentRoot = this.ContentTemplateRoot as IFrameworkElement;

			if (contentRoot == null)
			{
				return;
			}

			if (oldValue != null)
			{
				foreach (var item in oldValue)
				{
					item.DetachFromElement(contentRoot);
				}
			}

			if (newValue != null)
			{
				foreach (var item in newValue)
				{
					item.AttachToElement(contentRoot);
				}
			}
		}

		private protected override void OnLoaded()
		{
			base.OnLoaded();

			if (IsContentPresenterBypassEnabled)
			{
				SetUpdateTemplate();
			}
		}

		protected override void OnVisibilityChanged(Visibility oldValue, Visibility newValue)
		{
			base.OnVisibilityChanged(oldValue, newValue);

			if (oldValue == Visibility.Collapsed && newValue == Visibility.Visible)
			{
				SetUpdateTemplate();
			}
		}

		public void UpdateContentTemplateRoot()
		{
			if (Visibility == Visibility.Collapsed)
			{
				return;
			}

			//If ContentTemplateRoot is null, it must be updated even if the templates haven't changed
			if (ContentTemplateRoot == null)
			{
				_dataTemplateUsedLastUpdate = null;
			}

			//ContentTemplate/ContentTemplateSelector will only be applied to a control with no Template, normally the innermost element
			if (IsContentPresenterBypassEnabled)
			{

				var dataTemplate = this.ResolveContentTemplate();

				//Only apply template if it has changed
				if (!object.Equals(dataTemplate, _dataTemplateUsedLastUpdate))
				{
					_dataTemplateUsedLastUpdate = dataTemplate;
					ContentTemplateRoot = dataTemplate?.LoadContentCached() ?? Content as View;
				}

				if (Content != null
					&& !(Content is View)
					&& ContentTemplateRoot == null
					&& dataTemplate == null
					&& ContentTemplate == null
				)
				{
					SetContentTemplateRootToPlaceholder();
				}

				if (ContentTemplateRoot == null && Content is View contentView && dataTemplate == null)
				{
					// No template and Content is a View, set it directly as root
					ContentTemplateRoot = contentView as View;
				}
			}

			if (ContentTemplateRoot != null)
			{
				OnApplyTemplate();
			}

			SyncDataContext();
		}

		private void SetContentTemplateRootToPlaceholder()
		{
			if (this.Log().IsEnabled(Uno.Foundation.Logging.LogLevel.Debug))
			{
				this.Log().DebugFormat("No ContentTemplate was specified for {0} and content is not a UIView, defaulting to TextBlock.", GetType().Name);
			}

			ContentTemplateRoot = new ImplicitTextBlock(this)
				.Binding("Text", "")
				.Binding("HorizontalAlignment", new Binding { Path = "HorizontalContentAlignment", Source = this, Mode = BindingMode.OneWay })
				.Binding("VerticalAlignment", new Binding { Path = "VerticalContentAlignment", Source = this, Mode = BindingMode.OneWay });
		}

		partial void RegisterContentTemplateRoot();

		internal protected override void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnDataContextChanged(e);

			if (IsContentPresenterBypassEnabled)
			{
				SyncDataContext();
			}
		}

		protected virtual void SyncDataContext()
		{
			if (IsContentPresenterBypassEnabled)
			{
				// This case is to support the ability for the content
				// control to be templated while having a Content as a view.
				// The template then needs to TemplateBind to the Content to function
				// properly.
				if (Content is View)
				{
					ResetContentDataContextOverride();
				}
				else
				{
					if ((ContentTemplateRoot is IDependencyObjectStoreProvider provider) &&
						// The DataContext may be set directly on the template root
						(_localContentDataContextOverride || !(provider as DependencyObject).IsDependencyPropertyLocallySet(provider.Store.DataContextProperty))
					)
					{
						_localContentDataContextOverride = true;
						provider.Store.SetValue(provider.Store.DataContextProperty, Content, DependencyPropertyValuePrecedences.Local);
					}
				}
			}
			else
			{
				ResetContentDataContextOverride();
			}
		}

		private void ResetContentDataContextOverride()
		{
			if (_localContentDataContextOverride && ContentTemplateRoot is IDependencyObjectStoreProvider provider)
			{
				_localContentDataContextOverride = false;
				provider.Store.ClearValue(provider.Store.DataContextProperty, DependencyPropertyValuePrecedences.Local);
			}
		}

		/// <summary>
		/// This property determines if the current instance is not providing a Control
		/// Template, to allow for the ContentControl to avoid using a ContentPresenter. This extra layer
		/// is a problem on Android, where the stack size is severely limited (32KB at most)
		/// on version 4.4 and earlier.
		/// Android 5.0 does not have this limitation, because ART requires greater stack sizes.
		/// </summary>
		/// <remarks>
		/// If the default style for the current type has a Template property, 
		/// we know that the IsContentPresenterBypassEnabled will be false once the style has been set.
		/// Return false in this case, even if the Template is null.
		/// </remarks>
		internal bool IsContentPresenterBypassEnabled => Template == null && !HasDefaultTemplate(GetDefaultStyleKey());

		/// <summary>
		/// Gets whether the default style for the given type sets a non-null Template.
		/// </summary>
		private static Func<Type, bool> HasDefaultTemplate =
			Funcs.CreateMemoized((Type type) =>
				Style.GetDefaultStyleForType(type) is Style defaultStyle
					&& defaultStyle
						.Flatten(s => s.BasedOn!)
						.SelectMany(s => s.Setters)
						.OfType<Setter>()
						.Any(s => s.Property == TemplateProperty && s.Value != null)
			);

		/// <summary>
		/// Creates a ContentControl which can be measured without being added to the visual tree (eg as container in virtualized lists).
		/// </summary>
		internal static ContentControl CreateItemContainer()
		{
			return new ContentControl
			{
				_canCreateTemplateWithoutParent = true,
				IsGeneratedContainer = true
			};
		}

#nullable disable // Public members should stay nullable-oblivious for now to stay consistent with WinUI
#if XAMARIN_ANDROID
		// Support for the C# collection initializer style.
		public void Add(View view)
		{
			Content = view;
		}

		public IEnumerator GetEnumerator()
		{
			if (Content != null)
			{
				return new[] { Content }.GetEnumerator();
			}
			else
			{
				return Enumerable.Empty<object>().GetEnumerator();
			}
		}
#endif

		public override string GetAccessibilityInnerText()
		{
			switch (Content)
			{
				case string str:
					return str;
				case IFrameworkElement frameworkElement:
					return frameworkElement.GetAccessibilityInnerText();
				case object content:
					return content.ToString();
				default:
					return null;
			}
		}
#nullable enable
	}
}
