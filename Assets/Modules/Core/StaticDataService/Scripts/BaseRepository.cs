using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.StaticData
{
	public class CollectionRepository<TV> : IRepository where TV : IId
	{
		public Dictionary<string, TV> Data = new Dictionary<string, TV>();

		private ISettingFileProvider _fileProvider;

		public CollectionRepository(ISettingFileProvider fileProvider)
		{
			IsLoaded = false;
			_fileProvider = fileProvider;
		}

		public virtual bool IsLoaded { get; private set; }

		public virtual void Load(string file)
		{
			Data.Clear();
			var listItems = JsonConvert.DeserializeObject<List<TV>>(_fileProvider.GetFile(file));
			listItems.ForEach(li =>
			{
				if (Data.ContainsKey(li.ID))
				{
					Debug.LogError($"Same ID {li.ID} already added in {file}");
				}
				else
				{
					Data.Add(li.ID, li);
				}
			});
			IsLoaded = true;
		}

		public TV Get(string id)
		{
			if (Data.ContainsKey(id))
			{
				return Data[id];
			}

			throw new MissingMemberException($"Element with id {id} no exist");
		}
	}

	public class Repository<V> : IRepository where V : class
	{
		private ISettingFileProvider _fileProvider;
		public virtual bool IsLoaded { get; private set; }
		public V Data;

		public Repository(ISettingFileProvider fileProvider)
		{
			IsLoaded = false;
			_fileProvider = fileProvider;
		}

		public void Load(string file)
		{
			Data = JsonConvert.DeserializeObject<V>(_fileProvider.GetFile(file));
			IsLoaded = true;
		}
	}
}