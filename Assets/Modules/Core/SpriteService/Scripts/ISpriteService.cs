using System;
using UnityEngine.UI;

namespace Core.Sprites
{
	public interface ISpriteService : IService
	{
		IDisposable SetSpriteSingle(Image image, string key);

		void SetSprite(Image image, CurrencyIconType iconType);
	}
}