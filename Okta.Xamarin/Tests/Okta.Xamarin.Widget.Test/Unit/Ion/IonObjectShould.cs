// <copyright file="IonObjectShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit.Ion
{
    public class IonObjectShould
    {
        [Fact]
        public void HaveParentedMembers()
        {
            string json = @"{
  ""value"": ""test value"",
  ""baloney"": ""sandwich""
}";
            IonObject value = IonObject.ReadObject(json);

            IonMember valueMember = value["value"];
            IonMember baloneyMember = value["baloney"];

            valueMember.Should().NotBeNull();
            baloneyMember.Should().NotBeNull();

            value.Should().BeSameAs(valueMember.Parent);
            value.Should().BeEquivalentTo(valueMember.Parent);
            value.Should().BeSameAs(baloneyMember.Parent);
            value.Should().BeEquivalentTo(baloneyMember.Parent);
        }

        [Fact]
        public void AddMember()
        {
            IonObject value = "test value";
            value.AddMember("baloney", "sandwich");
            string expected = @"{
  ""value"": ""test value"",
  ""baloney"": ""sandwich""
}";
            value.ToJson(true).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void OutputIonJsonForConvertedString()
        {
            IonObject value = "hello"; // implicit operator converts string to IonObject
            string expected = @"{
  ""value"": ""hello""
}";
            string memberJson = value.ToIonJson(true);
            memberJson.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void OutputIonJson()
        {
            string json =
@"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23""
}";

            IonObject value = IonObject.ReadObject(json);
            string output = value.ToIonJson(true);
            output.Should().BeEquivalentTo(json);
        }

        [Fact]
        public void AddSupportingMembers()
        {
            string expected = "{\r\n  \"value\": \"Hello\",\r\n  \"lang\": \"en\"\r\n}";
            IonObject value = "Hello";
            value.SetSupportingMember("lang", "en");
            string output = value.ToIonJson(true);
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SetTypeSupportingMember()
        {
            string json =
                @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23""
}";

            IonObject value = IonObject.ReadObject(json);
            value.SetTypeContext<TestPerson>();

            string actual = value.ToIonJson(true);
            string expected = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23"",
  ""type"": ""TestPerson""
}";
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SetType()
        {
            string json =
                @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23""
}";

            IonObject value = IonObject.ReadObject(json);
            value.Type = typeof(TestPerson);

            string actual = value.ToIonJson(true);
            string expected = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23"",
  ""type"": ""TestPerson""
}";
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ParseSupportingMembers()
        {
            string input = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23"",
  ""type"": ""TestPerson""
}";
            IonObject<TestPerson> testPerson = IonObject.ReadObject<TestPerson>(input);
            testPerson.Members.Count.Should().Be(3);
            testPerson.SupportingMembers.Count.Should().Be(1);

            testPerson["firstName"].Value.Should().BeEquivalentTo("Bob");
            testPerson["lastName"].Value.Should().BeEquivalentTo("Smith");
            KeyValuePair<string, object> keyValuePair = testPerson.SupportingMembers.First();
            keyValuePair.Key.Should().BeEquivalentTo("type");
            keyValuePair.Value.Should().BeEquivalentTo("TestPerson");
        }

        [Fact]
        public void ConvertToInstance()
        {
            string input = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23"",
  ""type"": ""TestPerson""
}";
            IonObject<TestPerson> testPersonIonObject = IonObject.ReadObject<TestPerson>(input);
            TestPerson testPerson = testPersonIonObject.Value;
            testPerson.FirstName.Should().BeEquivalentTo("Bob");
            testPerson.LastName.Should().BeEquivalentTo("Smith");
            testPerson.BirthDate.Should().BeEquivalentTo("1980-01-23");
        }

        [Fact]
        public void AddSupportingMember()
        {
            IonObject<string> hello = "hello";
            hello.AddSupportingMember("lang", "en");
            string expected = @"{
  ""value"": ""hello"",
  ""lang"": ""en""
}";
            string actual = hello.ToIonJson(true);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
