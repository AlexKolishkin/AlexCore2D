using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Core.Addressable.AddressableUtils;

namespace Core.Addressable
{
	public sealed class AddressableService : IAddressableService
	{
		public AddressableRemoteDownloader RemoteDownloader;
	
		private AddressableService()
		{
			RemoteDownloader = new AddressableRemoteDownloader();
			Debug.Log("AddressableService Initialize");
		}

		public AsyncOperationHandleDisposable<T> LoadAsync<T>(string address, Action<AsyncOperationHandle<T>> onComplete = null)
		{
			Debug.Log($"AddressableService LoadAsync {address}");
			var result = Addressables.LoadAssetAsync<T>(address);
			result.Completed += onComplete;
			return new AsyncOperationHandleDisposable<T>(result);
		}
	
		public void Release<T>(AsyncOperationHandleDisposable<T> operationHandle)
		{
			Debug.Log($"AddressableService Release {operationHandle.DebugName}");
			if (operationHandle.IsValid())
			{
				Addressables.Release(operationHandle);
			}
		}

		public void SynchronizationWithRemote()
		{
			RemoteDownloader.Synchronization();
		}

		public AddressableDownloadLabelData GetCurrentDownloading()
		{
			return RemoteDownloader.GetCurrentDownloading();
		}
	}
}