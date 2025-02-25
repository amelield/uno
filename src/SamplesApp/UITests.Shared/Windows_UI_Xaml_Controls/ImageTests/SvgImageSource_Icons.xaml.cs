﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.UI.Samples.Controls;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UITests.Windows_UI_Xaml_Controls.ImageTests;

[Sample("Image")]
public sealed partial class SvgImageSource_Icons : Page
{
	public SvgImageSource_Icons()
	{
		this.InitializeComponent();
	}

	private async void OnClick(object sender, RoutedEventArgs args)
	{
		var svgImageSource = new SvgImageSource { RasterizePixelHeight = 48, RasterizePixelWidth = 48 };
		var homeIcon = new Uri("ms-appx:///Assets/Formats/home.svg");
		var file = await StorageFile.GetFileFromApplicationUriAsync(homeIcon);
		var text = await FileIO.ReadTextAsync(file);
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
		await svgImageSource.SetSourceAsync(stream.AsRandomAccessStream());

		var flyout = new MenuFlyout
		{
			Placement = FlyoutPlacementMode.Bottom,
		};
		flyout.Items.Add(
			new MenuFlyoutItem
			{
				Icon = new Microsoft.UI.Xaml.Controls.ImageIcon { Source = svgImageSource },
				Text = "This menu item should have a HOME icon",
			});

		flyout.ShowAt(sender as Button);
	}
}
