// <copyright file="FlowManager.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Oie.Data;
using Okta.Xamarin.Oie.Client;
using Okta.Xamarin.Oie.Client.Data;
using Okta.Xamarin.Oie.Logging;
using Okta.Xamarin.Oie.Session;
using Okta.Xamarin.Oie.Views;

namespace Okta.Xamarin.Oie
{
    public class FlowManager : IFlowManager
    {
        public FlowManager()
            : this(Oie.ServiceProvider.Default)
        {
        }

        public FlowManager(IServiceProvider serviceProvider)
            : this(serviceProvider, serviceProvider.GetService<IIdentityClient>(), serviceProvider.GetService<IIdentityDataProvider>(),  serviceProvider.GetService<ISessionProvider>(), serviceProvider.GetService<IStorageProvider>(), serviceProvider.GetService<ILoggingProvider>(), serviceProvider.GetService<IViewProvider>())
        {
        }

        public FlowManager(IServiceProvider serviceProvider, IIdentityClient idxClient, IIdentityDataProvider dataProvider, ISessionProvider sessionProvider, IStorageProvider storageProvider, ILoggingProvider loggingProvider, IViewProvider viewProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.IdentityClient = idxClient;
            this.DataProvider = dataProvider;
            this.SessionProvider = sessionProvider;
            this.StorageProvider = storageProvider;
            this.LoggingProvider = loggingProvider;
            this.ViewProvider = viewProvider;
        }

        public event EventHandler<PipelineManagerEventArgs> FlowStarting;

        public event EventHandler<PipelineManagerEventArgs> FlowStartCompleted;

        public event EventHandler<PipelineManagerEventArgs> FlowStartExceptionThrown;

        public event EventHandler<PipelineManagerEventArgs> FlowContinuing;

        public event EventHandler<PipelineManagerEventArgs> FlowContinueCompleted;

        public event EventHandler<PipelineManagerEventArgs> FlowContinueExceptionThrown;

        public event EventHandler<PipelineManagerEventArgs> Validating;

        public event EventHandler<PipelineManagerEventArgs> ValidateCompleted;

        public event EventHandler<PipelineManagerEventArgs> ValidateExceptionThrown;

        public IServiceProvider ServiceProvider { get; }

        public IIdentityClient IdentityClient { get; }

        public IIdentityDataProvider DataProvider { get; }

        public ISessionProvider SessionProvider { get; }

        public IStorageProvider StorageProvider { get; }

        public ILoggingProvider LoggingProvider { get; }

        public IViewProvider ViewProvider { get; }

        public Task<IIdentityIntrospection> Continue(IIdentityInteraction identitySession)
        {
            throw new NotImplementedException();
        }

        public Task<IIdentityIntrospection> ProceedAsync(IdentityRequest identityRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<IIdentityIntrospection> StartAsync()
        {
            try
            {
                this.FlowStarting?.Invoke(this, new PipelineManagerEventArgs
                {
                    FlowManager = this,
                });

                IIdentityInteraction interaction = await this.DataProvider.StartSessionAsync();
                _ = Task.Run(() => this.SessionProvider.Set(interaction.State, interaction.ToJson()));

                IIdentityIntrospection form = await this.DataProvider.GetFormDataAsync(interaction.InteractionHandle);

                this.FlowStartCompleted?.Invoke(this, new PipelineManagerEventArgs
                {
                    FlowManager = this,
                    Session = interaction,
                    Form = form,
                });

                return form;
            }
            catch (Exception ex)
            {
                this.FlowStartExceptionThrown?.Invoke(this, new PipelineManagerEventArgs
                {
                    FlowManager = this,
                    Exception = ex,
                });

                return new IdentityIntrospection { Exception = ex };
            }
        }
    }
}
