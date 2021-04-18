using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Addressable
{
	public class AddressableRemoteDownloader
	{
		public ReactiveProperty<bool> Loaded = new ReactiveProperty<bool>(false);

		public AddressableDownloadLabelData[] RemoteLabels = {
			new AddressableDownloadLabelData("remote_view_1"),
		};

		private const int Mb = 1048576;
		private float _progress = 0;
		private double _fullSize = 25;
		private double _downloadedSize;
		private AddressableDownloadLabelData _currentLabel;
		
		public void Synchronization(Action onComplete = null)
		{
			MainThreadDispatcher.StartCoroutine(SynchronizationAssets(onComplete));
		}

		public void DownloadByLabel(string label)
		{
			var downloadLabelData = RemoteLabels.FirstOrDefault(val => val.Label == label);
			MainThreadDispatcher.StartCoroutine(DownloadInner(downloadLabelData));
		}

		public AddressableDownloadLabelData GetCurrentDownloading()
		{
			return _currentLabel;
		} 
		
		private IEnumerator SynchronizationAssets(Action onComplete = null)
		{

			foreach (var label in RemoteLabels)
			{
				if (!label.IsLoaded)
				{
					yield return DownloadInner(label);
				}
			}

			onComplete?.Invoke();
			Loaded.Value = true;
		}

		public AddressableDownloadProgress GetProgress()
		{
			return new AddressableDownloadProgress(GetDownloadProgress(), GetDownloadProgressString(), _currentLabel.Label);
		}
		
		public string GetDownloadProgressString()
		{
			var currentMb = (float) _downloadedSize / Mb;

			if (_fullSize > 0)
			{
				return $"{(int) currentMb} / {_fullSize} MB";
			}

			return $"{(int) currentMb} MB";
		}

		public float GetDownloadProgress()
		{
			return _progress;
		}

		private IEnumerator DownloadInner(AddressableDownloadLabelData downloadLabelData)
		{
			Debug.Log($"Addressable start to download {downloadLabelData.Label}");
			_currentLabel = downloadLabelData;
			
			var locations = Addressables.LoadResourceLocationsAsync(downloadLabelData.Label);
			yield return locations;

			var getDownloadSize = Addressables.GetDownloadSizeAsync(locations.Result);
			yield return getDownloadSize;
			_fullSize = (int)((float) getDownloadSize.Result / Mb);
			Debug.Log($"Addressable {downloadLabelData.Label} download size: {_fullSize}");
			
			if (_fullSize > 0)
			{
				var downloadLocations = Addressables.DownloadDependenciesAsync(locations.Result);
				while (!downloadLocations.IsDone)
				{
					_progress = downloadLocations.PercentComplete;
					_downloadedSize = (int)(_progress * _fullSize * Mb);
					downloadLabelData.SetProgress(_downloadedSize, _fullSize, _progress);
					//Debug.Log($"Addressable {label} downloading {_progress * 100}% size {_downloadedSize}");
					yield return new WaitForSeconds(0.01f);
				}
				downloadLabelData.SetDone();
			}
			else
			{
				Debug.Log($"Addressable already to downloaded {downloadLabelData.Label}");
				downloadLabelData.SetDone();
			}
		}
	}

	public struct AddressableDownloadProgress
	{
		public float Progress;
		public string ProgressString;
		public string Label;

		public AddressableDownloadProgress(float progress, string progressString, string label)
		{
			Progress = progress;
			ProgressString = progressString;
			Label = label;
		}
	}
	
	public struct AddressableDownloadLabelData
	{
		public readonly string Label;
		public bool IsLoaded { get; private set; }

		public double Current { get; private set; }
		public double Full { get; private set; }
		public float Progress { get; private set; }

		public AddressableDownloadLabelData(string label) : this()
		{
			Label = label;
			IsLoaded = false;
		}

		public void SetProgress(double current, double full, float progress)
		{
			Current = current;
			Full = full;
			Progress = progress;
		}

		public void SetDone()
		{
			IsLoaded = true;
		}
	}
}