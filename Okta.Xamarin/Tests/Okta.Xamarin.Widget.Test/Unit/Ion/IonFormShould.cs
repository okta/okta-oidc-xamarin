// <copyright file="IonFormShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit.Ion
{
    public class IonFormShould
    {
        [Fact]
        public void BeFormIfChildOfFormField()
        {
            string expectedFormJson = @"{
  ""href"": ""https://example.io/profile"",
  ""rel"": [
    ""form""
  ],
  ""value"": [
    {
      ""name"": ""firstName""
    },
    {
      ""name"": ""lastName""
    }
  ]
}";

            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""form"": {
        ""href"": ""https://example.io/profile"",
        ""rel"": [
            ""form""
        ],
        ""value"": [
            {
                ""name"": ""firstName""
            },
            {
                ""name"": ""lastName""
            }
        ]
    }
}";

            IonForm ionForm = IonForm.Read(formFieldJson);
            IonMember formMember = ionForm["form"];
            IonFormValidationResult ionFormValidationResult = IonForm.Validate(formMember);

            ionFormValidationResult.Success.Should().BeTrue();
            formMember.Value.ToJson(true).Should().BeEquivalentTo(expectedFormJson);
        }

        [Fact]
        public void BeForm()
        {
            string formJson = @"

{
  ""href"": ""https://example.io/loginAttempts"", ""rel"":[""form""], ""method"": ""POST"",
  ""value"": [
    { ""name"": ""username"" },
    { ""name"": ""password"", ""secret"": true }
  ]
}";
            bool isForm = Widget.Ion.IsForm(formJson);
            isForm.Should().BeTrue();
        }

        [Fact]
        public void NotBeValidFormWithDuplicateFieldNames()
        {
            string formJson = @"

{
  ""href"": ""https://example.io/loginAttempts"", ""rel"":[""form""], ""method"": ""POST"",
  ""value"": [
    { ""name"": ""username"" },
    { ""name"": ""username"" },
    { ""name"": ""password"", ""secret"": true }
  ]
}";
            bool isForm = Widget.Ion.IsForm(formJson);
            isForm.Should().BeFalse();
        }
    }
}
