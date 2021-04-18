using System.Threading.Tasks;

namespace Core.Scene
{
	public interface ISceneService : IService
	{
		Task Load(SceneName sceneName, bool additive = false);
		Task Unload(SceneName sceneName);
	}
}
