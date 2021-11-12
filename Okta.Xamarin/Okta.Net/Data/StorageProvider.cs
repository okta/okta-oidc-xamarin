using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Okta.Net.Data
{
	public abstract class StorageProvider : IStorageProvider
	{
		public event EventHandler<StorageEventArgs> LoadStarted;
		public event EventHandler<StorageEventArgs> LoadCompleted;
		public event EventHandler<StorageEventArgs> LoadExceptionThrown;
		public event EventHandler<StorageEventArgs> SaveStarted;
		public event EventHandler<StorageEventArgs> SaveCompleted;
		public event EventHandler<StorageEventArgs> SaveExceptionThrown;
		public event EventHandler<StorageEventArgs> DeleteStarted;
		public event EventHandler<StorageEventArgs> DeleteCompleted;
		public event EventHandler<StorageEventArgs> DeleteExceptionThrown;

		public StorageProvider(ILoggingProvider loggingProvider)
		{
			this.LoggingProvider = loggingProvider;
		}

		public ILoggingProvider LoggingProvider { get; }

		protected virtual object OnBeforeSave(object value)
		{
			return JsonConvert.SerializeObject(value);
		}

		public async Task DeleteAllAsync()
		{
			foreach(string key in await GetAllKeysAsync())
			{
				await DeleteAsync(key);
			}
		}

		public async Task<bool> DeleteAsync(string key)
		{
			try
			{
				DeleteStarted?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this,
				});

				Delete(key);

				DeleteCompleted?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this,
				});
				return true;
			}
			catch (Exception ex)
			{
				DeleteExceptionThrown?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this,
					Exception = ex
				});
				return false;
			}
		}

		public Task<IEnumerable<string>> GetAllKeysAsync()
		{
			return Task.Run(() => GetAllKeys());
		}

		public Task<Dictionary<string, object>> LoadAllAsync()
		{
			return Task.Run(() => LoadAll());
		}

		public async Task<T> LoadAsync<T>(string key)
		{
			string json = await LoadAsync(key);
			return JsonConvert.DeserializeObject<T>(json);
		}

		public async Task<string> LoadAsync(string key)
		{
			try
			{
				LoadStarted?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this
				});

				string value = Load(key);

				LoadCompleted?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this,
					Value = value
				});

				return value;
			}
			catch (Exception ex)
			{
				LoadExceptionThrown?.Invoke(this, new StorageEventArgs
				{
					Key = key,
					StorageProvider = this,
					Exception = ex
				});
				return string.Empty;
			}
		}

		public Task SaveAsync(string key, object value)
		{
			return Task.Run(() =>
			{
				try
				{
					SaveStarted?.Invoke(this, new StorageEventArgs
					{
						Key = key,
						Value = value,
						StorageProvider = this
					});

					Save(key, value);

					SaveCompleted?.Invoke(this, new StorageEventArgs
					{
						Key = key,
						Value = value,
						StorageProvider = this
					});
				}
				catch (Exception ex)
				{
					SaveExceptionThrown?.Invoke(this, new StorageEventArgs
					{
						Key = key,
						Value = value,
						StorageProvider = this,
						Exception = ex
					});
				}
			});
		}

		protected abstract IEnumerable<string> GetAllKeys();

		protected abstract Dictionary<string, object> LoadAll();

		protected abstract bool Delete(string key);

		protected abstract string Load(string key);

		protected abstract void Save(string key, object value);
	}
}