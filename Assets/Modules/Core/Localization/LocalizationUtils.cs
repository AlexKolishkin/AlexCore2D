using Core.Resource;
using UnityEngine;

namespace Core.Localization
{
	public static class LocalizationUtils
	{
		private static ResourceService _resourceService;

		public static void Init(ResourceService resourceService)
		{
			_resourceService = resourceService;
		}
		
		public static string GetLocalizedString(this string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogError("GetLocalizedString: string is null");
				return "string is null";
			}

			var result = _resourceService.Localization.GetString(key);
			return result;
		}
	}
}