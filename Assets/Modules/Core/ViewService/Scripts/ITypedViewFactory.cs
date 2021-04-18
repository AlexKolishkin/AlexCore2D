using System.Threading.Tasks;

namespace Core.View
{
	public interface ITypedViewFactory
	{
		Task<TypedView> Create(ViewType view);
		void UnloadPrefab(ViewType viewType);
	}
}
