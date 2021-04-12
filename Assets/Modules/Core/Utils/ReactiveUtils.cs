using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Utils
{
	public static class ReactiveUtils
	{
		public static IDisposable BindClick(this Button button, Action action)
		{
			return button.OnClickAsObservable().Subscribe(val => { action.Invoke(); });
		}

		public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour)
			where T : Component
		{
			var component = monoBehaviour.gameObject.GetComponent<T>();
			if (component == null) component = monoBehaviour.gameObject.AddComponent<T>();

			return component;
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject)
			where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if (component == null) component = gameObject.AddComponent<T>();

			return component;
		}
	}
}