// <copyright file="IriObjectTests.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit.Ion
{
	public class IriObjectShould
    {
        [Fact]
        public void SerializeAsExpected()
        {
            IriObject iriObject = new IriObject("http://test.cxm");
            string expected = @"{
  ""href"": ""http://test.cxm""
}";
            string actual = iriObject.ToJson(true);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ReadString()
        {
            string jsonIriObject = @"{
  ""href"": ""http://test.cxm""
}";
            IriObject readObject = IriObject.Read(jsonIriObject);

            readObject.Href.ToString().Should().BeEquivalentTo("http://test.cxm/"); // because Iri extends Uri a slash is appended
        }
    }
}
