// <copyright file="IIdentityViewModelProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Okta.Xamarin.Oie.Client.View
{
    public interface IIdentityViewModelProvider
    {
        event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelStarted;

        event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelCompleted;

        event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelExceptionThrown;

        Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm);
    }
}
