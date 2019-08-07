using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	public static class Helpers
	{
		public static Dictionary<string, string> ToDictionary(this System.Collections.Specialized.NameValueCollection nvc)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			foreach (var k in nvc.AllKeys)
			{
				dict.Add(k, nvc[k]);
			}
			return dict;
		}

		public static Dictionary<string, string> JsonDecode(string encodedString)
		{
			var inputs = new Dictionary<string, string>();
			var json = JValue.Parse(encodedString) as JObject;

			foreach (KeyValuePair<string, JToken> kv in json)
			{
				if (kv.Value is JValue v)
				{
					if (v.Type != JTokenType.String)
						inputs[kv.Key] = v.ToString();
					else
						inputs[kv.Key] = (string)v;
				}
			}

			return inputs;
		}
	}
}
