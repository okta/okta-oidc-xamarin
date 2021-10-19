using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// An abstract base class denoting intent that implementations should be secure.
    /// </summary>
    public abstract class SecureKeyValueStore : IKeyValueStore
    {
        /// <summary>
        /// Get the value associated with the specified key and deserialize it as the specified generic type.
        /// The value is assumed to be a json string.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task{T}.</returns>
        public abstract Task<T> GetAsync<T>(string key);

        /// <inheritdoc/>
        public abstract Task<string> GetAsync(string key);

        /// <inheritdoc/>
        public abstract bool Remove(string key);

        /// <inheritdoc/>
        public abstract void RemoveAll();

        /// <inheritdoc/>
        public abstract Task SetAsync(string key, string value);
    }
}
