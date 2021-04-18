using Newtonsoft.Json;
using UnityEngine;

namespace Core.Localization
{
    [JsonObject]
    public class LocalizationStringData
    {
        public string id;
        public string en;
        public string ua;
        public string ru;

		public string GetString(SystemLanguage language)
        {
            if (language == SystemLanguage.Ukrainian)
                return ua;
			if (language == SystemLanguage.Russian)
				return ru;
			return en;
        }
    }
}