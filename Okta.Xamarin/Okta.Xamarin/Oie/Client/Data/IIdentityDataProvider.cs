// <copyright file="IIdentityDataProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Oie.Client.View;
using Okta.Xamarin.Oie.Session;

namespace Okta.Xamarin.Oie.Client.Data
{
    public interface IIdentityDataProvider : IHasServiceProvider
    {
        event EventHandler<IdentityDataProviderEventArgs> SessionStarting;

        event EventHandler<IdentityDataProviderEventArgs> SessionStarted;

        event EventHandler<IdentityDataProviderEventArgs> SessionStartExceptionThrown;

        SecureSessionProvider SecureSessionProvider { get; set; }

        IIdentityViewModelProvider ViewModelProvider { get; set; }

        Task<IIdentityInteraction> StartSessionAsync();

        Task<IIdentityIntrospection> GetFormDataAsync(string interactionHandle);

        Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm);
    }
}
