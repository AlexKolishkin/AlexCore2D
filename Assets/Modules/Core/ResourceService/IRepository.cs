namespace Core.Resource
{
	public interface IRepository
	{
		bool IsLoaded { get; }
		void Load(string file);
	}
}