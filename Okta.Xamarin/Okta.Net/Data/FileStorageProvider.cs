using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Data
{
	public class FileStorageProvider : StorageProvider
	{
		public FileStorageProvider()
		{
			this.Name = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
		}

		public FileStorageProvider(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Gets or sets the name of this file storage provider.  Used as the folder name that files are saved to, the default is the name of the entry assembly.
		/// </summary>
		public string Name { get; set; }

		protected override bool Delete(string key)
		{
			FileInfo fileToDelete = new FileInfo(GetFilePath(key));
			if(fileToDelete.Exists)
			{
				fileToDelete.Delete();
				return true;
			}
			return false;
		}

		protected override string Load(string key)
		{
			FileInfo fileToRead = new FileInfo(GetFilePath(key));
			if (fileToRead.Exists)
			{
				return File.ReadAllText(fileToRead.FullName);
			}
			return string.Empty;
		}

		protected override void Save(string key, object value)
		{
			object toSave = this.OnBeforeSave(value);
			if(toSave is string json)
			{
				FileInfo fileToWrite = new FileInfo(GetFilePath(key));
				File.WriteAllText(fileToWrite.FullName, json);
			}
		}

		protected string GetFilePath(string fileName)
		{
			return Path.Combine(GetRootFolderPath(), fileName);
		}

		protected string GetRootFolderPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Name);
		}

		protected override IEnumerable<string> GetAllKeys()
		{
			DirectoryInfo rootFolder = new DirectoryInfo(GetRootFolderPath());
			foreach(FileInfo file in rootFolder.GetFiles())
			{
				yield return Path.GetFileNameWithoutExtension(file.Name);
			}
		}

		protected override Dictionary<string, object> LoadAll()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			foreach(string key in GetAllKeys())
			{
				result.Add(key, Load(key));
			}
			return result;
		}
	}
}
