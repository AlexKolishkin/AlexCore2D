using System.Collections.Generic;
using Core.Sprites;
using Core.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Alert
{
	public class AlertView : GameBehaviour
	{
		public Image IconImage;
		public GameObject Content;

		public const string NewSpriteName = "icon_notification_new"; 
		public const string UpdateSpriteName = "icon_notification_update"; 
		public const string SuccessSpriteName = "icon_notification_update";
		public const string WarningSpriteName = "icon_notification_update";

		[Inject] private AlertService _alertService;
		[Inject] private SpriteService _spriteService;

		private bool _isInit;

		protected override void Awake()
		{
			base.Awake();
			if (!_isInit)
			{
				_isInit = true;
			}
		}

		public AlertStateType Check(AlertType type, object param = null)
		{
			if (!Content)
			{
				return AlertStateType.None;
			}

			if (!_isInit)
			{
				Awake();
			}

			var alertData = _alertService.GetAlertByType(type);
			if (alertData != null)
			{
				var alertType = alertData.GetAlertState(param);
				Content.SetActive(alertType != AlertStateType.None);
				SetSpriteByType(alertType);
				return alertType;
			}

			Debug.LogError("Couldn't find " + type + " in Alert Service");
			Content.SetActive(false);
			return AlertStateType.None;
		}

		public void CheckAny(List<AlertType> types)
		{
			if (!_isInit) Awake();
			foreach (var type in types)
			{
				var alertData = _alertService.GetAlertByType(type);
				if (alertData != null)
				{
					var alertType = alertData.GetAlertState();
					Content.SetActive(alertType != AlertStateType.None);
					SetSpriteByType(alertType);

					if (alertType != AlertStateType.None)
					{
						break;
					}
				}
				else
				{
					Debug.LogError("Couldn't find {0} in Alert Service");
					Content.SetActive(false);
				}
			}
		}

		private void SetSpriteByType(AlertStateType alertStateType)
		{
			switch (alertStateType)
			{
				case AlertStateType.New:
					_spriteService.SetSpriteSingle(IconImage, NewSpriteName);
					break;
				case AlertStateType.Update:
					_spriteService.SetSpriteSingle(IconImage, UpdateSpriteName);
					break;
				case AlertStateType.Finish:
					_spriteService.SetSpriteSingle(IconImage, SuccessSpriteName);
					break;
				case AlertStateType.Warning:
					_spriteService.SetSpriteSingle(IconImage, WarningSpriteName);
					break;
			}
		}
	}
}