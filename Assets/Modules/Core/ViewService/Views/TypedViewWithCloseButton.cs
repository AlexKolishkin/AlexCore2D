using UnityEngine.UI;
using UniRx;

namespace Core.View
{
	public abstract class TypedViewWithCloseButton : TypedView
	{
		public Button closeButton;

		public void Awake()
		{
			closeButton.OnClickAsObservable().Subscribe(unit => CloseView()).AddTo(Collector);
		}

		protected void CloseView()
		{
			_viewService.Remove(Type);
		}
	}
}