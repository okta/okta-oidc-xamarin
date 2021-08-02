// <copyright file="TinyIoCContainerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;
using Okta.Xamarin.TinyIoC;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class TinyIocContainerShould
    {
        [Fact]
        public async Task GetInstance()
        {
            TinyIoCContainer container = new TinyIoCContainer();
            OktaContext context = container.Resolve<OktaContext>();
            context.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterInterfaceMapping()
        {
            TinyIoCContainer container = new TinyIoCContainer();
            container.Register<IOktaStateManager, TestOktaStateManager>();

            IOktaStateManager oktaStateManager = container.Resolve<IOktaStateManager>();

            oktaStateManager.Should().NotBeOfType(typeof(OktaStateManager));
            oktaStateManager.Should().BeOfType(typeof(TestOktaStateManager));
        }
    }
}
