using Core.Scene;
using UnityEngine;
using Zenject;

namespace Core.GameState
{
	public class LoadingGameState : IGameState
	{
		private LoadingWindowView _loadingWindow;

		[Inject] private ISceneService _sceneService;

		public async void OnEnter()
		{
			await _sceneService.Load(SceneName.LoadingScene, true);
			_loadingWindow = GameObject.FindObjectOfType<LoadingWindowView>();
		}

		private void Update(LoadingData data)
		{
			_loadingWindow.Fill(data);
		}

		public async void OnExit()
		{
			await _sceneService.Unload(SceneName.LoadingScene);
			Debug.Log("LoadingGameState OnExit");
		}
	}
}