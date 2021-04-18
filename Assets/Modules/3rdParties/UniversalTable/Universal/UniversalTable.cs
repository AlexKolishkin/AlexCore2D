using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace Core.UniversalTable
{
    [RequireComponent(typeof(EnhancedScroller))]
	public class UniversalTable : MonoBehaviour, IEnhancedScrollerDelegate
	{
		public EnhancedScroller Scroller;

		protected IGenericTableSource _dataSource;

		public void Bind<T, TV>(List<T> data, TV prefab, Action<T, TV> fillFactory) where TV : EnhancedScrollerCellView
		{
			var source = new GenericTableSource<T, TV>()
			{
				Data = data,
				CellPrefab = prefab
			};
			source.fillFactory = fillFactory;
			DataSource = source;
		}

		public void ReloadData() => Scroller.ReloadData();

		public IGenericTableSource DataSource
		{
			set
			{
				_dataSource = value;
				Scroller.ReloadData();
			}
			get { return _dataSource; }
		}

		private void Awake()
		{
			if (!Scroller)
				Scroller = GetComponent<EnhancedScroller>();

			Scroller.Delegate = this;
		}

		public int GetNumberOfCells(EnhancedScroller scroller)
		{
			if (_dataSource == null)
			{
				//Debug.LogError("no Data source on {0}".F(gameObject.name), this);
				return 0;
			}

			return _dataSource.GetNumberOfCells(scroller);
		}

		public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
		{
			return _dataSource.GetCellViewSize(scroller, dataIndex);
		}

		public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
		{
			return _dataSource.GetCellView(scroller, dataIndex, cellIndex);
		}

		public void Refresh()
		{
			StartCoroutine(RefreshImp());
		}

		IEnumerator RefreshImp()
		{
			yield return new WaitForSeconds(0.1f);
			Scroller.ScrollPosition = Scroller.ScrollPosition + 0.01f;
		}

		private void OnEnable()
		{
			if (DataSource != null)
				Scroller.ScrollPosition = 0;
		}
	}
}