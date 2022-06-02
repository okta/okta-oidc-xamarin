// <copyright file="OktaPlatformBase.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.TinyIoC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    public abstract class OktaPlatformBase
    {
        static OktaPlatformBase()
        {
            OktaContainer = new TinyIoCContainer();
        }

        protected static TinyIoCContainer OktaContainer { get; }

        public static async Task<OktaContext> InitAsync(IOidcClient oidcClient)
        {
            return await InitAsync(oidcClient, OktaContext.Current);
        }

        public static async Task<OktaContext> InitAsync(IOidcClient oidcClient, OktaContext okaContext)
        {
            return await InitAsync(oidcClient, okaContext, OktaContainer);
        }

        internal static async Task<OktaContext> InitAsync(IOidcClient oidcClient, OktaContext oktaContext, TinyIoCContainer oktaContainer)
        {
            if (oidcClient == null)
            {
                throw new ArgumentNullException(nameof(oidcClient));
            }

            if (oidcClient.Config == null)
            {
                throw new ArgumentNullException(nameof(oidcClient.Config));
            }

            OktaContext.RegisterOktaDefaults(oktaContainer, oktaContext);

            oktaContext.OidcClient = oidcClient;
            oktaContext.OktaConfig = oidcClient.Config;

            oktaContainer.Register(oktaContext);
            oktaContainer.Register(oidcClient.Config);
            oktaContainer.Register(oidcClient);

            return oktaContext;
        }
    }
}
