#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Microsoft.UI.Xaml.Automation.Peers
{
	#if false || false || false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class TreeViewItemDataAutomationPeer : global::Microsoft.UI.Xaml.Automation.Peers.ItemAutomationPeer,global::Microsoft.UI.Xaml.Automation.Provider.IExpandCollapseProvider
	{
		// Skipping already declared property ExpandCollapseState
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public TreeViewItemDataAutomationPeer( object item,  global::Microsoft.UI.Xaml.Automation.Peers.TreeViewListAutomationPeer parent) : base(item, parent)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.UI.Xaml.Automation.Peers.TreeViewItemDataAutomationPeer", "TreeViewItemDataAutomationPeer.TreeViewItemDataAutomationPeer(object item, TreeViewListAutomationPeer parent)");
		}
		#endif
		// Forced skipping of method Microsoft.UI.Xaml.Automation.Peers.TreeViewItemDataAutomationPeer.TreeViewItemDataAutomationPeer(object, Microsoft.UI.Xaml.Automation.Peers.TreeViewListAutomationPeer)
		// Forced skipping of method Microsoft.UI.Xaml.Automation.Peers.TreeViewItemDataAutomationPeer.ExpandCollapseState.get
		// Skipping already declared method Microsoft.UI.Xaml.Automation.Peers.TreeViewItemDataAutomationPeer.Collapse()
		// Skipping already declared method Microsoft.UI.Xaml.Automation.Peers.TreeViewItemDataAutomationPeer.Expand()
		// Processing: Microsoft.UI.Xaml.Automation.Provider.IExpandCollapseProvider
	}
}
