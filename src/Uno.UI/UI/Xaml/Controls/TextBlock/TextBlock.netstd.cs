using System;
using System.Collections.Generic;
using Uno.UI;

namespace Microsoft.UI.Xaml.Controls
{
	partial class TextBlock
	{
		internal override bool IsViewHit() => Text != null || base.IsViewHit();
	}
}
