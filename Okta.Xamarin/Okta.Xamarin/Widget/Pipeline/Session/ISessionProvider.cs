// <copyright file="ISessionProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Widget.Pipeline.Data;
using Okta.Xamarin.Widget.Pipeline.Logging;

namespace Okta.Xamarin.Widget.Pipeline.Session
{
    public interface ISessionProvider
    {
        ILoggingProvider LoggingProvider { get; set; }

        IStorageProvider StorageProvider { get; set; }

        T Get<T>(string key);

        string Get(string key);

        void Set(string key, string value);
    }
}
