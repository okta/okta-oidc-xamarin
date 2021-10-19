using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Okta.Net.Configuration
{
	public static class YamlExtensions
	{
		public static string ToYaml(this object value)
		{
			Serializer serializer = new Serializer();
			return serializer.Serialize(value);
		}

		public static T FromYaml<T>(this string yaml)
		{
			Deserializer deserializer = new Deserializer();
			return deserializer.Deserialize<T>(yaml);
		}
	}
}
