using Core.Addressable;
using Core.Audio;
using Core.Localization;
using Core.StaticData;
using Core.Scene;
using Core.Sprites;
using Core.GameState;
using Core.View;
using Zenject;
using Core.Analytic;
using Core.Alert;

namespace Core
{
	public class BootstrapInstaller : MonoInstaller
	{
		private static DiContainer _diContainer;

		public static void InjectBehaviour(GameBehaviour behaviour)
		{
			_diContainer.Inject(behaviour);
		}

		public override void InstallBindings()
		{
			_diContainer = Container;

			BindCoreServices();
			BindGameServices();

			BindViewFactory();

			//	InitExecutionOrder();
		}

		private void BindCoreServices()
		{
			Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();
			Container.BindInterfacesAndSelfTo<CoroService>().AsSingle();
			Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SaveService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LifeCycleService>().AsSingle();
			Container.BindInterfacesAndSelfTo<AddressableService>().AsSingle();
			Container.BindInterfacesAndSelfTo<ViewService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SpriteService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LocalizationService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
			Container.BindInterfacesAndSelfTo<AlertService>().AsSingle();
			Container.BindInterfacesAndSelfTo<AnalyticService>().AsSingle();
			Container.BindInterfacesAndSelfTo<MusicService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
		}

		private void BindGameServices()
		{
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
			Container.BindInitializableExecutionOrder<GameStateService>(-100);
			Container.BindInitializableExecutionOrder<CoroService>(-90);
			Container.BindInitializableExecutionOrder<StaticDataService>(-80);
			Container.BindInitializableExecutionOrder<SaveService>(-70);
			Container.BindInitializableExecutionOrder<LifeCycleService>(-60);
		}
	}
}
