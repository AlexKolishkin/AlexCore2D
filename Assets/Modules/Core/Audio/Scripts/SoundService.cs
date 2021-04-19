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

		void Play(string path, AudioParameters audioParameters = null);
	}

	public class SoundService : ISoundService
	{
		private const string _baseSourceName = "Sound Service Source";

		private AddressableCache<AudioClip> _audioClipCache;

		public ReactiveProperty<bool> MuteSound { get; } = new ReactiveProperty<bool>();

		public const int MaxSoundCount = 10;
		Stack<AudioPlayer> _sourcePool = new Stack<AudioPlayer>();

		private AudioParameters _defaultParameters = new AudioParameters(false, 0.5f, 0.2f);

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

			_audioClipCache = new AddressableCache<AudioClip>(_addressableService);
		}

		public void Play(string path, AudioParameters audioParameters = null) 
		{
			if (audioParameters != null)
			{
				PlayAsync(path, audioParameters);
			}
			else
			{
				PlayAsync(path, _defaultParameters);
			}
		}

		private async void PlayAsync(string path, AudioParameters audioParameters)
		{
			if (!IsPlayAccessible())
			{
				return;
			}

			AudioPlayer audioPlayer = _sourcePool.Pop();

			var clip = await _audioClipCache.GetAddressable(path);

			audioPlayer.Play(clip, audioParameters);

			await ReleaseAfterPlayed(audioPlayer);
		}

		private async Task ReleaseAfterPlayed(AudioPlayer audioPlayer)
		{
			while (audioPlayer.IsPlaying)
			{
				await Task.Yield();
			}

			_audioClipCache.Release(audioPlayer.ClipName);
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