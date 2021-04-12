namespace Core.Sprites
{
	public class ResourceIconsFromAtlas : SpriteFromAtlas
	{
		public ResourceIconType IconType;

		private void OnEnable()
		{
			SpriteService.SetSprite(Image, IconType);
		}
	}
}