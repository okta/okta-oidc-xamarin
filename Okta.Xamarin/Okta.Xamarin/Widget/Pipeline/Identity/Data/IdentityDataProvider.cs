// <copyright file="IdentityDataProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Widget.Pipeline.Identity.View;
using Okta.Xamarin.Widget.Pipeline.Session;

namespace Okta.Xamarin.Widget.Pipeline.Identity.Data
{
    public class IdentityDataProvider : IIdentityDataProvider
    {
        public event EventHandler<IdentityDataProviderEventArgs> SessionStarting;

        public event EventHandler<IdentityDataProviderEventArgs> SessionStarted;

        public event EventHandler<IdentityDataProviderEventArgs> SessionStartExceptionThrown;

        public event EventHandler<IdentityDataProviderEventArgs> GetFormDataStarted;

        public event EventHandler<IdentityDataProviderEventArgs> GetFormDataCompleted;

        public event EventHandler<IdentityDataProviderEventArgs> GetFormDataExcpetionThrown;

        public event EventHandler<IdentityDataProviderEventArgs> GetViewModelStarted;

        public event EventHandler<IdentityDataProviderEventArgs> GetViewModelCompleted;

        public event EventHandler<IdentityDataProviderEventArgs> GetViewModelExceptionThrown;

        public IdentityDataProvider(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.ViewModelProvider = serviceProvider.GetService<IIdentityViewModelProvider>();
            this.IdentityClient = serviceProvider.GetService<IIdentityClient>();
            this.SecureSessionProvider = serviceProvider.GetService<SecureSessionProvider>();
        }

        public IServiceProvider ServiceProvider { get; }

        public IIdentityViewModelProvider ViewModelProvider { get; set; }

        public IIdentityClient IdentityClient { get; set; }

        public SecureSessionProvider SecureSessionProvider { get; set; }

        public async Task<IIdentityInteraction> StartSessionAsync()
        {
            try
            {
                this.SessionStarting?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    DataProvider = this,
                });
                IIdentityInteraction identitySession = await this.IdentityClient.InteractAsync();
                identitySession.Save(this.SecureSessionProvider);
                this.SessionStarted?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    DataProvider = this,
                });

                return identitySession;
            }
            catch (Exception ex)
            {
                this.SessionStartExceptionThrown?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    Exception = ex,
                });
                return new IdentityInteraction { Exception = ex };
            }
        }

        public async Task<IIdentityIntrospection> GetFormDataAsync(string interactionHandle)
        {
            try
            {
                this.GetFormDataStarted?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    DataProvider = this,
                });

                IIdentityIntrospection form = await this.IdentityClient.IntrospectAsync(interactionHandle);

                this.GetFormDataCompleted?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    DataProvider = this,
                    Form = form,
                });

                return form;
            }
            catch (Exception ex)
            {
                this.GetFormDataExcpetionThrown?.Invoke(this, new IdentityDataProviderEventArgs
                {
                    Exception = ex,
                });
                return new IdentityIntrospection { Exception = ex };
            }
        }

        public async Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm)
        {
            try
            {
                this.GetViewModelStarted?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    DataProvider = this,
                });

                IIdentityFormViewModel viewModel = await ViewModelProvider.GetViewModelAsync(identityForm);

                this.GetViewModelCompleted?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    DataProvider = this,
                    ViewModel = viewModel,
                });
                return viewModel;
            }
            catch (Exception ex)
            {
                this.GetViewModelExceptionThrown?.Invoke(this, new IdentityViewModelProviderEventArgs
                {
                    DataProvider = this,
                    Exception = ex,
                });
                return new IdentityFormViewModel { Exception = ex };
            }
        }
    }
}
