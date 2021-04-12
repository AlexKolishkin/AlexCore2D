using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Core.Addressable.AddressableUtils;

namespace Core.Addressable
{
	public interface IAddressableService : IService
	{
		AsyncOperationHandleDisposable<T> LoadAsync<T>(string address, Action<AsyncOperationHandle<T>> onComplete = null);

		void Release<T>(AsyncOperationHandleDisposable<T> operationHandle);

		void SynchronizationWithRemote();

		AddressableDownloadLabelData GetCurrentDownloading();
	}

}