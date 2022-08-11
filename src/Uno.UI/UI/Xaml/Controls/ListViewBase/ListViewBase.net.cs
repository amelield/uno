#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;

namespace Microsoft.UI.Xaml.Controls
{
	public partial class ListViewBase
	{
		private int PageSize => throw new NotImplementedException();

		private protected override bool ShouldItemsControlManageChildren => true;

		private void AddItems(int firstItem, int count, int section)
		{
			Refresh();
		}

		private void RemoveItems(int firstItem, int count, int section)
		{
			Refresh();
		}

		private void AddGroup(int groupIndexInView)
		{
			Refresh();
		}

		private void RemoveGroup(int groupIndexInView)
		{
			Refresh();
		}

		private void ReplaceGroup(int groupIndexInView)
		{
			Refresh();
		}

		private ContentControl ContainerFromGroupIndex(int groupIndex) => throw new NotImplementedException();

		private void TryLoadMoreItems()
		{
			//TODO: ISupportIncrementalLoading
		}
	}
}
