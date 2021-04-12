using Core.View;
using UnityEngine;
using Zenject;

namespace Core.GameState
{
	public class GameplayGameState : IGameState
	{
		[Inject] private ViewService _viewService;

		public void OnEnter()
		{
			_viewService.Show(ViewType.MainMenuWindowViewExample);
		}

		public void OnExit()
		{
			Debug.Log("GameplayGameState OnExit");
		}
	}
}