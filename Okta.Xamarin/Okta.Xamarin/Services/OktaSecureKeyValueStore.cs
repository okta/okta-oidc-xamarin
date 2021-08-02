// <copyright file="SecureKeyValueStore.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// A key value store that uses Xamarin.Essentials.SecureStorage as the backing store.
    /// </summary>
    public class OktaSecureKeyValueStore : SecureKeyValueStore
    {
        /// <inheritdoc/>
        public override async Task<T> GetAsync<T>(string key)
        {
            string json = await this.GetAsync(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <inheritdoc/>
        public override Task<string> GetAsync(string key)
        {
            return SecureStorage.GetAsync(key);
        }

        /// <inheritdoc/>
        public override bool Remove(string key)
        {
            return SecureStorage.Remove(key);
        }

        /// <inheritdoc/>
        public override void RemoveAll()
        {
            SecureStorage.RemoveAll();
        }

        /// <inheritdoc/>
        public override Task SetAsync(string key, string value)
        {
            return SecureStorage.SetAsync(key, value);
        }
    }
}
