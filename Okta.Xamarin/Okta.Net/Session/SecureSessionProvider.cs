using Newtonsoft.Json;
using Okta.Net.Data;
using Okta.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Session
{
	/// <summary>
	/// A secure session provider.  Keys are hashed and values encrypted for storage.
	/// </summary>
	public sealed class SecureSessionProvider : ISessionProvider
	{
		public const string EncryptionKeyKey = "AesKey";
		public const string EncryptionIVKey = "AesIV";

		private AesManaged _aes;

		public SecureSessionProvider(IStorageProvider storageProvider = null, ILoggingProvider loggingProvider = null)
		{
			this.LoggingProvider = loggingProvider ?? new LoggingProvider();
			this.StorageProvider = storageProvider ?? new InMemoryStorageProvider(this.LoggingProvider);
			_ = this.InitializeAsync();
		}

		public ILoggingProvider LoggingProvider { get; set; }
		public IStorageProvider StorageProvider { get; set; }

		public T Get<T>(string key)
		{
			string value = Get(key);
			return JsonConvert.DeserializeObject<T>(value);
		}

		public string Get(string key)
		{
			string keyHash = GetKeyHash(key);
			string base64EncodedCipher = this.StorageProvider.LoadAsync(keyHash).Result;
			return Decrypt(base64EncodedCipher);
		}

		public void Set(string key, string value)
		{
			string base64EncodedCipher = Encrypt(value);
			string keyHash = GetKeyHash(key);
			this.StorageProvider.SaveAsync(keyHash, base64EncodedCipher);
		}

		private string GetKeyHash(string key)
		{
			byte[] keyHashBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
			return BitConverter.ToString(keyHashBytes).Replace("-", "").ToLower();
		}

		private async Task InitializeAsync()
		{
			this._aes = new AesManaged();
			string base64EncodedKey = await StorageProvider.LoadAsync(EncryptionKeyKey);
			string base64EncodedIV = await StorageProvider.LoadAsync(EncryptionIVKey);
			if (string.IsNullOrEmpty(base64EncodedKey))
			{
				this._aes.GenerateKey();
				base64EncodedKey = Convert.ToBase64String(this._aes.Key);
				_ = StorageProvider.SaveAsync(EncryptionKeyKey, base64EncodedKey);
			}

			if (string.IsNullOrEmpty(base64EncodedIV))
			{
				this._aes.GenerateIV();
				base64EncodedIV = Convert.ToBase64String(this._aes.IV);
				_ = StorageProvider.SaveAsync(EncryptionIVKey, base64EncodedIV);
			}
			this._aes.Key = Convert.FromBase64String(base64EncodedKey);
			this._aes.IV = Convert.FromBase64String(base64EncodedIV);
		}

		private string Encrypt(string value)
		{
			ICryptoTransform encryptor = this._aes.CreateEncryptor();
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
			ICryptoTransform decryptor = this._aes.CreateDecryptor();

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
