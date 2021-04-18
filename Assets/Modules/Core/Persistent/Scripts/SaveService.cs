using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Attributes;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
	public class SaveService : ISaveService, IInitializable, ILoading
	{
		public LoadingState LoadingState { get; private set; }

		private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public |
		                                        BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		private JsonSerializerSettings _serializerSettingsObject = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Objects
		};

		private List<IAutoPersistent> _saveList = new List<IAutoPersistent>();

		private ILifeCycleService _lifeCycleService;
		private DiContainer _diContainer;

		[Inject]
		public SaveService(ILifeCycleService lifeCycleService, DiContainer diContainer)
		{
			_lifeCycleService = lifeCycleService;
			_diContainer = diContainer;
		}


		public void Initialize()
		{
			LoadingState = LoadingState.Loading;

			Debug.Log("Initialize SaveService");
			_lifeCycleService.ApplicationPause.Subscribe(val =>
			{
				if (val)
				{
					Save();
				}
			});

			_lifeCycleService.ApplicationQuitStream.Subscribe(val => { Save(); });

			_saveList.AddRange(_diContainer.ResolveAll<IAutoPersistent>());

			Load();

			foreach (var element in _saveList)
			{
				element.OnLoaded();
			}

			LoadingState = LoadingState.Loaded;
		}

		public void Load()
		{
			foreach (var service in _saveList)
			{
				var type = service.GetType();
				var persistentFields = type.GetFields(FieldFlags)
					.Where(info => info.GetCustomAttribute<PersistentAttribute>() != null).ToList();

				foreach (var persistentField in persistentFields)
				{
					var key = type.Name + "/" + persistentField.Name;
					if (PlayerPrefs.HasKey(key))
					{
						var str = PlayerPrefs.GetString(key);
						if ((persistentField.FieldType.Attributes & TypeAttributes.Interface) != 0)
						{
							persistentField.SetValue(service,
								JsonConvert.DeserializeObject(str, persistentField.FieldType,
									_serializerSettingsObject));
						}
						else
						{
							persistentField.SetValue(service,
								JsonConvert.DeserializeObject(str, persistentField.FieldType));
						}
					}
				}

				if (service is IManualPersistent persistentService)
				{
					var name = persistentService.GetType().Name;
					var save = PlayerPrefs.GetString(name, "");

					if (!string.IsNullOrEmpty(save))
					{
						var saveObject = JsonConvert.DeserializeObject(save, persistentService.SaveType);
						if (saveObject != null)
						{
							persistentService.LoadSave(saveObject);
						}
						else
						{
							persistentService.LoadDefaults();
						}
					}
					else
					{
						persistentService.LoadDefaults();
					}
				}
			}
		}


		public void Save()
		{
			foreach (var service in _saveList)
			{
				var type = service.GetType();

				var persistentFields = type.GetFields(FieldFlags)
					.Where(info => info.GetCustomAttribute<PersistentAttribute>() != null).ToList();

				foreach (var persistentField in persistentFields)
				{
					var key = type.Name + "/" + persistentField.Name;
					string serialized = null;
					if ((persistentField.FieldType.Attributes & TypeAttributes.Interface) != 0)
					{
						serialized = JsonConvert.SerializeObject(persistentField.GetValue(service),
							_serializerSettingsObject);
					}
					else
					{
						serialized = JsonConvert.SerializeObject(persistentField.GetValue(service));
					}

					PlayerPrefs.SetString(key, serialized);
				}

				if (service is IManualPersistent persistentService)
				{
					var name = persistentService.GetType().Name;
					var value = persistentService.GetSave();
					var serialized = JsonConvert.SerializeObject(value);
					PlayerPrefs.SetString(name, serialized);
				}
			}
		}
	}
}