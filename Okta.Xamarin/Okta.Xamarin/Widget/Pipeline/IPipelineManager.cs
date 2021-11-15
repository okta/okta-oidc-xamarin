// <copyright file="IFlowManager.cs" company="Okta, Inc">
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
    public interface IPipelineManager : IHasServiceProvider
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

        IPolicyProvider PolicyProvider { get; }

        ISessionProvider SessionProvider { get; }

        IStorageProvider StorageProvider { get; }

        ILoggingProvider LoggingProvider  { get; }

        IViewProvider ViewProvider { get; }

        Task<IIdentityIntrospection> StartAsync();

        Task<IIdentityIntrospection> Continue(IIdentityInteraction identitySession);

        Task<IPolicyValidationResult> ValidatePolicyConformanceAsync(IIdentityIntrospection idxState);

        Task<IIdentityIntrospection> ProceedAsync(IdentityRequest idxRequest);
    }
}
