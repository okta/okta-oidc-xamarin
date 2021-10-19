// <copyright file="IKeyValueStore.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// An interface defining a basic key-value store.
    /// </summary>
    public interface IKeyValueStore
    {
        /// <summary>
        /// Get the value associated with the specified key as the specified generic type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object as.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>{T}.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Get the string value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>string.</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Task.</returns>
        Task SetAsync(string key, string value);

        /// <summary>
        /// Removes the value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>a boolean value indicating success or failure.</returns>
        bool Remove(string key);

        /// <summary>
        /// Removes all values.
        /// </summary>
        void RemoveAll();
    }
}
