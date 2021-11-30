// <copyright file="ProfileIdentityClientConfigurationProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Oie.Client;

namespace Okta.Xamarin.Oie.Configuration
{
    public class ProfileIdentityClientConfigurationProvider : IIdentityClientConfigurationProvider
    {
        public IdentityClientConfiguration GetConfiguration()
        {
            return new ProfileIdentityClientConfiguration().Load();
        }
    }
}
