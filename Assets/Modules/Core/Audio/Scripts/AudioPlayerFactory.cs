using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Audio
{
	public interface IAudioPlayerFactory
	{
		AudioPlayer Create(string nameBase);
	}


	public class AudioPlayerFactory : IAudioPlayerFactory
	{
		private const string _audioPlayerPrefabPath = "AudioPlayerPrefab";

		private int _PlayersCount = 0;

		private AudioPlayer _audioPlayerCached;

		private GameObject _audioGoParent;

		private AudioPlayer _audioPlayerPrefab
		{
			get
			{
				if (_audioPlayerCached == null)
				{
					_audioPlayerCached = Resources.Load<AudioPlayer>(_audioPlayerPrefabPath);
				}

				return _audioPlayerCached;
			}
		}
		
		public AudioPlayer Create(string nameBase)
		{
			if (!_audioGoParent)
			{
				_audioGoParent = new GameObject("Audio");
				Object.DontDestroyOnLoad(_audioGoParent);
			}

			var gameObject = GameObject.Instantiate(_audioPlayerPrefab, _audioGoParent.transform);

			gameObject.name = nameBase + " " + _PlayersCount;

			_PlayersCount++;

			return gameObject.GetComponent<AudioPlayer>();
		}
	}

}