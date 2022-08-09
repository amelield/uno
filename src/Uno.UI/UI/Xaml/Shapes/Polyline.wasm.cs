using System;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml.Wasm;
using Uno;
using Uno.Extensions;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace Microsoft.UI.Xaml.Shapes
{
	partial class Polyline
	{
		private readonly SvgElement _polyline = new SvgElement("polyline");

		partial void InitializePartial()
		{
			SvgChildren.Add(_polyline);

			InitCommonShapeProperties();
		}

		protected override SvgElement GetMainSvgElement()
		{
			return _polyline;
		}

		partial void OnPointsChanged()
		{
			var points = Points;
			if (points == null)
			{
				_polyline.RemoveAttribute("points");
			}
			else
			{
				_polyline.SetAttribute("points", points.ToCssString());
			}
		}
	}
}
