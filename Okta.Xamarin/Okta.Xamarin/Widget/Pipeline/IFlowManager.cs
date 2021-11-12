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
    public interface IFlowManager : IHasServiceProvider
    {
        event EventHandler<FlowManagerEventArgs> FlowStarting;

        event EventHandler<FlowManagerEventArgs> FlowStartCompleted;

        event EventHandler<FlowManagerEventArgs> FlowStartExceptionThrown;

        event EventHandler<FlowManagerEventArgs> FlowContinuing;

        event EventHandler<FlowManagerEventArgs> FlowContinueCompleted;

        event EventHandler<FlowManagerEventArgs> FlowContinueExceptionThrown;

        event EventHandler<FlowManagerEventArgs> Validating;

        event EventHandler<FlowManagerEventArgs> ValidateCompleted;

        event EventHandler<FlowManagerEventArgs> ValidateExceptionThrown;

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
