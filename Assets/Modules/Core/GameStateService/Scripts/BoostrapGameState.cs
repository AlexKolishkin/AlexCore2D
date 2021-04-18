using Core.Scene;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.GameState
{

	public class BoostrapGameState : IGameState
	{
		private LoadingWindowView _loadingWindow;

		[Inject] private DiContainer _diContainer;

		[Inject] private IGameStateService _gameStateService;

		[Inject] private ISceneService _sceneService;

		public async void OnEnter()
		{
			InstallCulture();

			_loadingWindow = GameObject.FindObjectOfType<LoadingWindowView>();

			await LoadingCycle();
		}

		private void InstallCulture()
		{
			var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture = customCulture;
		}

		private async Task LoadingCycle()
		{
			var services = _diContainer.ResolveAll<ILoading>();
			int servicesCount = services.Count;

			while (true)
			{
				if (services.All(val => val.LoadingState == LoadingState.Loaded))
				{
					break;
				}

				ILoading loading = services.FirstOrDefault(val => val.LoadingState == LoadingState.Loading);

				if (loading != null)
				{
					int doneCount = services.Count(val => val.LoadingState == LoadingState.Loaded);
					float progress = doneCount / (float)servicesCount;
					Update(new LoadingData(progress, $"Loading {loading.GetType().Name}"));
				}

				await Task.Delay(100);
			}

			await _sceneService.Load(SceneName.MainScene);
			_gameStateService.ChangeState(new GameplayGameState());
		}

		private void Update(LoadingData data)
		{
			var loadingData = data;
			if (_loadingWindow != null)
			{
				_loadingWindow.Fill(loadingData);
			}
		}

		public void OnExit()
		{
			Debug.Log("LoadingGameState OnExit");
		}
	}
}