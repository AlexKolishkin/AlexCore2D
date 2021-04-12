using System.Collections.Generic;
using Core.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Alert
{
	public class AlertView : GameBehaviour
	{
		public Sprite NewSprite, UpdateSprite, SuccessSprite, WarningSprite;
		public Image IconImage;
		public GameObject Content;

		[Inject] private AlertService _alertService;

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
				IconImage.sprite = GetSpriteByType(alertType);
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
					IconImage.sprite = GetSpriteByType(alertType);

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

		private Sprite GetSpriteByType(AlertStateType alertStateType)
		{
			switch (alertStateType)
			{
				case AlertStateType.New:
					return NewSprite;
				case AlertStateType.Update:
					return UpdateSprite;
				case AlertStateType.Finish:
					return SuccessSprite;
				case AlertStateType.Warning:
					return WarningSprite;
			}

			return null;
		}
	}
}