// <copyright file="TestOktaStateManager.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace Okta.Xamarin.Test
{
    public class TestOktaStateManager: OktaStateManager
    {
        public TestOktaStateManager() : base() { }
        public TestOktaStateManager(string accessToken, string tokenType, string idToken = null, string refreshToken = null, int? expiresIn = null, string scope = null)
        : base(accessToken, tokenType, idToken, refreshToken, expiresIn, scope) 
        {
        }

        public TestOktaStateManager(string accessToken, string refreshToken) : this(accessToken, null, null, refreshToken) { }

        public int GetBasePathCallCount{ get; set; }
        public string CallGetBasePath()
        {
            ++this.GetBasePathCallCount;
            return base.GetBasePath();
        }

        public int RevokeAsyncCallCount { get; set; }

        public override Task RevokeAsync(TokenType tokenType)
        {
            return Task.Run(() => ++RevokeAsyncCallCount);
        }
    }
}
