using Zenject;

namespace Core.Alert
{
	public interface IAlertState
	{
		AlertStateType GetAlertState(object param = null);
	}

	public class PlayerScoreAlertExample : IAlertState
	{
		[Inject]
		private PlayerServiceExample _playerServiceExample;

		public AlertStateType GetAlertState(object param = null)
		{
			var score = _playerServiceExample.ScoreCell.Value;
			if (score < 5)
			{
				return AlertStateType.None;
			}			
			if (score < 10)
			{
				return AlertStateType.New;
			}
			
			if (score < 20)
			{
				return AlertStateType.Update;
			}
			
			if (score < 30)
			{
				return AlertStateType.Finish;
			}
			
			if (score < 40)
			{
				return AlertStateType.Warning;
			}
			
			return AlertStateType.None;
		}
	}
}