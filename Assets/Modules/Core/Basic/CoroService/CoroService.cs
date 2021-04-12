using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Core
{
	public class CoroService : ICoroService, IInitializable
	{
		private CoroBehaviour coroutineBehaviour;
		public void Initialize()
		{
			var go = new GameObject("CoroController");
			Object.DontDestroyOnLoad(go);
			coroutineBehaviour = go.AddComponent<CoroBehaviour>();
			Debug.Log("Initialize CoroutineService");
		}

		public Coro Run(IEnumerator coroutine, Action onComplete = null)
		{
			var coro = new Coro(coroutine, coroutineBehaviour);
			coro.OnCompleted(onComplete);
			return coro;
		}
	}

	public class CoroBehaviour : MonoBehaviour
	{
	}

	public class Coro : IDisposable
	{
		public readonly ReactiveProperty<bool> Completed = new ReactiveProperty<bool>();

		private readonly CoroBehaviour behaviour;
		private readonly Coroutine coro;

		public Coro(IEnumerator coroutine, CoroBehaviour behaviour)
		{
			coro = behaviour.StartCoroutine(Run(coroutine));
			this.behaviour = behaviour;
		}

		public void Dispose()
		{
			if (!Completed.Value)
			{
				behaviour.StopCoroutine(coro);
				Completed.Value = true;
			}
		}

		private event Action _onComplete;

		private IEnumerator Run(IEnumerator coroutine)
		{
			yield return coroutine;
			Completed.Value = true;
			_onComplete?.Invoke();
		}

		public void OnCompleted(Action action)
		{
			_onComplete += action;
		}
	}
}