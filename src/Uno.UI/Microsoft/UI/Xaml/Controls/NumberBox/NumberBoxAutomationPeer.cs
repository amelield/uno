using System;
using Windows.Foundation.Metadata;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;

namespace Microsoft.UI.Xaml.Controls
{
	public class NumberBoxAutomationPeer : AutomationPeer, IRangeValueProvider
	{
		private NumberBox _owner;

		internal NumberBoxAutomationPeer(NumberBox owner)
		{
			_owner = owner;
		}

		// IAutomationPeerOverrides
		protected override object GetPatternCore(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.RangeValue)
			{
				return this;
			}

			return base.GetPatternCore(patternInterface);
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Spinner;
		}

		NumberBox GetImpl()
		{
			return _owner;
		}

		// IRangeValueProvider
		public double Minimum => GetImpl().Minimum;

		public double Maximum => GetImpl().Maximum;

		public double Value => GetImpl().Value;

		public double SmallChange => GetImpl().SmallChange;

		public double LargeChange => GetImpl().LargeChange;

		public bool IsReadOnly => false;

		public void SetValue(double value) => GetImpl().Value = value;

		internal void RaiseValueChangedEvent(double oldValue, double newValue)
		{
			if (ApiInformation.IsPropertyPresent("Microsoft.UI.Xaml.Automation.RangeValuePatternIdentifiers", nameof(RangeValuePatternIdentifiers.ValueProperty)))
			{
				RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty,
							   oldValue,
							   newValue);
			}
		}
	}
}
