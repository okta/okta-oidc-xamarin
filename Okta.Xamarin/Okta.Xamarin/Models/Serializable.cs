using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Okta.Xamarin.Models
{
    public abstract class Serializable
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToQueryString(bool prefixWithQuestionMark = false)
        {
            var type = GetType();
            var properties = type.GetProperties();
            var result = new StringBuilder();
            if (prefixWithQuestionMark)
            {
                result.Append("?");
            }

            result.Append(string.Join("&",
                properties.Select(prop => $"{GetQueryStringName(prop)}={prop.GetValue(this)?.ToString()}").ToArray()));
            return result.ToString();
        }
        
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string GetQueryStringName(PropertyInfo propertyInfo)
        {
            var result = propertyInfo.Name;
            var attribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
            if (attribute != null)
            {
                result = attribute.PropertyName;
            }

            return result;
        }
    }
}