using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Data
{
	public class InMemoryStorageProvider : StorageProvider
	{
		private Dictionary<string, object> _storage;
		public InMemoryStorageProvider()
		{
			_storage = new Dictionary<string, object>();
		}

		protected override bool Delete(string key)
		{
			if(_storage.ContainsKey(key))
			{
				return _storage.Remove(key);
			}
			return false;
		}

		protected override IEnumerable<string> GetAllKeys()
		{
			return _storage.Keys;
		}

		protected override string Load(string key)
		{
			if(_storage.ContainsKey(key))
			{
				return _storage[key] as string;
			}

			return string.Empty;
		}

		protected override Dictionary<string, object> LoadAll()
		{
			return _storage;
		}

		protected override void Save(string key, object value)
		{
			if (_storage.ContainsKey(key))
			{
				_storage[key] = value;
			}
			else
			{
				_storage.Add(key, value);
			}
		}
	}
}
