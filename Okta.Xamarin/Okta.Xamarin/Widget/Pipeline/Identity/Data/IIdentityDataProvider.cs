// <copyright file="IIdentityDataProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Widget.Pipeline.Identity.View;
using Okta.Xamarin.Widget.Pipeline.Session;

namespace Okta.Xamarin.Widget.Pipeline.Identity.Data
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
