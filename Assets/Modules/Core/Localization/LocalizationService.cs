using Core;
using Core.Attributes;
using Core.StaticData;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Localization
{
	public class LocalizationService : ILocalizationService, IInitializable, IAutoPersistent
	{
		[Persistent]
		public SystemLanguage _currentLanguage;

		public ReactiveProperty<SystemLanguage> CurrentLanguage { get; set; } = new ReactiveProperty<SystemLanguage>();

		private StaticDataService _staticDataService;
		
		[Inject]
		public LocalizationService(StaticDataService resourceService)
		{
			_staticDataService = resourceService;
		}

		public void Initialize()
		{
			LocalizationUtils.Init(_staticDataService);

			ChangeLanguage(CurrentLanguage.Value);
			Debug.Log("LocalizationService Initialized");
		}

		public void OnLoaded()
		{
			CurrentLanguage.Value = _currentLanguage;
			ChangeLanguage(CurrentLanguage.Value);
		}

		public void ChangeLanguage(SystemLanguage language)
		{
			_staticDataService.Localization.Setup(language);
			CurrentLanguage.Value = language;
		}
	}

	public interface ILocalizationService : IService
	{
		ReactiveProperty<SystemLanguage> CurrentLanguage { get; }
		void ChangeLanguage(SystemLanguage language);
	}
}