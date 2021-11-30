// <copyright file="RedeemInteractionCodeEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Okta.Xamarin.Oie.Client;

namespace Okta.Xamarin.Oie
{
    public class RedeemInteractionCodeEventArgs : EventArgs
    {
        public IIdentityClient IdentityClient { get; set; }

        public IIdentityInteraction IdentitySession { get; set; }

        public TokenResponse TokenResponse { get; set; }

        public string InteractionCode { get; set; }
    }
}
