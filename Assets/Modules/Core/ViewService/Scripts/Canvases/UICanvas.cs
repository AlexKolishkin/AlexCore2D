using UnityEngine;

namespace Core.View
{
	public abstract class UICanvas : MonoBehaviour
	{
		private Canvas canvas;
		public Transform Content;

		public Canvas Canvas => canvas ? canvas : (canvas = GetComponent<Canvas>());

		public void SetCamera(Camera camera)
		{
			if (Canvas != null) Canvas.worldCamera = camera;
		}
	}
}