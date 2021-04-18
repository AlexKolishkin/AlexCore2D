using System;
using UnityEngine;

namespace Core
{
	public class GameEventsBehaviour : MonoBehaviour
	{
		[NonSerialized] public ILifeCycleService LifeCycleService;

		private float _halfMinuteSum;
		private float _secondSum;

		private void Update()
		{
			_secondSum += Time.deltaTime;
			_halfMinuteSum += Time.deltaTime;

			if (_secondSum >= 1)
			{
				_secondSum -= 1f;
				LifeCycleService.OneSecondTick.OnNext(DateTime.Now);
			}

			if (_halfMinuteSum >= 30)
			{
				_halfMinuteSum = 0;
				LifeCycleService.HalfMinuteTick.OnNext(DateTime.Now);
			}

			LifeCycleService.UpdateStream.OnNext(Time.deltaTime);
		}

		private void LateUpdate()
		{
			LifeCycleService.LateUpdateStream.OnNext(Time.deltaTime);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			LifeCycleService.ApplicationFocusStream.OnNext(hasFocus);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			LifeCycleService.ApplicationPause.Value = pauseStatus;
		}

		private void OnApplicationQuit()
		{
			LifeCycleService.ApplicationQuitStream.OnNext(true);
		}

		private void OnGUI()
		{
			LifeCycleService.OnGUIStream.OnNext(Time.deltaTime);
		}
	}
}
