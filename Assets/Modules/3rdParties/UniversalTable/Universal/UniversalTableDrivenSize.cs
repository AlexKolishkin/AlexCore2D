using EnhancedUI.EnhancedScroller;
using System;
using System.Collections.Generic;

namespace Core.UniversalTable
{
	public class UniversalTableDrivenSize : UniversalTable
	{
		public void BindDrivenSize<T, TV>(List<T> data, TV prefab, Action<T, TV> fillFactory) where TV : EnhancedScrollerCellView where T : IUniversalTableSize
		{
			var source = new GenericTableSourceDrivenSize<T, TV>()
			{
				Data = data,
				CellPrefab = prefab
			};
			source.fillFactory = fillFactory;
			DataSource = source;
		}

		public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			return _dataSource.GetCellViewSize(scroller, dataIndex);
		}
	}
}