using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Resource
{
	public interface ISettingFileProvider
	{
		string GetFile(string fileName);
	}

	public class PlayerDataSettingFileProvider : ISettingFileProvider
	{
		public string GetFile(string fileName)
		{
			var path = Path.Combine(Application.persistentDataPath, Path.Combine(ResourceService.JsonFolderPath, fileName));
			return File.ReadAllText(path);
		}
	}

	public class RealtimeSettingFileProvider : ISettingFileProvider
	{
		public string GetFile(string fileName)
		{
			return DownloadDataFromGoogle.JsonData[fileName];
		}
	}

	public class ResourcesSettingFileProvider : ISettingFileProvider
	{
		public string GetFile(string fileName)
		{
			return Resources.Load<TextAsset>(Path.Combine(ResourceService.JsonFolderPath, fileName)).text;
		}
	}
}