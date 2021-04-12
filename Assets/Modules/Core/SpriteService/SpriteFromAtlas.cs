using Core.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Sprites
{
	public abstract class SpriteFromAtlas : GameBehaviour
	{
		protected Image Image;

		protected ISpriteService SpriteService;

		[Inject]
		private void Construct(ISpriteService spriteService)
		{
			SpriteService = spriteService;
		}

		protected void Awake()
		{
			Image = GetComponent<Image>();
		}
	}
}