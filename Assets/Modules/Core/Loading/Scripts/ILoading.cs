namespace Core
{
	public interface ILoading
	{
		LoadingState LoadingState { get; }
	}

	public enum LoadingState
	{
		None,
		Loading,
		Loaded
	}
}