// <copyright file="FlowManagerEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Okta.Xamarin.Oie.Client;

namespace Okta.Xamarin.Oie
{
    public class PipelineManagerEventArgs : EventArgs
    {
        public IFlowManager FlowManager { get; set; }

        public IIdentityInteraction Session { get; set; }

        public IIdentityIntrospection Form { get; set; }

        public Exception Exception { get; set; }
    }
}
