// <copyright file="DefaultIdentityClientShouldActually.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using Okta.Xamarin.Oie.Configuration;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Integration
{
	public class DefaultIdentityClientShouldActually
    {
        [Fact]
        public async Task CallInteractWithoutException()
        {
            DefaultIdentityClient defaultIdentityClient = new DefaultIdentityClient();
            await defaultIdentityClient.InteractAsync();
        }
    }
}
