using Core.Resource;
using UnityEngine;

namespace Core.Localization
{
	public interface ILocalizationRepository : IRepository
	{
		void Setup(SystemLanguage language);
		string GetString(uint id);
		string GetString(string key);
		string GetFrequentString(string key);
	}
}