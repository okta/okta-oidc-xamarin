// <copyright file="IdentityViewModelProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Oie.Client.Data;

namespace Okta.Xamarin.Oie.Client.View
{
    public class IdentityViewModelProvider : IIdentityViewModelProvider
    {
        public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelStarted;

        public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelCompleted;

        public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelExceptionThrown;

        public IIdentityDataProvider DataProvider { get; set; }

        public async Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm)
        {
            try
            {
                this.GetViewModelStarted?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    ViewModelProvider = this,
                });

                IdentityFormViewModel result = new IdentityFormViewModel(identityForm);

                this.GetViewModelCompleted?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    ViewModelProvider = this,
                    ViewModel = result,
                });
                return result;
            }
            catch (Exception ex)
            {
                this.GetViewModelExceptionThrown?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    ViewModelProvider = this,
                    Exception = ex,
                });
                return new IdentityFormViewModel { Exception = ex };
            }
        }
    }
}
