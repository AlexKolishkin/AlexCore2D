using System;
using System.Collections.Generic;
using Core.Addressable;
using Core.View;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Zenject;

namespace Core.Sprites
{
	public class SpriteService : ISpriteService
	{
		private AddressableCache<Sprite> _spriteCache;

		private IAddressableService _addressableService;

		private Dictionary<AtlasType, SpriteAtlasData> _atlasesData = new Dictionary<AtlasType, SpriteAtlasData>();

		[Inject]
		public SpriteService(IAddressableService addressableService)
		{
			_addressableService = addressableService;
			_spriteCache = new AddressableCache<Sprite>(_addressableService);

			SetupAtlasesData();
		}

		private void SetupAtlasesData()
		{
			_atlasesData[AtlasType.CurrencyIcon] = new SpriteAtlasData("CurrencySpriteAtlas");
		}

		public IDisposable SetSpriteSingle(Image image, string key)
		{
			SetAloneSprite(image, key);
			return Disposable.Create(() => { _spriteCache.Release(key); });
		}

		public void SetSprite(Image image, CurrencyIconType iconType)
		{
			LoadSpriteFromAtlas(image, AtlasType.CurrencyIcon, iconType.GetSpriteName());
		}

		#region Missing sprite

		private const string _missingSpritePath = "MissingSprite";
		private Sprite _missingCachedSprite;

		private bool IsMissingSprite(Sprite sprite)
		{
			return !sprite || sprite == _missingSprite;
		}

		private Sprite _missingSprite
		{
			get
			{
				if (_missingCachedSprite == null)
				{
					_missingCachedSprite = Resources.Load<Sprite>(_missingSpritePath);
				}

				return _missingCachedSprite;
			}
		}

		#endregion

		#region Implementation

		private async void LoadSpriteFromAtlas(Image image, AtlasType atlasType, string spriteName, string defaultSprite = "")
		{
			if (!_atlasesData.ContainsKey(atlasType))
			{
				Debug.LogError($"AtlasData for atlasType [{atlasType}] is null");
				return;
			}

			SpriteAtlasData atlasData = _atlasesData[atlasType];

			if (atlasData.Handle == null)
			{
				LoadAtlas(atlasType);
			}

			await atlasData.Handle.Task;

			image.sprite = GetSpriteFromAtlas(atlasData.Atlas, spriteName, defaultSprite);
		}
	

		private async void SetAloneSprite(Image image, string key)
		{
			var sprite = await _spriteCache.GetItem(key);
			var color = image.color;
			image.color = new Color(1, 1, 1, 0);
			if (image != null)
			{
				image.sprite = sprite;
				image.color = color;
			}
		}

		private void LoadAtlas(AtlasType atlasType)
		{
			var atlasData = _atlasesData[atlasType];
			if (atlasData.Handle == null)
			{
				_atlasesData[atlasType].Handle = _addressableService.LoadAsync<SpriteAtlas>(atlasData.Path);
			}
		}

		private Sprite GetSpriteFromAtlas(SpriteAtlas atlas, string spriteName, string defaultSpriteName = "")
		{
			if (atlas == null)
			{
				Debug.LogError($"{spriteName}: SpriteAtlas is null");
				return _missingSprite;
			}

			var sprite = atlas.GetSprite(spriteName);

			if (sprite != null)
			{
				return sprite;
			}
			else
			{
				Debug.LogError($"No |{spriteName}| sprite in {atlas.name}");

				if (!string.IsNullOrEmpty(defaultSpriteName))
				{
					var defaultSprite = atlas.GetSprite(defaultSpriteName);

					if (defaultSprite != null)
					{
						return defaultSprite;
					}
					else
					{
						Debug.LogError($"No default sprite |{defaultSpriteName}| in {atlas.name}");
					}
				}
			}
			return _missingSprite;
		}

		#endregion
	}

	public enum AtlasType
	{
		CurrencyIcon
	}
}