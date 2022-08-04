using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Extensions;
using Uno.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Uno.UI.Helpers.Xaml
{
	public static class SetterExtensions
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// This is normally called from code generated by the Xaml parser. This overload is kept for backwards compatibility.
		public static Setter ApplyThemeResourceUpdateValues(this Setter setter, string themeResourceName, object parseContext)
			=> ApplyThemeResourceUpdateValues(setter, themeResourceName, parseContext, isTheme: true, isHotReload: false);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// This is normally called from code generated by the Xaml parser
		public static Setter ApplyThemeResourceUpdateValues(this Setter setter, string themeResourceName, object parseContext, bool isTheme, bool isHotReload)
		{
			if (isHotReload && !isTheme && FeatureConfiguration.Xaml.ForceHotReloadDisabled)
			{
				// If hot reload is disabled and it's not a theme resource assignation, don't create a resource binding
				return setter;
			}

			setter.ThemeResourceKey = !themeResourceName.IsNullOrEmpty() ? themeResourceName : null;
			setter.ThemeResourceContext = parseContext as XamlParseContext;

			if (isTheme)
			{
				setter.ResourceBindingUpdateReason |= ResourceUpdateReason.ThemeResource;
			}
			if (isHotReload)
			{
				setter.ResourceBindingUpdateReason |= ResourceUpdateReason.HotReload;
			}

			return setter;
		}
	}
}
