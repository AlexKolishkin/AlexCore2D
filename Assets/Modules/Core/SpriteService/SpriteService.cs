using System;
using System.Collections.Generic;
using Core.Addressable;
using Core.View;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Zenject;

namespace Core.Sprites
{
	public class SpriteService : ISpriteService
	{
		private IAddressableService _addressableService;

		[Inject]
		public SpriteService(IAddressableService addressableService)
		{
			_addressableService = addressableService;
		}
		
		private Dictionary<AtlasType, string> Paths = new Dictionary<AtlasType, string>
		{
			{AtlasType.ResourcesIcon, "ResourceIcons"},
		};

		public void SetSprite(Image image, AtlasType atlasType, string key)
		{
			LoadSpriteFromAtlas(image, atlasType, key);
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

		#region ResourceIcons

		private SpriteAtlas _resourceIconsAtlas;

		public void SetSprite(Image image, ResourceIconType type)
		{
			SetSprite(image, AtlasType.ResourcesIcon, type.GetSpriteName());
		}

		#endregion

		#region Implementation

		private void LoadSpriteFromAtlas(Image image, AtlasType atlasType, string spriteName, string defaultSprite = "",
			Action<Sprite> onResult = null)
		{
			var atlas = GetAtlas(atlasType);

			if (!image)
			{
				onResult?.Invoke(_missingSprite);
				return;
			}

			if (!atlas)
			{
				var color = image.color;
				image.color = new Color(1, 1, 1, 0);

				var path = Paths[atlasType];

				_addressableService.LoadAsync<SpriteAtlas>(path, onComplete: handle =>
				{
					Sprite resultSprite = _missingSprite;

					if (handle.IsValid())
					{
						atlas = handle.Result;
					}

					if (atlas != null)
					{
						SetAtlas(atlas, atlasType);
						resultSprite = atlas.GetSprite(spriteName);
						if (resultSprite == null && !string.IsNullOrEmpty(defaultSprite))
						{
							resultSprite = atlas.GetSprite(defaultSprite);
						}

						if (resultSprite == null)
						{
							resultSprite = _missingSprite;
						}
					}

					image.sprite = resultSprite;
					image.color = color;
					onResult?.Invoke(resultSprite);
				});
			}
			else
			{
				var sprite = atlas.GetSprite(spriteName);
				if (sprite == null && string.IsNullOrEmpty(defaultSprite))
				{
					sprite = atlas.GetSprite(defaultSprite);
				}

				if (sprite == null)
				{
					sprite = _missingSprite;
				}

				image.sprite = sprite;
				onResult?.Invoke(sprite);
			}
		}

		private void PreloadAtlas(AtlasType atlasType)
		{
			var atlas = GetAtlas(atlasType);
			var path = Paths[atlasType];
			if (atlas == null)
			{
				_addressableService.LoadAsync<SpriteAtlas>(path, handle =>
				{
					if (handle.IsValid())
					{
						atlas = handle.Result;
					}

					SetAtlas(atlas, atlasType);
				});
			}
		}

		private Sprite GetSpriteFromAtlas(SpriteAtlas atlas, string spriteName)
		{
			if (atlas == null)
			{
				Debug.LogError($"{spriteName}: SpriteAtlas is null");
				return _missingSprite;
			}

			var sprite = atlas.GetSprite(spriteName);

			if (Application.isEditor && !sprite)
			{
				Debug.LogError($"No |{spriteName}| sprite in {atlas.name}");
				return _missingSprite;
			}

			return sprite;
		}

		private SpriteAtlas GetAtlas(AtlasType atlasType)
		{
			switch (atlasType)
			{
				case AtlasType.ResourcesIcon: return _resourceIconsAtlas;
			}

			return null;
		}

		private void SetAtlas(SpriteAtlas atlas, AtlasType atlasType)
		{
			switch (atlasType)
			{
				case AtlasType.ResourcesIcon:
					_resourceIconsAtlas = atlas;
					break;
			}
		}

		#endregion
	}

	public enum AtlasType
	{
		ResourcesIcon,
	}
}