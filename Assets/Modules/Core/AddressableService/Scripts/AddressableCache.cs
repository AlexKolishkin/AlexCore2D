using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Addressable
{
	public class AddressableCache<T>
	{

		private Dictionary<string, AddressableUtils.AsyncOperationHandleDisposable<T>> _cached =
			new Dictionary<string, AddressableUtils.AsyncOperationHandleDisposable<T>>();

		private Dictionary<string, int> _cachedCount = new Dictionary<string, int>();

		private IAddressableService _addressableService;

		public AddressableCache(IAddressableService addressableService)
		{
			_addressableService = addressableService;
		}

		public async Task<T> GetAddressable(string path)
		{
			if (_cachedCount.ContainsKey(path))
			{
				_cachedCount[path]++;
			}
			else
			{
				_cachedCount[path] = 1;
			}

			if (!_cached.ContainsKey(path) || _cached[path] == null)
			{
				_cached[path] = _addressableService.LoadAsync<T>(path);
			}

			AddressableUtils.AsyncOperationHandleDisposable<T> _handle = _cached[path];
			await _handle.Task;
			return _handle.Result;
		}

		public void Release(string path)
		{
			if (path == "")
			{
				return;
			}

			_cachedCount[path]--;

			if (_cachedCount[path] == 0)
			{
				_cached[path].Dispose();
				_cached[path] = null;
			}
		}
	}
}
