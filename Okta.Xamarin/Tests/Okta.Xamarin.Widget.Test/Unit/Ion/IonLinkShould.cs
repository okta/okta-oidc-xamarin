// <copyright file="IonLinkShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit.Ion
{
    public class IonLinkShould
    {
        [Fact]
        public void IdentifyLinkJson()
        {
            string json = @"{ ""href"": ""https://example.io/corporations/acme"" }";

            Widget.Ion.IsLink(json).Should().BeTrue();
            IonLink.IsValid(json).Should().BeTrue();
        }

        [Fact]
        public void IonLinkShouldSerializeAsExpected()
        {
            IonLink ionLink = new IonLink("employer", "https://example.io/corporations/acme");
            ionLink.AddSupportingMember("name", "Joe");

            string expected =
            @"{
  ""name"": ""Joe"",
  ""employer"": {
    ""href"": ""https://example.io/corporations/acme""
  }
}";

            string actual = ionLink.ToJson(true);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
