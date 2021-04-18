using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
	public class SceneService : ISceneService
	{
		public async Task Load(SceneName sceneName, bool additive = false)
		{
			Debug.Log($"Prepare to loading scene: {sceneName}");
			await SceneManager.LoadSceneAsync(sceneName.ToString(), additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
			Debug.Log($"Loading scene done: {sceneName}");
		}

		public async Task Unload(SceneName sceneName)
		{
			Debug.Log($"Prepare to unLoad scene: {sceneName}");
			await SceneManager.UnloadSceneAsync(sceneName.ToString());
			Debug.Log($"Unloading scene done: {sceneName}");
		}
	}

	public enum SceneName
	{
		BaseScene = 0,
		LoadingScene = 1,
		MainScene = 2,
	}
}