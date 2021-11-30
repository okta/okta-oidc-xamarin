// <copyright file="IonMemberShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit.Ion
{
    public class IonMemberShould
    {
        [Fact]
        public void BeNamedMember()
        {
            string json = @"{ 
    ""name"": ""profile"",
    ""namedChild"": {
      ""key"": ""value""
}
}";
            IonObject valueObject = IonObject.ReadObject(json);
            IonMember ionMember = valueObject["namedChild"];
            valueObject.Should().BeEquivalentTo(ionMember.Parent);
            valueObject.Should().BeSameAs(ionMember.Parent);
            ionMember.IsMemberNamed("namedChild").Should().BeTrue();
        }

        [Fact]
        public void Serialize()
        {
            IonMember ionMember = "test";

            string expected = @"{
  ""value"": ""test""
}";

            string actual = ionMember.ToJson(true);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ConvertToString()
        {
            IonMember ionMember = "test";

            string expected = "\"value\": \"test\"";

            string actual = ionMember.ToString();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
