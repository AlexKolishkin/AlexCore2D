namespace Core.StaticData
{
	public interface IRepository
	{
		bool IsLoaded { get; }
		void Load(string file);
	}
}