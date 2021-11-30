// <copyright file="IonFormEformMemberShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit.Ion
{
    public class IonFormEformMemberShould
    {
        /*
 * 

The eform member value is either a Form object or a Link to a Form object that reflects the required object structure of each element in the field’s value array. The name "eform" is short for "element form".
        */

        [Fact]
        public void BeValid()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": {
        ""href"": ""https://example.io/profile"",
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
            IonFormField.IsValid(formFieldJson).Should().BeTrue();
        }

        /*If the field’s type member is not equal to array or set, an Ion parser MUST ignore the eform member.*/
        [Fact]
        public void BeIgnoredIfNotArrayOrSet()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": {
        ""href"": ""https://example.io/profile"",
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
            IonFormField ionFormField = IonFormField.Read(formFieldJson);

            ionFormField["eform"].Should().BeNull();
        }

        [Fact]
        public void NotBeIgnoredIfArrayOrSet()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": {
        ""rel"": [
          ""form""
        ],
        ""type"": ""array"",
        ""href"": ""https://example.io/profile"",
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
            IonFormField ionFormField = IonFormField.Read(formFieldJson);

            ionFormField["eform"].Should().NotBeNull();
        }

        /*  
If the eform member equals null, an Ion parser MUST ignore the eform member.*/
        [Fact]
        public void BeIgnoredIfMemberIsNull()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": null
}";
            IonFormField ionFormField = IonFormField.Read(formFieldJson);

            ionFormField["eform"].Should().BeNull();
        }
        /*
 *     Either:

The JSON object is discovered to be an Ion Link as defined in Section 4 AND its meta member has an internal rel member that contains one of the octet sequences form, edit-form, create-form or query-form, OR:

The JSON object is a member named form inside an Ion Form Field.
 */
        [Fact]
        public void HaveParent()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""form"": {
  ""type"": ""array"",
  ""href"": ""https://example.io/loginAttempts"",
  ""rel"": [
    ""form""
  ],
  ""method"": ""POST"",
  ""value"": [
    {
      ""name"": ""username""
    },
    {
      ""name"": ""password"",
      ""secret"": true
    }
  ]
}
}";
            string expectedForm = @"{
  ""type"": ""array"",
  ""href"": ""https://example.io/loginAttempts"",
  ""rel"": [
    ""form""
  ],
  ""method"": ""POST"",
  ""value"": [
    {
      ""name"": ""username""
    },
    {
      ""name"": ""password"",
      ""secret"": true
    }
  ]
}";
            IonFormField formField = IonFormField.Read(formFieldJson);
            IonMember formMember = formField["form"];
            
            formField.Should().BeSameAs(formMember.Parent);
            IonForm.IsValid(formMember.Value, out IonObject ionForm).Should().BeTrue();
            expectedForm.Should().BeEquivalentTo(ionForm.ToJson(true));
        }

        /*
        If the eform member is not a valid Ion Form object, an Ion parser MUST ignore the eform member.*/

        [Fact]
        public void BeIgnoredIfNotAValidForm()
        {
            string formJson = @"{
  ""type"": ""array"",
  ""href"": ""https://example.io/loginAttempts"", ""rel"":[""form""], ""method"": ""POST"",
  ""value"": [
    { ""name"": ""username"" },
    { ""name"": ""username"" },
    { ""name"": ""password"", ""secret"": true }
  ]
}";

            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": {
      ""type"": ""array"",
      ""href"": ""https://example.io/loginAttempts"", ""rel"":[""form""], ""method"": ""POST"",
      ""value"": [
        { ""name"": ""username"" },
        { ""name"": ""username"" },
        { ""name"": ""password"", ""secret"": true }
      ]
    }
}";
            IonFormValidationResult result = IonForm.Validate(formJson);
            result.Should().NotBeNull();
            result.Success.Should().Be(false); // verify that the form is not valid.  form is the same as form in form field
            IonFormField ionFormField = IonFormField.Read(formFieldJson);
            ionFormField["eform"].Should().BeNull(); // should be null because not a valid form (duplicate username fields)
        }
        /* 

If the eform member exists and is valid, and the etype member does not exist or equals null, an Ion parser MUST assign the field an etype member with a value of object.*/

        [Fact]
        public void HaveEtypeOfObjectIfEformIsValid()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""eform"": {
        ""rel"": [
          ""form""
        ],
        ""type"": ""array"",
        ""href"": ""https://example.io/profile"",
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
            IonFormField ionFormField = IonFormField.Read(formFieldJson);
            ionFormField["eform"].Should().NotBeNull();
            ionFormField["etype"].Value.Should().BeEquivalentTo("object");
        }
        /*
        If the etype member does not equal object, an Ion parser MUST ignore the eform member. */
        [Fact]
        public void IgnoreEtypeIfNotObject()
        {
            string formFieldJson = @"{ 
    ""name"": ""profile"",
    ""etype"": ""monkey"",
    ""eform"": {
        ""rel"": [
          ""form""
        ],
        ""type"": ""array"",
        ""href"": ""https://example.io/profile"",
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
            IonFormField ionFormField = IonFormField.Read(formFieldJson);
            ionFormField["etype"].Should().BeNull();
        }
        }
}
