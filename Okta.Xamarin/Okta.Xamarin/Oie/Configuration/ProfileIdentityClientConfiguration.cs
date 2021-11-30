// <copyright file="ProfileIdentityClientConfiguration.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Okta.Xamarin.Oie.Client;

namespace Okta.Xamarin.Oie.Configuration
{
    public class ProfileIdentityClientConfiguration : IdentityClientConfiguration
    {
        public const string FileName = "identityclient.json";

        public ProfileIdentityClientConfiguration()
        {
            this.File = new FileInfo(Profile.PathFor($"~/.okta/{Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location)}/{FileName}"));
        }

        [JsonIgnore]
        public FileInfo File { get; set; }

        public ProfileIdentityClientConfiguration Load()
        {
            if (this.File.Exists)
            {
                IdentityClientConfiguration existing = System.IO.File.ReadAllText(this.File.FullName).FromJson<IdentityClientConfiguration>();

                this.CopyProperties(existing);
            }
            else
            {
                this.CopyProperties(IdentityClientConfiguration.Default);
                Save();
            }

            return this;
        }

        public void Save()
        {
            if (!this.File.Directory.Exists)
            {
                this.File.Directory.Create();
            }

            System.IO.File.WriteAllText(File.FullName, this.ToJson(true));
            this.File.Refresh();
        }
    }
}
