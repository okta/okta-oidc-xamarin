﻿// <copyright file="OktaState.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// State manager interface.
    /// </summary>
    public interface IOktaStateManager
    {
        string AccessToken { get; }

        IOktaConfig Config { get; set; }

        IOidcClient Client { get; set; }

        DateTime? Expires { get; }

        string IdToken { get; }

        bool IsAuthenticated { get; }

        HttpResponseMessage LastApiResponse { get; }

        string RefreshToken { get; }

        string Scope { get; }

        string TokenType { get; }

        void Clear();

        string GetToken(TokenKind tokenKind);

        Task<T> GetUserAsync<T>(string authorizationServerId = "default");

        Task<Dictionary<string, object>> GetUserAsync(string authorizationServerId = "default");

        Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string authorizationServerId = "default");

        Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokeKind, string authorizationServerId = "default");

        Task<RenewResponse> RenewAsync(bool renewIdToken = false, string authorizationServerId = "default");

        Task RevokeAsync(TokenKind tokenType);

        Task WriteToSecureStorageAsync();
    }
}
