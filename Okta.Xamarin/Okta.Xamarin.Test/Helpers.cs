using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public static class TestHelpers
	{
		public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dict, K key, V defaultValue = default(V))
		{
			if (dict.ContainsKey(key))
				return dict[key];
			else
				return default(V);
		}
	}
}
