using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UniRx.Async;

namespace Core.Audio
{
	public class AudioPlayer : MonoBehaviour
	{
		[SerializeField] private AudioSource _audioSource;

		private AudioParameters _audioParameters;

		public void Play(AudioClip clip, AudioParameters audioParameters)
		{
			_audioParameters = audioParameters;
			_audioSource.clip = clip;
			_audioSource.loop = audioParameters.Loop;
			_audioSource.Play();
		}

		public async Task FadeVolume(float volume)
		{
			await _audioSource.DOFade(volume, _audioParameters.FadeTime).AsyncWaitForCompletion();
		}

		public async UniTask FadeVolume(float volume, CancellationToken token)
		{
			await _audioSource.DOFade(volume, _audioParameters.FadeTime).AsTask(token);
		}

		public bool IsPlaying => _audioSource != null && _audioSource.isPlaying;

		public string ClipName => _audioSource.clip ? _audioSource.clip.name : "";

		public void SetMute(bool isMuted)
		{
			_audioSource.mute = isMuted;
		}
	}
}