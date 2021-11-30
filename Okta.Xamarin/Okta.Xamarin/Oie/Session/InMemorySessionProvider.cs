// <copyright file="InMemorySessionProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;
using Okta.Xamarin.Oie.Data;
using Okta.Xamarin.Oie.Logging;

namespace Okta.Xamarin.Oie.Session
{
    public class InMemorySessionProvider : ISessionProvider
    {
        private readonly Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        public InMemorySessionProvider()
        {
        }

        public IStorageProvider StorageProvider { get; set; }

        public ILoggingProvider LoggingProvider { get; set; }

        /// <summary>
        /// Get the value associated with the specified key as the specified generic type.  Assumes that the stored value 
        /// is Json.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object as.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>{T}.</returns>
        public T Get<T>(string key)
        {
            if (this.keyValuePairs.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(this.keyValuePairs[key]);
            }

            return default(T);
        }

        public string Get(string key)
        {
            if (this.keyValuePairs.ContainsKey(key))
            {
                return this.keyValuePairs[key];
            }

            return string.Empty;
        }

        public void Set(string key, string value)
        {
            if (this.keyValuePairs.ContainsKey(key))
            {
                this.keyValuePairs[key] = value;
            }
            else
            {
                this.keyValuePairs.Add(key, value);
            }
        }
    }
}
