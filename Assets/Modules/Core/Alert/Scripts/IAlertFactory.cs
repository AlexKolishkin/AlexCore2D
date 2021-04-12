using System;
using UnityEngine;
using Zenject;

namespace Core.Alert
{
	public interface IAlertFactory : IFactory<AlertType, IAlertState>
	{
	}

	public class AlertFactory : IAlertFactory
	{
		private DiContainer _diContainer;

		public AlertFactory(DiContainer diContainer)
		{
			_diContainer = diContainer;
			
		}

		public IAlertState Create(AlertType alertType)
		{
			Type type = null; 
			switch (alertType)
			{
				case AlertType.PlayerScoreAlertExample:
					type = typeof(PlayerScoreAlertExample);
					break;
				
				// write new alert here

				default:
					throw new ArgumentOutOfRangeException(nameof(alertType), alertType, null);
			}
			
			return (IAlertState) _diContainer.Instantiate(type);
		}
	}
	
	public enum AlertType
	{
		PlayerScoreAlertExample,
	}
}