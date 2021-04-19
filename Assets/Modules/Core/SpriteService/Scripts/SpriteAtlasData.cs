using Core.Addressable;
using UnityEngine;
using UnityEngine.U2D;

namespace Core.Sprites
{
	public class SpriteAtlasData
	{
		public string Path;
		public SpriteAtlas Atlas => Handle.Result;

		public AddressableUtils.AsyncOperationHandleDisposable<SpriteAtlas> Handle;

		public SpriteAtlasData(string path)
		{
			Path = path;
		}
	}
}
