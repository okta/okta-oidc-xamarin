// <copyright file="OktaContextShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class OktaContextShould
    {
        [Fact]
        public void RaiseSignInEventsOnSignIn()
        {
            OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));
            OktaContext.Init(client);
            bool? signInStartedEventRaised = false;
            bool? signInCompletedEventRaised = false;
            OktaContext.AddSignInStartedListener((sender, eventArgs) => signInStartedEventRaised = true);
            OktaContext.AddSignInCompletedListener((sender, eventArgs) => signInCompletedEventRaised = true);

            OktaContext.Current.SignIn();
            client.CurrentTask_Accessor.SetResult(new OktaState("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));

            Assert.True(signInStartedEventRaised);
            Assert.True(signInCompletedEventRaised);
        }

        [Fact]
        public void RaiseSignOutEventsOnSignOut()
        {
            OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));
            OktaContext.Init(client);
            bool? signOutStartedEventRaised = false;
            bool? signOutCompletedEventRaised = false;
            OktaContext.AddSignOutStartedListener((sender, eventArgs) => signOutStartedEventRaised = true);
            OktaContext.AddSignOutCompletedListener((sender, eventArgs) => signOutCompletedEventRaised = true);

            OktaContext.Current.SignIn();
            client.CurrentTask_Accessor.SetResult(new OktaState("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));
            OktaContext.Current.SignOut();
            client.CurrentTask_Accessor.SetResult(new OktaState(string.Empty, string.Empty));

            Assert.True(signOutStartedEventRaised);
            Assert.True(signOutCompletedEventRaised);
        }
    }
}
