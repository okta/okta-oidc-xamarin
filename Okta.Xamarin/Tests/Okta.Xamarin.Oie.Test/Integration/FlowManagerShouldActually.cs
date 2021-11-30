// <copyright file="FlowManagerShouldActually.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Okta.Xamarin.Oie;
using Okta.Xamarin.Oie.Client;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Integration
{
    public class FlowManagerShouldActually
    {
        [Fact]
        public async Task GetFormOnStart()
        {
            FlowManager flowManager = new FlowManager();

            IIdentityIntrospection form = await flowManager.StartAsync();

            form.Should().NotBeNull();
            form.HasException.Should().BeFalse();
            form.Raw.Should().NotBeNullOrWhiteSpace();
        }
    }
}
