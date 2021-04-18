using System.Collections.Generic;
using Core.StaticData;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Localization
{
	public class LocalizationStringRepository : ILocalizationRepository
	{
		private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();
		private string _file = string.Empty;

		private ISettingFileProvider _fileProvider;

		public LocalizationStringRepository(ISettingFileProvider fileProvider)
		{
			_fileProvider = fileProvider;
		}

		public SystemLanguage Language { get; private set; } = SystemLanguage.English;

		public void Setup(SystemLanguage language)
		{
			Language = language;
		}

		public bool IsLoaded { get; private set; }

		public void Load(string file)
		{
			if (!IsLoaded)
			{
				this._file = file;
				_strings.Clear();
				var listItems = JsonConvert.DeserializeObject<List<LocalizationStringData>>(_fileProvider.GetFile(file));

				foreach (var item in listItems)
				{
					if (_strings.ContainsKey(item.id))
						Debug.LogWarning($"string => {item.id} has repeated");
					else
						_strings.Add(item.id, item.GetString(Language));
				}

				IsLoaded = true;
			}
		}

		public string GetString(string key)
		{
			if (_strings.ContainsKey(key)) return _strings[key];

			Debug.LogError($"not found string with key => {key}");
			return key;
		}

		public bool Reload(SystemLanguage targetLanguage)
		{
			if (!string.IsNullOrEmpty(_file))
			{
				Setup(targetLanguage);
				IsLoaded = false;
				Load(_file);
				return true;
			}

			return false;
		}
	}
}