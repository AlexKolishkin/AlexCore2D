using System.Collections;
using Zenject;

namespace Core.View
{
	public abstract class BaseView : GameBehaviour
	{
		protected ViewData ViewData { get; private set; }

		public virtual int ViewDepth { get; } = 0;
		private int OverrideViewDepth { get; set; } = int.MinValue;

		public int SortDepth
			=> OverrideViewDepth == int.MinValue ? ViewDepth : OverrideViewDepth;

		public virtual CanvasType CanvasType { get; } = CanvasType.Window;

		public virtual void Setup(ViewData data)
		{
			ViewData = data;
		}

		public virtual void OnViewRemove()
		{
		}

		public void SetViewDepth(int value)
		{
			OverrideViewDepth = value;
		}
	}
}