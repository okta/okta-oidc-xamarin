// <copyright file="IdentityViewModelProviderEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Oie.Client.Data;

namespace Okta.Xamarin.Oie.Client.View
{
    public class IdentityViewModelProviderEventArgs : IdentityDataProviderEventArgs
    {
        public IIdentityViewModelProvider ViewModelProvider { get; set; }

        public IIdentityFormViewModel ViewModel { get; set; }
    }
}
