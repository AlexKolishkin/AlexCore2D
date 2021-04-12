using Core.View;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace Core.Localization
{
	public class LocalizeText : GameBehaviour
	{
		public string key;
		public bool toUpperCase = false;
		public bool toLowerCase = false;
		public bool isUpperFirstLetter = false;

		public bool addSuffix = false;
		public string suffix = string.Empty;

		private Text _text;
		private ILocalizationService _localizationService;
	
		[Inject]
		private void Construct(ILocalizationService localizationService)
		{
			_localizationService = localizationService;
		}

		private void Start()
		{
			_text = GetComponent<Text>();
			_localizationService.CurrentLanguage.Subscribe(val => { Localize(); }).AddTo(Collector);
		}

		private void Localize()
		{
			var label = string.IsNullOrEmpty(key) ? null : ConstructString();
			if (_text && label != null)
			{
				_text.text = label;
			}
		}

		private string ConstructString()
		{
			string str = key.GetLocalizedString();
			str = str.Trim();
			if (addSuffix)
			{
				str = str + suffix;
			}

			if (toUpperCase)
			{
				str = str.ToUpper();
			}

			if (toLowerCase)
			{
				str = str.ToLower();
			}

			if (isUpperFirstLetter)
			{
				if (str.Length > 0)
				{
					str = char.ToUpper(str[0]) + str.Substring(1).ToLower();
				}
			}

			return str;
		}
	}
}