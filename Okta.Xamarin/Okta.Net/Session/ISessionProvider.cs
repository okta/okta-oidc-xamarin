using Okta.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Session
{
	public interface ISessionProvider
	{
		IStorageProvider StorageProvider { get; set; }
		T Get<T>(string key);
		string Get(string key);
		void Set(string key, string value);
	}
}
