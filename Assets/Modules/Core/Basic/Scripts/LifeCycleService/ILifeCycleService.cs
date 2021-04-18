using System;
using UniRx;

namespace Core
{
	public interface ILifeCycleService : IService
	{
		Subject<bool> ApplicationFocusStream { get; }
		ReactiveProperty<bool> ApplicationPause { get; }
		Subject<bool> ApplicationQuitStream { get; }
		Subject<DateTime> OneSecondTick { get; }
		Subject<DateTime> HalfMinuteTick { get; }
		Subject<float> LateUpdateStream { get; }
		Subject<float> OnGUIStream { get; }
		Subject<float> UpdateStream { get; }

		DateTime LogOutTime { get; }
		TimeSpan OfflineSpan { get; }
		DateTime GameDateTime { get; }
		ReactiveProperty<bool> IsReadyGameTime { get; }
	}
}