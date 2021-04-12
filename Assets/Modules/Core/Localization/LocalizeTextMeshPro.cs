using Core.View;
using TMPro;
using UniRx;
using Zenject;

namespace Core.Localization
{
	public class LocalizeTextMeshPro : GameBehaviour
	{
		public string Key;

		public bool AddSuffix;
		public string Suffix = string.Empty;
		private TextMeshProUGUI _textObject;

		private ILocalizationService _localizationService;

		[Inject]
		private void Construct(ILocalizationService localizationService)
		{
			_localizationService = localizationService;
		}

		private void Start()
		{
			_textObject = GetComponent<TextMeshProUGUI>();
			_localizationService.CurrentLanguage.Subscribe(val => { Text = ConstructString(); }).AddTo(Collector);
		}


		public string Text
		{
			get => _textObject.text;
			set => _textObject.text = value;
		}

		private void Reset()
		{
			_textObject = GetComponent<TextMeshProUGUI>();
		}

		private string ConstructString()
		{
			var str = Key.GetLocalizedString();
			if (AddSuffix)
			{
				str = str + Suffix;
			}

			return str;
		}
	}
}