// <copyright file="IdentityIntrospectionShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.IO;
using FluentAssertions;
using Okta.Xamarin.Oie.Client;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit.Ion
{
    public class IdentityIntrospectionShould
    {
        [Fact]
        public void HaveIonObject()
        {
            IdentityIntrospection introspection = new IdentityIntrospection();
            string introspectJson = File.ReadAllText("./Unit/Ion/test-introspect-response.json");
            introspection.Raw = introspectJson;

            introspection.IonObject.Should().NotBeNull();
        }
    }
}
