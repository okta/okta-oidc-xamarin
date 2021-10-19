using Newtonsoft.Json;
using Okta.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Session
{
	public class InMemorySessionProvider : ISessionProvider
	{
		private Dictionary<string, string> _keyValuePairs = new Dictionary<string, string>();

		public InMemorySessionProvider()
		{
			
		}

		public IStorageProvider StorageProvider { get; set; }

		/// <summary>
		/// Get the value associated with the specified key as the specified generic type.  Assumes that the stored value 
		/// is Json.
		/// </summary>
		/// <typeparam name="T">The type to deserialize the object as.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>{T}.</returns>
		public T Get<T>(string key)
		{
			if (_keyValuePairs.ContainsKey(key))
			{
				return JsonConvert.DeserializeObject<T>(_keyValuePairs[key]);
			}
			return default(T);
		}

		public string Get(string key)
		{
			if (_keyValuePairs.ContainsKey(key))
			{
				return _keyValuePairs[key];
			}
			return string.Empty;
		}

		public void Set(string key, string value)
		{
			if (_keyValuePairs.ContainsKey(key))
			{
				_keyValuePairs[key] = value;
			}
			else
			{
				_keyValuePairs.Add(key, value);
			}
		}
	}
}
