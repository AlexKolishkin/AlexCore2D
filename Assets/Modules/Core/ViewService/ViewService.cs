using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.View
{
	public class ViewService : IViewService, IInitializable
	{
		public Subject<ViewType> OpenedViewStream { get; } = new Subject<ViewType>();
		public Subject<ViewType> ClosedViewStream { get; } = new Subject<ViewType>();
		public ReactiveProperty<ViewType> CurrentOpenViewCell { get; } = new ReactiveProperty<ViewType>(ViewType.None);


		private readonly WindowInstanceCollection typedViewInstances = new WindowInstanceCollection();

		private CanvasProvider CanvasProvider;
		private readonly ICoroService _coroutineService;
		private readonly ITypedViewFactory _typedViewFactory;

		[Inject]
		public ViewService(ITypedViewFactory typedViewFactory, ICoroService coroutineService)
		{
			_coroutineService = coroutineService;
			_typedViewFactory = typedViewFactory;

			CanvasProvider = new CanvasProvider();
		}

		public void Initialize()
		{
			Debug.Log("ViewService Initialize");
			CurrentOpenViewCell.Subscribe(val => { Debug.Log($"Current window: {val}"); });
		}

		public void ClearCanvases()
		{
			foreach (var viewType in typedViewInstances.Keys) Remove(viewType);
			Remove(CurrentOpenViewCell.Value);
		}

		#region IViewService

		public Transform WindowCanvas => CanvasProvider.WindowCanvas;

		private bool _isShowProcess;
		public void Show(ViewType viewType, ViewData data = null, float delay = 0)
		{
			if (_isShowProcess)
			{
				Debug.Log("Show in process.");
				return;
			}
			_isShowProcess = true;

			if (!typedViewInstances.ContainsKey(viewType))
			{
				typedViewInstances.Add(viewType, null);
			}

			_coroutineService.Run(ShowDelayedImpl(viewType, delay, data), () =>
			{
				_isShowProcess = false;
			});
		}

		private IEnumerator ShowDelayedImpl(ViewType viewType, float delay, ViewData data)
		{
			var instancedCached = false;

			if (typedViewInstances.ContainsKey(viewType))
			{
				var instance = typedViewInstances.Get(viewType);
				if (!instance)
				{
					Debug.Log("Instance was destroyed, recreating");
				}
				else
				{
					yield return new WaitForSeconds(delay);
					instance.Setup(data);
					instancedCached = true;
				}
			}

			if (!instancedCached)
			{
				if (delay > 0)
				{
					yield return new WaitForSeconds(delay);
				}

				var task = _typedViewFactory.Create(viewType);
				yield return new WaitWhile(() => !task.IsCompleted);
				var typedView = task.Result;

				if (typedView != null)
				{
					typedViewInstances.Add(viewType, typedView);
					typedView.transform.SetParent(CanvasProvider.GetCanvasTransform(typedView.CanvasType), false);

					if (data?.ViewDepth != null)
					{
						typedView.SetViewDepth(data.ViewDepth.Value);
					}

					SortByDepth(typedView.CanvasType);

					var animatedProcess = typedView.AnimIn();

					typedView.OnSetupPrepare();

					typedView.Setup(data);

					yield return animatedProcess.WaitComplete();

					typedView.OnSetupDone();

					CurrentOpenViewCell.Value = viewType;
					OpenedViewStream.OnNext(typedView.Type);
				}
				else
				{
					throw new UnityException($"Not found TypedView on view => {viewType}");
				}

				typedView.transform.SetAsLastSibling();
			}
		}

		public bool Remove(ViewType viewType, float delay = 0.0f)
		{
			if (typedViewInstances.ContainsKey(viewType))
			{
				Debug.Log("Removing " + viewType);

				var view = typedViewInstances.Get(viewType);
				typedViewInstances.Remove(viewType);

				if (view && view.gameObject)
				{
					_coroutineService.Run(RemoveImpl(view, delay));
					return true;
				}
			}
			else
			{
				Debug.Log("Not found: " + viewType);
			}

			return false;
		}

		private IEnumerator RemoveImpl(TypedView view, float delay)
		{
			yield return view.AnimOut();
			yield return new WaitForSeconds(delay);
			if (view && view.gameObject)
			{
				view.OnViewRemove();
				ClosedViewStream.OnNext(view.Type);
				_typedViewFactory.UnloadPrefab(view.Type);
				Object.Destroy(view.gameObject);
			}
		}

		public bool Exists(ViewType viewType)
		{
			return typedViewInstances.ContainsKey(viewType);
		}

		public T FindView<T>(ViewType type) where T : TypedView
		{
			if (typedViewInstances.ContainsKey(type))
			{
				var inst = typedViewInstances.Get(type);
				return inst as T;
			}

			return null;
		}

		private void SortByDepth(CanvasType canvasType)
		{
			var list = new List<BaseView>(typedViewInstances.Count);
			var canvasTransform = CanvasProvider.GetCanvasTransform(canvasType);
			list.AddRange(typedViewInstances.Values.Where(val => val.transform.parent == canvasTransform).ToList());
			list.Sort((v1, v2) => v1.SortDepth.CompareTo(v2.SortDepth));

			for (var i = 0; i < list.Count; i++)
				if (list[i] && list[i].gameObject)
					list[i].GetComponent<RectTransform>().SetSiblingIndex(i);
		}

		#endregion
	}

	public class ViewData
	{
		public ViewData(object userData)
		{
			UserData = userData;
		}

		public ViewData()
		{
		}

		public object UserData { get; set; }
		public int? ViewDepth { get; set; } = null;
		public ViewType ParentView { get; set; } = ViewType.None;
		public ViewData ParentViewData { get; set; } = null;
	}


}