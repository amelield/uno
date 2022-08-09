using System;
using Windows.Foundation;
#if XAMARIN_IOS
using UIKit;
#endif

namespace Microsoft.UI.Xaml.Media
{
	public partial class RectangleGeometry : Geometry
	{
		public RectangleGeometry()
		{
			InitPartials();
		}

		partial void InitPartials();

		#region Rect DependencyProperty

		public Rect Rect
		{
			get => (Rect)this.GetValue(RectProperty);
			set => this.SetValue(RectProperty, value);
		}

		public static DependencyProperty RectProperty { get; } =
			DependencyProperty.Register(
				"Rect",
				typeof(Rect), typeof(RectangleGeometry),
				new FrameworkPropertyMetadata(
					null,
					options: FrameworkPropertyMetadataOptions.AffectsMeasure));

		#endregion

		#region Geometry implementation (not implemented)

#if XAMARIN_ANDROID
		public override Android.Graphics.Path ToPath()
		{
			throw new NotImplementedException();
		}
#endif

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

#if XAMARIN_IOS
		public override UIImage ToNativeImage()
		{
			throw new NotImplementedException();
		}

		public override UIImage ToNativeImage(CoreGraphics.CGSize targetSize, UIColor color = null, Thickness margin = default(Thickness))
		{
			throw new NotImplementedException();
		}

		public override CoreGraphics.CGPath ToCGPath()
		{
			throw new NotImplementedException();
		}
#endif

		#endregion

		private protected override Rect ComputeBounds()
		{
			if(Transform is { } transform)
			{
				return transform.TransformBounds(Rect);
			}
			else
			{
				return Rect;
			}
		}
	}
}
