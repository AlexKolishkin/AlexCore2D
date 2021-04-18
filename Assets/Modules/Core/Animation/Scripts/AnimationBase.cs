using System.Collections;
using UnityEngine;

namespace Core.Animation
{
	public abstract class AnimationBase : MonoBehaviour
	{
		public abstract IEnumerator Animate();
	}
}