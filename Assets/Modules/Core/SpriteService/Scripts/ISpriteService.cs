using UnityEngine.UI;

namespace Core.Sprites
{
	public interface ISpriteService : IService
	{
		void SetSprite(Image image, AtlasType atlasType, string key);
		void SetSprite(Image image, ResourceIconType type);
	}
}