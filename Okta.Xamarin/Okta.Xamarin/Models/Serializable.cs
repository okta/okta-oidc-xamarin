// <copyright file="Serializable.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Okta.Xamarin.Models
{
	/// <summary>
	/// Base class for serializing to and from json.
	/// </summary>
	public abstract class Serializable
    {
        /// <summary>
        /// Deserialize the specified json as the specified generic type T.
        /// </summary>
        /// <typeparam name="T">The kind of instance to return.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns>T.</returns>
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Returns a json representation of the current instance.
        /// </summary>
        /// <returns>Json.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Returns a Dictionary representation of the current instance.
        /// </summary>
        /// <returns>Dictionary{string, object}.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(ToJson());
        }

        /// <summary>
        /// Gets the values of the current instance as query string.
        /// </summary>
        /// <param name="prefixWithQuestionMark">True to prefix result with a question mark.</param>
        /// <returns>Query string.</returns>
        public string ToQueryString(bool prefixWithQuestionMark = false)
        {
            var type = this.GetType();
            var properties = type.GetProperties();
            var result = new StringBuilder();
            if (prefixWithQuestionMark)
            {
                result.Append("?");
            }

            result.Append(string.Join("&", properties.Select(prop => $"{this.GetQueryStringName(prop)}={prop.GetValue(this)?.ToString()}").ToArray()));
            return result.ToString();
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
