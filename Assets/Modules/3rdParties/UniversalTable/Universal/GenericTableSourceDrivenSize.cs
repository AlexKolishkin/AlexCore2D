using EnhancedUI.EnhancedScroller;

namespace Core.UniversalTable
{
	public class GenericTableSourceDrivenSize<T, TV> : GenericTableSource<T, TV> where TV : EnhancedScrollerCellView where T : IUniversalTableSize
	{
		public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			return scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical ? Data[dataIndex].SizeHeight : Data[dataIndex].SizeWidth;
		}
	}
}