using Zenject;

namespace Core.GameState
{

	public interface IGameStateService : IService, IInitializable
	{
		IGameState ChangeState(IGameState newState);
	}

	public class GameStateService : IGameStateService
	{
		private IGameState _gameState;
	
		private readonly DiContainer _diContainer;
	
		[Inject]
		public GameStateService(DiContainer diContainer)
		{
			_diContainer = diContainer;
			Init();
		}

		public void Init()
		{
			_gameState = new EmptyGameState();
		}

		public void Initialize()
		{
			ChangeState(new BoostrapGameState());
		}

		public IGameState ChangeState(IGameState newState)
		{
			if (!_gameState.Equals(newState))
			{
				_gameState.OnExit();
				_gameState = newState;
				_diContainer.Inject(_gameState);
				_gameState.OnEnter();
			}

			return _gameState;
		}


	}
}