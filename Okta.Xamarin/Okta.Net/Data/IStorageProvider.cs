using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Data
{
	public interface IStorageProvider
	{
		event EventHandler<StorageEventArgs> LoadStarted;
		event EventHandler<StorageEventArgs> LoadCompleted;
		event EventHandler<StorageEventArgs> LoadExceptionThrown;

		event EventHandler<StorageEventArgs> SaveStarted;
		event EventHandler<StorageEventArgs> SaveCompleted;
		event EventHandler<StorageEventArgs> SaveExceptionThrown;

		event EventHandler<StorageEventArgs> DeleteStarted;
		event EventHandler<StorageEventArgs> DeleteCompleted;
		event EventHandler<StorageEventArgs> DeleteExceptionThrown;

		Task<IEnumerable<string>> GetAllKeysAsync();

		Task<Dictionary<string, object>> LoadAllAsync();

		/// <summary>
		/// Get the value associated with the specified key as the specified generic type.
		/// </summary>
		/// <typeparam name="T">The type to deserialize the object as.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>{T}.</returns>
		Task<T> LoadAsync<T>(string key);

		/// <summary>
		/// Get the string value associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>string.</returns>
		Task<string> LoadAsync(string key);

		/// <summary>
		/// Sets the value for the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>Task.</returns>
		Task SaveAsync(string key, object value);

		/// <summary>
		/// Removes the value for the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>a boolean value indicating success or failure.</returns>
		Task<bool> DeleteAsync(string key);

		/// <summary>
		/// Removes all values.
		/// </summary>
		Task DeleteAllAsync();
	}
}
