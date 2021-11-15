// <copyright file="FlowManager.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Okta.Xamarin.Widget.Pipeline.Data;
using Okta.Xamarin.Widget.Pipeline.Identity;
using Okta.Xamarin.Widget.Pipeline.Identity.Data;
using Okta.Xamarin.Widget.Pipeline.Logging;
using Okta.Xamarin.Widget.Pipeline.Policy;
using Okta.Xamarin.Widget.Pipeline.Session;
using Okta.Xamarin.Widget.Pipeline.View;

namespace Okta.Xamarin.Widget.Pipeline
{
    public class PipelineManager : IPipelineManager
    {
        public PipelineManager()
            : this(Pipeline.ServiceProvider.Default)
        {
        }

        public PipelineManager(IServiceProvider serviceProvider)
            : this(serviceProvider, serviceProvider.GetService<IIdentityClient>(), serviceProvider.GetService<IIdentityDataProvider>(), serviceProvider.GetService<IPolicyProvider>(), serviceProvider.GetService<ISessionProvider>(), serviceProvider.GetService<IStorageProvider>(), serviceProvider.GetService<ILoggingProvider>(), serviceProvider.GetService<IViewProvider>())
        {
        }

        public PipelineManager(IServiceProvider serviceProvider, IIdentityClient idxClient, IIdentityDataProvider dataProvider, IPolicyProvider policyProvider, ISessionProvider sessionProvider, IStorageProvider storageProvider, ILoggingProvider loggingProvider, IViewProvider viewProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.IdentityClient = idxClient;
            this.DataProvider = dataProvider;
            this.PolicyProvider = policyProvider;
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

        public IPolicyProvider PolicyProvider { get; }

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

                IIdentityInteraction interaction = await DataProvider.StartSessionAsync();
                _ = Task.Run(() => this.SessionProvider.Set(interaction.State, interaction.ToJson()));

                IIdentityIntrospection form = await DataProvider.GetFormDataAsync(interaction.InteractionHandle);

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

        public async Task<IPolicyValidationResult> ValidatePolicyConformanceAsync(IIdentityIntrospection identitySession)
        {
            return this.PolicyProvider.ValidatePolicy(new PolicyValidationOptions { IdentityForm = identitySession });
        }
    }
}
