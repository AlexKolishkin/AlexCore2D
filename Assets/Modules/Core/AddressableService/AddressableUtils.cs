using System;
using System.Collections;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Addressable
{
	public static class AddressableUtils
	{
		public class AsyncOperationHandleDisposable : IDisposable
		{
			private AsyncOperationHandle _handle;

			public AsyncOperationHandleDisposable(AsyncOperationHandle handle)
			{
				_handle = handle;
			}
			public object Result => _handle.Result;
			public Task Task => _handle.Task;
			public bool IsDone => _handle.IsDone;

			public bool IsValid()
			{
				return _handle.IsValid();
			}
			public void Dispose()
			{
				if (_handle.IsValid())
				{
					Addressables.Release(_handle);
				}
			}
		}

		public class AsyncOperationHandleDisposable<T> : IDisposable
		{
			private AsyncOperationHandle<T> _handle;

			public AsyncOperationHandleDisposable(AsyncOperationHandle<T> handle)
			{
				_handle = handle;
			}

			public T Result => _handle.Result;
			public Task<T> Task => _handle.Task;
			
			public bool IsDone => _handle.IsDone;

			public string DebugName => _handle.DebugName;


			public bool IsValid()
			{
				return _handle.IsValid();
			}

			public void Dispose()
			{
				Debug.Log($"AddressableService Release {_handle.DebugName}");
				if (_handle.IsValid())
				{
					Addressables.Release(_handle);
				}
			}
		}

		public static void AddTo(this AsyncOperationHandle handle, CompositeDisposable collector)
		{
			if (collector != null && handle.IsValid())
			{
				new AsyncOperationHandleDisposable(handle).AddTo(collector);
			}
		}

		public static void AddTo<T>(this AsyncOperationHandle<T> handle, CompositeDisposable collector)
		{
			if (collector != null && handle.IsValid())
			{
				new AsyncOperationHandleDisposable(handle).AddTo(collector);
			}
		}

		public static IEnumerator WaitDone(this AsyncOperationHandle handle)
		{
			if (handle.IsValid())
			{ 
				yield return new WaitWhile(() => !handle.IsDone);
			}
		}

		public static IEnumerator WaitDone<T>(this AsyncOperationHandle<T> handle)
		{
			if (handle.IsValid())
			{
				yield return new WaitWhile(() => !handle.IsDone);
			}
		}

		public static IEnumerator WaitDone(this AsyncOperationHandleDisposable handle)
		{
			if (handle.IsValid())
			{
				yield return new WaitWhile(predicate: () => !handle.IsDone);
			}
		}

		public static IEnumerator WaitDone<T>(this AsyncOperationHandleDisposable<T> handle)
		{
			if (handle.IsValid())
			{
				yield return new WaitWhile(() => !handle.IsDone);
			}
		}
	}
}