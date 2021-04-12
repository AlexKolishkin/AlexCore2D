using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Alert
{
	public interface IAlertService : IService
	{
		IAlertState GetAlertByType(AlertType alertType);
	}

	public class AlertService : IAlertService
	{
		private Dictionary<AlertType, IAlertState> _alertCached = new Dictionary<AlertType, IAlertState>();

		private IAlertFactory _alertFactory;

		[Inject]
		public AlertService(IAlertFactory alertFactory)
		{
			_alertFactory = alertFactory;

			_alertCached.Clear();

			foreach (var type in (AlertType[]) Enum.GetValues(typeof(AlertType)))
			{
				_alertCached.Add(type, _alertFactory.Create(type));
			}
		}

		public IAlertState GetAlertByType(AlertType alertType)
		{
			if (_alertCached.ContainsKey(alertType))
			{
				return _alertCached[alertType];
			}

			return null;
		}
	}
}