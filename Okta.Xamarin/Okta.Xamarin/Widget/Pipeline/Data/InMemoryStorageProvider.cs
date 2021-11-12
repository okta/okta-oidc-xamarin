// <copyright file="InMemoryStorageProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Okta.Xamarin.Widget.Pipeline.Logging;

namespace Okta.Xamarin.Widget.Pipeline.Data
{
    public class InMemoryStorageProvider : StorageProvider
    {
        private readonly Dictionary<string, object> storage;

        public InMemoryStorageProvider(ILoggingProvider loggingProvider) : base(loggingProvider)
        {
            storage = new Dictionary<string, object>();
        }

        protected override bool Delete(string key)
        {
            if (this.storage.ContainsKey(key))
            {
                return this.storage.Remove(key);
            }

            return false;
        }

        protected override IEnumerable<string> GetAllKeys()
        {
            return this.storage.Keys;
        }

        protected override string Load(string key)
        {
            if (this.storage.ContainsKey(key))
            {
                return this.storage[key] as string;
            }

            return string.Empty;
        }

        protected override Dictionary<string, object> LoadAll()
        {
            return this.storage;
        }

        protected override void Save(string key, object value)
        {
            if (this.storage.ContainsKey(key))
            {
                this.storage[key] = value;
            }
            else
            {
                this.storage.Add(key, value);
            }
        }
    }
}
