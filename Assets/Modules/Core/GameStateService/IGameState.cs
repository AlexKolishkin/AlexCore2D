namespace Core.GameState
{
	public interface IGameState
	{
		void OnEnter();
		void OnExit();
	}

	public class EmptyGameState : IGameState
	{
		public void OnEnter()
		{
		}

		public void OnExit()
		{
		}
	}

}