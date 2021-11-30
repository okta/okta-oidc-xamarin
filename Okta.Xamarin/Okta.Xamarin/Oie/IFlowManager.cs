// <copyright file="IFlowManager.cs" company="Okta, Inc">
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
    public interface IFlowManager : IHasServiceProvider
    {
        event EventHandler<PipelineManagerEventArgs> FlowStarting;

        event EventHandler<PipelineManagerEventArgs> FlowStartCompleted;

        event EventHandler<PipelineManagerEventArgs> FlowStartExceptionThrown;

        event EventHandler<PipelineManagerEventArgs> FlowContinuing;

        event EventHandler<PipelineManagerEventArgs> FlowContinueCompleted;

        event EventHandler<PipelineManagerEventArgs> FlowContinueExceptionThrown;

        event EventHandler<PipelineManagerEventArgs> Validating;

        event EventHandler<PipelineManagerEventArgs> ValidateCompleted;

        event EventHandler<PipelineManagerEventArgs> ValidateExceptionThrown;

        IIdentityClient IdentityClient { get; }

        IIdentityDataProvider DataProvider { get; }

        ISessionProvider SessionProvider { get; }

        IStorageProvider StorageProvider { get; }

        ILoggingProvider LoggingProvider  { get; }

        IViewProvider ViewProvider { get; }

        Task<IIdentityIntrospection> StartAsync();

        Task<IIdentityIntrospection> Continue(IIdentityInteraction identitySession);

        Task<IIdentityIntrospection> ProceedAsync(IdentityRequest idxRequest);
    }
}
