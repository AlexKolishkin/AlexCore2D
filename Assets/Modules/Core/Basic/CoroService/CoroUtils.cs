using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
	public static class CoroUtils
	{
		public static IEnumerator WaitComplete(this IEnumerable<Coro> coros)
		{
			var enumerable = coros as Coro[] ?? coros.ToArray();
			if (!enumerable.Any())
			{
				yield break;
			}
			yield return new WaitWhile(() => !enumerable.All(val => val.Completed.Value));
		}
	}

}