// <copyright file="IIdentityClient.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    public interface IIdentityClient
    {
        IdentityClientConfiguration Configuration { get; set; }

        Task<IIdentityInteraction> InteractAsync(IIdentityInteraction state = null);

        Task<IIdentityIntrospection> IntrospectAsync(IIdentityInteraction state);

        Task<IIdentityIntrospection> IntrospectAsync(string interactionHandle);

        Task<TokenResponse> RedeemInteractionCodeAsync(IIdentityInteraction identitySession, string interactionCode);
    }
}
