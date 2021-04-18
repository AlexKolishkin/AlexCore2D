using Core.Bootstrap;
using Core.View;
using Zenject;

namespace Core
{
	public class GameBehaviour : ReactiveBehavior
	{
		[Inject] protected IViewService _viewService;

		[Inject] protected ICoroService _coroutineService;

		public bool Initialized { get; private set; }

		protected virtual void Awake()
		{
			Init();
		}

		protected void Init()
		{
			if (!Initialized)
			{
				BootstrapInstaller.InjectBehaviour(this);
				Initialized = true;
			}
		}
	}
}