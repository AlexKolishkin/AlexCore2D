using Core.Addressable;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Audio
{
	public interface IMusicService : IService
	{
		ReactiveProperty<bool> MuteMusic { get; }
		
		void Play(string path, AudioParameters audioParameters = null);

		void Play(IEnumerable<string> paths);
	}

	public class MusicService : IMusicService
	{
		private CompositeDisposable _collector = new CompositeDisposable();

		private const string _baseSourceName = "Music Service Source";

		private AddressableCache<AudioClip> _audioClipCache;

		private AudioPlayer _audioPlayer;

		private AudioParameters _defaultLoopParameters = new AudioParameters(true, 0.5f, 0.2f);
		private AudioParameters _defaultParameters = new AudioParameters(false, 0.5f,  0.2f);

		private List<string> _musicTracks = new List<string>();
		private int _currentTrackIndex;
		public ReactiveProperty<bool> MuteMusic { get; } = new ReactiveProperty<bool>();

		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		private readonly INextClipProvider _nextClipProvider = new NextInListClipProvider();

		private IAddressableService _addressableService;
		private IAudioPlayerFactory _audioPlayerFactory;
		private ILifeCycleService _lifeCycleService;

		[Inject]
		public MusicService(IAudioPlayerFactory audioPlayerFactory, IAddressableService addressableService, ILifeCycleService lifeCycleService)
		{
			_audioPlayerFactory = audioPlayerFactory;
			_addressableService = addressableService;
			_lifeCycleService = lifeCycleService;

			Init();
		}

		private void Init()
		{
			_audioPlayer = _audioPlayerFactory.Create(_baseSourceName);

			_audioClipCache = new AddressableCache<AudioClip>(_addressableService);

			MuteMusic.Subscribe(_audioPlayer.SetMute);

			_lifeCycleService.ApplicationQuitStream.Subscribe(val => RefreshToken());
		}

		private void RefreshToken()
		{
			TaskExtension.RefreshToken(ref _cancellationTokenSource);
		}

		public async void Play(string path, AudioParameters audioParameters = null)
		{
			RefreshToken();
			if (audioParameters != null)
			{
				await PlayAsync(path, audioParameters, _cancellationTokenSource.Token);
			}
			else
			{
				await PlayAsync(path, _defaultLoopParameters, _cancellationTokenSource.Token);
			}
		}

		public async void Play(IEnumerable<string> paths)
		{
			RefreshToken();

			_musicTracks.Clear();
			_currentTrackIndex = 0;

			foreach (var path in paths)
			{
				_musicTracks.Add(path);
			}

			await PlayNextTrack(_cancellationTokenSource.Token);
		}


		private async Task PlayNextTrack(CancellationToken token)
		{
			try
			{
				_currentTrackIndex = _nextClipProvider.GetNextIndex(_currentTrackIndex, _musicTracks);

				await PlayAsync(_musicTracks[_currentTrackIndex], _defaultParameters, token);

				token.ThrowIfCancellationRequested();

				while (_audioPlayer.IsPlaying)
				{
					await Task.Delay(1000, token);
				}

				await PlayNextTrack(token);
			}
			catch (OperationCanceledException) { }
		}

		private async Task PlayAsync(string path, AudioParameters audioParameters, CancellationToken token)
		{
			try
			{
				if (_audioPlayer.IsPlaying)
				{
					await _audioPlayer.FadeVolume(0, token);
				}

				_audioClipCache.Release(_audioPlayer.ClipName);

				var clip = await _audioClipCache.GetAddressable(path);

				token.ThrowIfCancellationRequested();

				_audioPlayer.Play(clip, audioParameters);

				await _audioPlayer.FadeVolume(audioParameters.Volume, token);

			}
			catch (OperationCanceledException) { }
		}
	}

	public interface INextClipProvider
	{
		int GetNextIndex(int current, List<string> all);
	}

	public class NextInListClipProvider : INextClipProvider
	{
		public int GetNextIndex(int current, List<string> all)
		{
			var nextIndex = ++current;

			return nextIndex >= all.Count ? 0 : nextIndex;
		}
	}

	public class RandomClipProvider : INextClipProvider
	{
		public int GetNextIndex(int current, List<string> all) => UnityEngine.Random.Range(0, all.Count);
	}
}