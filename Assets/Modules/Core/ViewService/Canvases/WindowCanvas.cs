namespace Core.View
{
	public class WindowCanvas : UICanvas
	{
		private static bool isCreated;

		protected void Awake()
		{
			if (!isCreated)
			{
				DontDestroyOnLoad(gameObject);
				isCreated = true;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}