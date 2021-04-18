using System;
using System.Collections;

namespace Core
{ 
	public interface ICoroService : IService
	{
		Coro Run(IEnumerator coroutine, Action onComplete = null);
	}
}