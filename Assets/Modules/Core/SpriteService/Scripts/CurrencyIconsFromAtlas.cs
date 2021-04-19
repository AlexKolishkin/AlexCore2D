namespace Core.Sprites
{
	public class CurrencyIconsFromAtlas : SpriteFromAtlas
	{
		public CurrencyIconType IconType;

		private void OnEnable()
		{
			SpriteService.SetSprite(Image, IconType);
		}
	}
}