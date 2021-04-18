using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace Core.UniversalTable
{
	public interface IGenericTableSource : IEnhancedScrollerDelegate
	{
	}

	public class GenericTableSource<T, TV> : IGenericTableSource where TV : EnhancedScrollerCellView
	{
		public IList<T> Data
		{
			get { return m_data; }
			set
			{
				m_data = value;
				m_dataModified = new List<T>(m_data);
			}
		}

		IList<T> m_data;
		protected IList<T> m_dataModified;

		public Action<T, TV> fillFactory;

		public EnhancedScrollerCellView CellPrefab
		{
			set
			{
				_cellPrefab = value;
				var rect = _cellPrefab.GetComponent<RectTransform>().rect;
				cellHeight = rect.height;
				cellWidth = rect.width;
				_cellPrefab.gameObject.SetActive(true);
			}
		}

		private EnhancedScrollerCellView _cellPrefab;
		private float cellHeight;
		private float cellWidth;

		public int GetNumberOfCells(EnhancedScroller scroller) => Data.Count;

		public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			return scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical ? cellHeight : cellWidth;
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
		{
			TV cellView = scroller.GetCellView(_cellPrefab) as TV;
			cellView.name = dataIndex.ToString();
			cellView.RefreshCellView();
			cellView.Collector.Clear();
			fillFactory?.Invoke(Data[dataIndex], cellView);
			return cellView;
		}
	}
}