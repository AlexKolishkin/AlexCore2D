using Core.Attributes;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using Core;
using Object = UnityEngine.Object;

namespace Core
{
	public class LifeCycleService : ILifeCycleService, IInitializable, IAutoPersistent
	{
		public Subject<bool> ApplicationFocusStream { get; } = new Subject<bool>();
		public ReactiveProperty<bool> ApplicationPause { get; } = new ReactiveProperty<bool>();
		public Subject<bool> ApplicationQuitStream { get; } = new Subject<bool>();
		public Subject<DateTime> SecondTick { get; } = new Subject<DateTime>();
		public Subject<DateTime> HalfMinuteTick { get; } = new Subject<DateTime>();

		public Subject<float> LateUpdateStream { get; } = new Subject<float>();
		public Subject<float> OnGUIStream { get; } = new Subject<float>();
		public Subject<float> UpdateStream { get; } = new Subject<float>();

		// Time
		[Persistent] private DateTime _logOutTime;
		public DateTime LogOutTime { get; private set; } = new DateTime();
		public TimeSpan OfflineSpan { get; private set; } = new TimeSpan(1);
		public ReactiveProperty<bool> IsReadyGameTime { get; private set; } = new ReactiveProperty<bool>();
		public DateTime GameDateTime { get; private set; } = new DateTime();

		private ITimeProvider _timeProvider = new LocalTimeProvider();
		private ICoroService _coroutineService;
		private float _updateTimePeriod = 10f;

		public LifeCycleService(ICoroService coroutineService)
		{
			_coroutineService = coroutineService;
		}

		public void Initialize()
		{
			var go = new GameObject("GameEvents");
			Object.DontDestroyOnLoad(go);
			var behaviour = go.AddComponent<GameEventsBehaviour>();
			behaviour.LifeCycleService = this;
			IsReadyGameTime.Value = false;
			Debug.Log("Initialize LifeCycleService");
		}

		public void OnLoaded()
		{
			LogOutTime = _logOutTime;
			_coroutineService.Run(SetTime(_updateTimePeriod));

			GameDateTime = _timeProvider.Get();
			OfflineSpan = GameDateTime - LogOutTime;
			
			IsReadyGameTime.Value = true;
		}

		private IEnumerator SetTime(float pause)
		{
			while (true)
			{
				GameDateTime = _timeProvider.Get();
				_logOutTime = GameDateTime;
				yield return new WaitForSecondsRealtime(pause);
			}
		}
	}
}