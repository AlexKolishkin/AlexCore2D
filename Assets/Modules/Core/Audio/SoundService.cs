using Core.Addressable;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Audio
{
	public interface ISoundService : IService
    {
		ReactiveProperty<bool> MuteSound { get; }

		void Play(string path);

		void Play(string path, AudioParameters audioParameters);
	}

	public class SoundService : ISoundService
	{
		private const string _baseSourceName = "Sound Service Source";

		private AudioClipCache  _audioClipCache;

		public ReactiveProperty<bool> MuteSound { get; } = new ReactiveProperty<bool>();

		public const int MaxSoundCount = 10;
		Stack<AudioPlayer> _sourcePool = new Stack<AudioPlayer>();

		private AudioParameters _defaultParameters;


		private IAddressableService _addressableService;
		private IAudioPlayerFactory _audioPlayerFactory;

		[Inject]
		public SoundService(IAudioPlayerFactory audioPlayerFactory, IAddressableService addressableService)
		{
			_audioPlayerFactory = audioPlayerFactory;
			_addressableService = addressableService;

			Init();
		}

		public void Init()
		{
			for (int i = 0; i < MaxSoundCount; i++)
			{
				_sourcePool.Push(_audioPlayerFactory.Create(_baseSourceName));
			}

			_audioClipCache = new AudioClipCache (_addressableService);

			_defaultParameters.Loop = false;
			_defaultParameters.Volume = 0.5f;
			_defaultParameters.FadeTime = 0.2f;
		}

		public void Play(string path)
		{
			Play(path, _defaultParameters);
		}

		public void Play(string path, AudioParameters audioParameters)
		{
			PlayAsync(path, audioParameters);
		}

		private async void PlayAsync(string path, AudioParameters audioParameters)
		{
			if (!IsPlayAccessible())
			{
				return;
			}

			AudioPlayer audioPlayer = _sourcePool.Pop();

			var clip = await _audioClipCache.GetClip(path);

			audioPlayer.Play(clip, audioParameters);

			await ReleaseAfterPlayed(audioPlayer);
		}

		private async Task ReleaseAfterPlayed(AudioPlayer audioPlayer)
		{
			while (audioPlayer.IsPlaying)
			{
				await Task.Yield();
			}

			_audioClipCache.ReleaseClip(audioPlayer.ClipName);
			_sourcePool.Push(audioPlayer);
		}

		private bool IsPlayAccessible()
		{
			if (MuteSound.Value || _sourcePool.Count == 0)
			{
				if (_sourcePool.Count == 0 && Application.isEditor)
				{
					Debug.LogWarning("AudioSource pool is empty");
				}
				return false;
			}
			return true;
		}
	}
}