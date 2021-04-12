using Core.Addressable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System.Threading.Tasks;
using static Core.Addressable.AddressableUtils;

namespace Core.Audio
{
	public class AudioClipCache 
	{
		private Dictionary<string, AsyncOperationHandleDisposable<AudioClip>> _clipCached = new Dictionary<string, AsyncOperationHandleDisposable<AudioClip>>();

		private Dictionary<string, int> _clipCachedCount = new Dictionary<string, int>();

		private IAddressableService _addressableService;

		public AudioClipCache (IAddressableService addressableService)
		{
			_addressableService = addressableService;
		}

		public async Task<AudioClip> GetClip(string clipPath)
		{
			if (_clipCachedCount.ContainsKey(clipPath))
			{
				_clipCachedCount[clipPath]++;
			}
			else
			{
				_clipCachedCount[clipPath] = 1;
			}

			if (!_clipCached.ContainsKey(clipPath) || _clipCached[clipPath] == null)
			{
				_clipCached[clipPath] = _addressableService.LoadAsync<AudioClip>(clipPath);
			}

			AsyncOperationHandleDisposable<AudioClip> _handle = _clipCached[clipPath];

			await _handle.Task;

			return _handle.Result;
		}


		public void ReleaseClip(string clipPath)
		{
			if (clipPath == "")
			{
				return;
			}

			_clipCachedCount[clipPath]--;


			if (_clipCachedCount[clipPath] == 0)
			{
				_clipCached[clipPath].Dispose();
				_clipCached[clipPath] = null;
			}
		}
	}
}