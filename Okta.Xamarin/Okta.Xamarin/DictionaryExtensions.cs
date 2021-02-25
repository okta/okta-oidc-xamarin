using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public static class DictionaryExtensions
	{
		public static T GetValueOrDefault<T>(this Dictionary<string, T> keyValuePairs, string key)
		{
			if(keyValuePairs.ContainsKey(key))
			{
				return keyValuePairs[key];
			}
			return default(T);
		}
	}
}
