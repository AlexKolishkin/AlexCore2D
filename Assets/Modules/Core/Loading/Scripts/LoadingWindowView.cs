using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class LoadingWindowView : MonoBehaviour
	{
		[SerializeField] private Image _fillProgress;

		[SerializeField] private TextMeshProUGUI _progressInfo;

		public void Awake()
		{
			_fillProgress.fillAmount = 0;
		}

		public void Fill(LoadingData info)
		{
			_fillProgress.DOFillAmount(info.Progress, 1).SetAutoKill();
			_progressInfo.text = info.ProgressInfo;
		}
	}

	public struct LoadingData
	{
		public readonly float Progress;
		public readonly string ProgressInfo;

		public LoadingData(float progress, string progressInfo)
		{
			Progress = progress;
			ProgressInfo = progressInfo;
		}
	}
}