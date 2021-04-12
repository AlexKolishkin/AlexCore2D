using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ReactiveUtils
{
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

	private class UnityActionDisposable : IDisposable
	{
		public UnityAction action;
		public UnityEvent e;

		public void Dispose()
		{
			if (e != null)
			{
				e.RemoveListener(action);
				e = null;
				action = null;
			}
		}
	}

	private class UnityActionDisposable<T> : IDisposable
	{
		public UnityAction<T> action;
		public UnityEvent<T> e;

		public void Dispose()
		{
			if (e != null)
			{
				e.RemoveListener(action);
				e = null;
				action = null;
			}
		}
	}
}