#if __IOS__ || __ANDROID__
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.UI.Xaml.Controls;

public partial class RefreshContainer : ContentControl
{
	public RefreshContainer()
	{
		DefaultStyleKey = typeof(RefreshContainer);

		InitializePlatform();
	}

	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		m_refreshVisualizer = Visualizer;
		if (m_refreshVisualizer == null)
		{
			Visualizer = new RefreshVisualizer();
			m_hasDefaultRefreshVisualizer = true;
		}
		else
		{
			m_hasDefaultRefreshVisualizer = false;
		}

		OnApplyTemplatePartial();
	}

	partial void OnApplyTemplatePartial();

	/// <summary>
	/// Initiates an update of the content.
	/// </summary>
	public void RequestRefresh() => RequestRefreshPlatform();

	private void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
	{		
	}

	private void OnNativeRefreshingChanged()
	{
		if (IsNativeRefreshing && !_managedIsRefreshing)
		{
			_managedIsRefreshing = true;

			var deferral = new Deferral(() =>
			{
				// CheckThread();
				EndNativeRefreshing();
				_managedIsRefreshing = false;
				//RefreshCompleted();
			});

			var args = new RefreshRequestedEventArgs(deferral);

			//This makes sure that everyone registered for this event can get access to the deferral
			//Otherwise someone could complete the deferral before someone else has had a chance to grab it
			args.IncrementDeferralCount();
			RefreshRequested?.Invoke(this, args);
			args.DecrementDeferralCount();
		}
	}
}
#endif
