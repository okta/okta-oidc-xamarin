// <copyright file="IonExtensionsShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit.Ion
{
    public class IonExtensionsShould
    {
        [Fact]
        public void ParseJsonArrayAsJArray()
        {
            string arrayJson = @"[
                {
                    ""name"": ""firstName""
                },
                {
                    ""name"": ""lastName""
                }
            ]";

            arrayJson.IsJsonArray(out JArray jArray).Should().BeTrue();
            jArray.Should().NotBeNull();
            jArray.Should().BeOfType<JArray>();
        }

        [Fact]
        public void ParseObjectArrayAsJArray()
        {
            object[] array = new[] { new { name = "user" }, new { name = "baloney" } };
            string json = array.ToJson();

            json.IsJsonArray(out JArray jArray).Should().BeTrue();
            jArray.Count.Should().Be(2);
        }
    }
}
