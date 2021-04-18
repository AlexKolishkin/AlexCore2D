using Core.Addressable;
using Core.Audio;
using Core.Localization;
using Core.Resource;
using Core.Scene;
using Core.Sprites;
using Core.GameState;
using Core.View;
using System;
using Zenject;
using Core.Analytic;
using Core.Alert;

namespace Core.Bootstrap
{
	public class BootstrapInstaller : MonoInstaller
	{
		private static DiContainer _diContainer;

		[Obsolete]
		public static void InjectBehaviour(GameBehaviour behaviour)
		{
			_diContainer.Inject(behaviour);
		}

		public override void InstallBindings()
		{
			_diContainer = Container;

			BindServices();
			BindViewFactory();

		//	InitExecutionOrder();

			CreateLoadingGameState();
		}

		private void CreateLoadingGameState()
		{
			var service = Container.Resolve<IGameStateService>();
			service.ChangeState(new BoostrapGameState());
		}

		private void BindServices()
		{
			Container.BindInterfacesAndSelfTo<CoroService>().AsSingle();
			Container.BindInterfacesAndSelfTo<ResourceService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SaveService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LifeCycleService>().AsSingle();
			Container.BindInterfacesAndSelfTo<AddressableService>().AsSingle();
			Container.BindInterfacesAndSelfTo<ViewService>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SpriteService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LocalizationService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();

			Container.BindInterfacesAndSelfTo<AlertService>().AsSingle();
			
			Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle();

			Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();

			Container.BindInterfacesAndSelfTo<PlayerServiceExample>().AsSingle();
		}

		private void BindViewFactory()
		{
			Container.Bind<ITypedViewFactory>().To<TypedViewFactory>().AsSingle();
			Container.Bind<IAudioPlayerFactory>().To<AudioPlayerFactory>().AsSingle();
			Container.Bind<IAlertFactory>().To<AlertFactory>().AsSingle();
		}

		private void InitExecutionOrder()
		{
			Container.BindInitializableExecutionOrder<CoroService>(-100);
			Container.BindInitializableExecutionOrder<ResourceService>(-90);
			Container.BindInitializableExecutionOrder<SaveService>(-80);
			Container.BindInitializableExecutionOrder<LifeCycleService>(-70);

			Container.BindInitializableExecutionOrder<LocalizationService>(100);
		}
	}
}
