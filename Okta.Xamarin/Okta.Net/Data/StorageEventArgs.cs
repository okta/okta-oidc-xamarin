using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Data
{
	public class StorageEventArgs : EventArgs
	{
		public string Key { get; set; }
		public object Value { get; set; }
		public IStorageProvider StorageProvider { get; set; }
		public Exception Exception { get; set; }
	}
}
