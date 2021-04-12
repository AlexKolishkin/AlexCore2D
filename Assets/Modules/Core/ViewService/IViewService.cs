using Core.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Core.View
{
	public interface IViewService : IService
	{
		Subject<ViewType> OpenedViewStream { get; }
		Subject<ViewType> ClosedViewStream { get; }
		ReactiveProperty<ViewType> CurrentOpenViewCell { get; }

		void Show(ViewType viewType, ViewData data = null, float delay = 0);
		bool Exists(ViewType viewType);
		bool Remove(ViewType viewType, float delay = 0.0f);

		void ClearCanvases();
		Transform WindowCanvas { get; }
	}
}