using Core.Localization;
using UnityEngine;
using Zenject;

namespace Core.StaticData
{
	public class StaticDataService : IService, IInitializable, ILoading
	{
		public LoadingState LoadingState { get; private set; }

		public const string KTestPAth = "test";
		public const string KLocalizationFile = "translate";

		public const string JsonFolderPath = "Data";

		public ILocalizationRepository Localization { get; } =
	new LocalizationStringRepository(new ResourcesSettingFileProvider());

		public CollectionRepository<TestDataExample> TestRepository { get; } =
			new CollectionRepository<TestDataExample>(new ResourcesSettingFileProvider());

		public void Initialize()
		{
			LoadResources();
			Debug.Log("Initialize ResourceService");
		}

		public void LoadResources()
		{
			LoadingState = LoadingState.Loading;

			Localization.Load(KLocalizationFile);
			TestRepository.Load(KTestPAth);

			LoadingState = LoadingState.Loaded;
		}

	}
}