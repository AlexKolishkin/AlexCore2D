using DG.Tweening;
using UnityEngine;

namespace Core.Alert
{
	public class AlertAnimation : MonoBehaviour
	{
		private Vector3 _scaleFactorBase = Vector3.one;

		private Vector3 _scaleFactor = new Vector3(1.1f, 1.1f, 1.1f);

		private Tween _tween;

		private void OnEnable()
		{
			_tween = transform.DOScale(_scaleFactor, 0.5f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
		}

		private void OnDisable()
		{
			_tween?.Kill();
			transform.localScale = _scaleFactorBase;
		}
	}
}