using System.Collections.Generic;
using Core.Resource;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Localization
{
	public class LocalizationStringRepository : ILocalizationRepository
	{
		private readonly Dictionary<uint, string> _frequentSubcache = new Dictionary<uint, string>();
		private readonly Dictionary<uint, string> _strings = new Dictionary<uint, string>();
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
					var jenkinsID = Utils.Utils.JenkinsOneAtATimeHash(item.id);
					if (_strings.ContainsKey(jenkinsID))
						Debug.LogWarning($"string => {item.id} has repeated jenkins id => {jenkinsID}");
					else
						_strings.Add(jenkinsID, item.GetString(Language));
				}

				IsLoaded = true;
			}
		}
		
		public string GetString(uint id)
		{
			if (_strings.ContainsKey(id)) return _strings[id];

			return string.Empty;
		}

		public string GetFrequentString(string key)
		{
			var id = Utils.Utils.JenkinsOneAtATimeHash(key);
			if (!_frequentSubcache.ContainsKey(id))
			{
				_frequentSubcache.Add(id, GetString(id));
			}

			return _frequentSubcache.TryGet(id);
		}

		public string GetString(string key)
		{
			var id = Utils.Utils.JenkinsOneAtATimeHash(key);
			if (_strings.ContainsKey(id)) return _strings[id];

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