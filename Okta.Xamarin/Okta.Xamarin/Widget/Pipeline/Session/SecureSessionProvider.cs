// <copyright file="SecureSessionProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Okta.Xamarin.Widget.Pipeline.Data;
using Okta.Xamarin.Widget.Pipeline.Logging;

namespace Okta.Xamarin.Widget.Pipeline.Session
{
    /// <summary>
    /// A secure session provider.  Keys are hashed and values encrypted for storage.
    /// </summary>
    public sealed class SecureSessionProvider : ISessionProvider
    {
        public const string EncryptionKeyKey = "AesKey";
        public const string EncryptionIVKey = "AesIV";

        private AesManaged aes;

        public SecureSessionProvider(IStorageProvider storageProvider = null, ILoggingProvider loggingProvider = null)
        {
            this.LoggingProvider = loggingProvider ?? new LoggingProvider();
            this.StorageProvider = storageProvider ?? new InMemoryStorageProvider(this.LoggingProvider);
            _ = this.InitializeAsync(); // TODO: Ensure getters and setters wait for initialization to complete before performing associated operations.
        }

        public ILoggingProvider LoggingProvider { get; set; }

        public IStorageProvider StorageProvider { get; set; }

        public T Get<T>(string key)
        {
            string value = this.Get(key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string Get(string key)
        {
            string keyHash = this.GetKeyHash(key);
            string base64EncodedCipher = this.StorageProvider.LoadAsync(keyHash).Result;
            return this.Decrypt(base64EncodedCipher);
        }

        public void Set(string key, string value)
        {
            string base64EncodedCipher = this.Encrypt(value);
            string keyHash = this.GetKeyHash(key);
            this.StorageProvider.SaveAsync(keyHash, base64EncodedCipher);
        }

        private string GetKeyHash(string key)
        {
            byte[] keyHashBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(keyHashBytes).Replace("-", string.Empty).ToLower();
        }

        private async Task InitializeAsync()
        {
            this.aes = new AesManaged();
            string base64EncodedKey = await this.StorageProvider.LoadAsync(EncryptionKeyKey);
            string base64EncodedIV = await this.StorageProvider.LoadAsync(EncryptionIVKey);
            if (string.IsNullOrEmpty(base64EncodedKey))
            {
                this.aes.GenerateKey();
                base64EncodedKey = Convert.ToBase64String(this.aes.Key);
                _ = this.StorageProvider.SaveAsync(EncryptionKeyKey, base64EncodedKey);
            }

            if (string.IsNullOrEmpty(base64EncodedIV))
            {
                this.aes.GenerateIV();
                base64EncodedIV = Convert.ToBase64String(this.aes.IV);
                _ = this.StorageProvider.SaveAsync(EncryptionIVKey, base64EncodedIV);
            }

            this.aes.Key = Convert.FromBase64String(base64EncodedKey);
            this.aes.IV = Convert.FromBase64String(base64EncodedIV);
        }

        private string Encrypt(string value)
        {
            ICryptoTransform encryptor = this.aes.CreateEncryptor();
            using (MemoryStream encryptBuffer = new MemoryStream())
            {
                using (CryptoStream encryptStream = new CryptoStream(encryptBuffer, encryptor, CryptoStreamMode.Write))
                {
                    byte[] data = Encoding.UTF8.GetBytes(value);
                    encryptStream.Write(data, 0, data.Length);
                    encryptStream.FlushFinalBlock();

                    return Convert.ToBase64String(encryptBuffer.ToArray());
                }
            }
        }

        private string Decrypt(string base64EncodedCipher)
        {
            ICryptoTransform decryptor = this.aes.CreateDecryptor();

            byte[] encryptedData = Convert.FromBase64String(base64EncodedCipher);
            using (MemoryStream decryptBuffer = new MemoryStream(encryptedData))
            {
                using (CryptoStream decryptStream = new CryptoStream(decryptBuffer, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decrypted = new byte[encryptedData.Length];

                    decryptStream.Read(decrypted, 0, encryptedData.Length);

                    // Remove trailing 0 bytes
                    List<byte> retBytes = new List<byte>();
                    foreach (byte b in decrypted)
                    {
                        if (b == 0)
                        {
                            break;
                        }

                        retBytes.Add(b);
                    }

                    return Encoding.UTF8.GetString(retBytes.ToArray());
                }
            }
        }
    }
}
