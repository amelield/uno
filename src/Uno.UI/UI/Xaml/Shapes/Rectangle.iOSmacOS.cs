﻿using System;
using System.Linq;
using Uno.UI.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using CoreGraphics;

#if __IOS__
using UIKit;
using _BezierPath = UIKit.UIBezierPath;
#elif __MACOS__
using AppKit;
using _BezierPath = AppKit.NSBezierPath;
#endif

#if NET6_0_OR_GREATER
using ObjCRuntime;
#endif

namespace Windows.UI.Xaml.Shapes
{
	public partial class Rectangle
	{
		static Rectangle()
		{
			StretchProperty.OverrideMetadata(typeof(Rectangle), new FrameworkPropertyMetadata(defaultValue: Media.Stretch.Fill));
		}

		public Rectangle()
		{
#if __IOS__
			ClipsToBounds = true;
#endif
		}

		/// <inheritdoc />
		protected override Size MeasureOverride(Size availableSize)
			=> base.MeasureRelativeShape(availableSize);

		/// <inheritdoc />
		protected override Size ArrangeOverride(Size finalSize)
		{
			var (shapeSize, renderingArea) = ArrangeRelativeShape(finalSize);

			CGPath path;
			if (renderingArea.Width > 0 && renderingArea.Height > 0)
			{
				path = Math.Max(RadiusX, RadiusY) > 0
#if __IOS__
					? _BezierPath.FromRoundedRect(renderingArea, UIRectCorner.AllCorners, new CGSize(RadiusX, RadiusY)).CGPath
#else
					? _BezierPath.FromRoundedRect(renderingArea, (nfloat)RadiusX, (nfloat)RadiusY).ToCGPath()
#endif
					: _BezierPath.FromRect(renderingArea).ToCGPath();
			}
			else
			{
				path = null;
			}
			Render(path);

			return shapeSize;
		}
	}
}
