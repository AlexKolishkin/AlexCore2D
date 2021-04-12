
namespace Core.View
{
	public class InnerView<T> : GameBehaviour
	{
		public virtual void Setup(T data)
		{
			Collector.Dispose();
			Appear();
		}

		public virtual void Appear()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}