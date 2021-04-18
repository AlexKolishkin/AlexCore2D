using Core.StaticData;
using UnityEngine;

namespace Core.Localization
{
	public interface ILocalizationRepository : IRepository
	{
		void Setup(SystemLanguage language);
		string GetString(string key);
	}
}