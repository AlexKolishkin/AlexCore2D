using System.Threading.Tasks;
using UnityEngine;

namespace Core.StaticData
{
	public interface ILoadingRepository<T, V> where V : Object
	{
		Task<V> LoadPrefab(T key);
		void UnloadPrefab(T key);
		bool HasPrefab(T key);
		void AddPath(T key, string path);
	}

}