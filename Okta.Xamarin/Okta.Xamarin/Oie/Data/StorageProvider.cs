// <copyright file="StorageProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Okta.Xamarin.Oie.Logging;

namespace Okta.Xamarin.Oie.Data
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
            foreach (string key in await GetAllKeysAsync())
            {
                await this.DeleteAsync(key);
            }
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                this.DeleteStarted?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                });

                this.Delete(key);

                this.DeleteCompleted?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                });
                return true;
            }
            catch (Exception ex)
            {
                this.DeleteExceptionThrown?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                    Exception = ex,
                });
                return false;
            }
        }

        public Task<IEnumerable<string>> GetAllKeysAsync()
        {
            return Task.Run(() => this.GetAllKeys());
        }

        public Task<Dictionary<string, object>> LoadAllAsync()
        {
            return Task.Run(() => this.LoadAll());
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            string json = await this.LoadAsync(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<string> LoadAsync(string key)
        {
            try
            {
                this.LoadStarted?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                });

                string value = Load(key);

                this.LoadCompleted?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                    Value = value,
                });

                return value;
            }
            catch (Exception ex)
            {
                this.LoadExceptionThrown?.Invoke(this, new StorageEventArgs
                {
                    Key = key,
                    StorageProvider = this,
                    Exception = ex,
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
                    this.SaveStarted?.Invoke(this, new StorageEventArgs
                    {
                        Key = key,
                        Value = value,
                        StorageProvider = this,
                    });

                    this.Save(key, value);

                    this.SaveCompleted?.Invoke(this, new StorageEventArgs
                    {
                        Key = key,
                        Value = value,
                        StorageProvider = this,
                    });
                }
                catch (Exception ex)
                {
                    this.SaveExceptionThrown?.Invoke(this, new StorageEventArgs
                    {
                        Key = key,
                        Value = value,
                        StorageProvider = this,
                        Exception = ex,
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