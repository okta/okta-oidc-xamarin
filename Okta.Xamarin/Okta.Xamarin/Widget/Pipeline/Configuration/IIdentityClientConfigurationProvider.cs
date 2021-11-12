// <copyright file="IIdentityClientConfigurationProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Widget.Pipeline.Identity;

namespace Okta.Xamarin.Widget.Pipeline.Configuration
{
    public interface IIdentityClientConfigurationProvider
    {
        IdentityClientConfiguration GetConfiguration();
    }
}
