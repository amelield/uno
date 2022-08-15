 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.UI.Samples.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI.Popups;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Uno.UI.Samples.Content.UITests.CommandBar
{
	[SampleControlInfo("CommandBar", "Examples")]
	public sealed partial class CommandBar_Examples : UserControl
	{
		public CommandBar_Examples()
		{
			this.InitializeComponent();
		}

		private async void OnCommandClicked(object sender, RoutedEventArgs e)
		{
			await new Windows.UI.Popups.MessageDialog("Clicked").ShowAsync();
		}
	}
}
