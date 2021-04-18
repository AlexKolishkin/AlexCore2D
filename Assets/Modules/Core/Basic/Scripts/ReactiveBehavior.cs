using UniRx;
using UnityEngine;

namespace Core
{
	public class ReactiveBehavior : MonoBehaviour
	{
		public CompositeDisposable Collector = new CompositeDisposable();

		protected virtual void OnDestroy()
		{
			Collector.Dispose();
		}
	}
}