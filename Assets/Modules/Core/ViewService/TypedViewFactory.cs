using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Core.Addressable;
using Core.Resource;

namespace Core.View
{
	public class TypedViewFactory : ITypedViewFactory
	{
		private ILoadingRepository<ViewType, GameObject> typedViewLoadingResources;

		private readonly DiContainer _diContainer;
		private IAddressableService _addressableService;

		public TypedViewFactory(DiContainer diContainer, IAddressableService addressableService)
		{
			_diContainer = diContainer;

			typedViewLoadingResources =
				new AddressableLoadingResourceRepository<ViewType, GameObject>(addressableService);

			typedViewLoadingResources.AddPath(ViewType.None, "None");
			typedViewLoadingResources.AddPath(ViewType.MainMenuWindowViewExample, "MainMenuWindowViewExample");
		}

		public async Task<TypedView> Create(ViewType view)
		{
			var result = await typedViewLoadingResources.LoadPrefab(view);
			var gameObject = _diContainer.InstantiatePrefab(result);
			return gameObject.GetComponent<TypedView>();
		}

		public void UnloadPrefab(ViewType viewType)
		{
			typedViewLoadingResources.UnloadPrefab(viewType);
		}
	}

	public enum ViewType
	{
		None = 0,
		MainMenuWindowViewExample = 1,
	}
}

