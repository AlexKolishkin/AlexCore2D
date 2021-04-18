using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Addressable;
using UnityEngine;
using Object = UnityEngine.Object;
using UniRx.Async;
namespace Core.StaticData
{
	public class LoadingResourceRepository<T, V> : ILoadingRepository<T, V> where V : Object
	{
		public delegate bool PrefabInspection(GameObject obj);

		private readonly Dictionary<T, V> prefabCache = new Dictionary<T, V>();
		private readonly Dictionary<T, string> prefabPathDictionary = new Dictionary<T, string>();


		public LoadingResourceRepository()
		{
			prefabPathDictionary.Clear();
		}

		public async Task<V> LoadPrefab(T key)
		{
			if (prefabCache.ContainsKey(key))
			{
				return null;
			}

			var load = Resources.LoadAsync<V>(prefabPathDictionary[key]);
			await load;

			var loaded = load.asset as V;
			if (!prefabCache.ContainsKey(key))
			{
				prefabCache.Add(key, loaded);
			}

			return loaded;
		}

		public void UnloadPrefab(T key)
		{
			Debug.Log($"UnloadPrefab {key}");
		}

		public bool HasPrefab(T key)
		{
			return prefabCache.ContainsKey(key);
		}

		public void AddPath(T key, string path)
		{
			prefabPathDictionary[key] = path;
		}
	}

	public class AddressableLoadingResourceRepository<T, V> : ILoadingRepository<T, V> where V : Object
	{
		private readonly Dictionary<T, V> _prefabCache = new Dictionary<T, V>();
		private readonly Dictionary<T, string> _prefabPathDictionary = new Dictionary<T, string>();
		private readonly Dictionary<T, List<AddressableUtils.AsyncOperationHandleDisposable<V>>> _collectedOperations
			= new Dictionary<T, List<AddressableUtils.AsyncOperationHandleDisposable<V>>>();

		private IAddressableService _addressableService;

		public AddressableLoadingResourceRepository(IAddressableService addressableService)
		{
			_addressableService = addressableService;
		}

		public IEnumerator LoadPrefab(T key, Action<V> onComplete)
		{
			if (_prefabCache.ContainsKey(key))
			{
				yield break;
			}

			AddressableUtils.AsyncOperationHandleDisposable<V> operation
				= _addressableService.LoadAsync<V>(_prefabPathDictionary[key]);

			yield return operation.WaitDone();

			if (!_prefabCache.ContainsKey(key))
			{
				_prefabCache.Add(key, operation.Result);
			}

			onComplete.Invoke(operation.Result);
			CollectOperation(key, operation);
		}

		public async Task<V> LoadPrefab(T key)
		{
			if (_prefabCache.ContainsKey(key))
			{
				return null;
			}

			AddressableUtils.AsyncOperationHandleDisposable<V> operation
				= _addressableService.LoadAsync<V>(_prefabPathDictionary[key]);

			await operation.Task;

			if (!_prefabCache.ContainsKey(key))
			{
				_prefabCache.Add(key, operation.Result);
			}

			CollectOperation(key, operation);

			return operation.Result;
		}

		public void UnloadPrefab(T key)
		{
			Release(key);
			_prefabCache.Remove(key);
		}


		public bool HasPrefab(T key)
		{
			return _prefabCache.ContainsKey(key);
		}

		public void AddPath(T key, string path)
		{
			_prefabPathDictionary[key] = path;
		}

		private void CollectOperation(T key, AddressableUtils.AsyncOperationHandleDisposable<V> operation)
		{
			if (_collectedOperations.ContainsKey(key))
			{
				_collectedOperations[key].Add(operation);
			}
			else
			{
				_collectedOperations.Add(key, new List<AddressableUtils.AsyncOperationHandleDisposable<V>>());
				_collectedOperations[key].Add(operation);
			}
		}

		public void Release(T key)
		{
			if (_collectedOperations.ContainsKey(key))
			{
				var operations = _collectedOperations[key];
				foreach (var operation in operations.Where(operation => operation.IsValid()))
				{
					operation.Dispose();
				}
				_collectedOperations[key] = new List<AddressableUtils.AsyncOperationHandleDisposable<V>>();
			}
		}
	}
}