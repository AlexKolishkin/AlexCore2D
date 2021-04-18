using Core.StaticData;
using UnityEngine;

namespace Core.Localization
{
	public static class LocalizationUtils
	{
		private static StaticDataService _staticDataService;

		public static void Init(StaticDataService StaticDataService)
		{
			_staticDataService = StaticDataService;
		}
		
		public static string GetLocalizedString(this string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogError("GetLocalizedString: string is null");
				return "string is null";
			}

			var result = _staticDataService.Localization.GetString(key);
			return result;
		}
	}
}