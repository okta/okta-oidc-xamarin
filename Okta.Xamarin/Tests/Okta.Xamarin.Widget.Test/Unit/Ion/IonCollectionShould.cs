// <copyright file="IonCollectionShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit.Ion
{
    public class IonCollectionShould
    {
        [Fact]
        public void SerializeEmptyAsExpected()
        {
            string expected = @"{
  ""value"": []
}";
            IonCollection collection = new IonCollection();

            string actual = collection.ToIonJson(true);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ContainValue()
        {
            string testValue = "test Value";

            IonCollection ionCollection = new IonCollection();

            ionCollection.Add(testValue);

            ionCollection.Contains(testValue).Should().BeTrue();
        }

        [Fact]
        public void ContainObjectValue()
        {
            string bob = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith""
}";
            string jane = @"{
  ""firstName"": ""Jane"",
  ""lastName"": ""Doe""
}";
            IonCollection ionCollection = new IonCollection();
            ionCollection.Add<TestPerson>(bob);
            ionCollection.Add<TestPerson>(jane);

            ionCollection.Contains(bob).Should().BeTrue();
            ionCollection.Contains(jane).Should().BeTrue();
        }

        [Fact]
        public void ContainIonObjectValue()
        {
            string bob = @"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith""
}";
            string jane = @"{
  ""firstName"": ""Jane"",
  ""lastName"": ""Doe""
}";
            IonObject<TestPerson> bobObj = new IonObject<TestPerson>(bob);
            IonObject<TestPerson> janeObj = new IonObject<TestPerson>(jane);

            IonCollection ionCollection = new IonCollection();
            ionCollection.Add(bobObj);
            ionCollection.Add(janeObj);

            ionCollection.Contains(bobObj).Should().BeTrue();
            ionCollection.Contains(janeObj).Should().BeTrue();
            ionCollection.Contains(bob).Should().BeTrue();
            ionCollection.Contains(jane).Should().BeTrue();
        }

        [Fact]
        public void HaveElementMetaData()
        {
           string collectionJson = @"{
    ""eform"": { ""href"": ""https://example.io/users/form"" },
    ""value"": [
        {
        ""firstName"": ""Bob"",
        ""lastName"": ""Smith"",
      },
      {
        ""firstName"": ""Jane"",
        ""lastName"": ""Doe"",
      }
    ]
}";
           IonCollection ionCollection = IonCollection.Read(collectionJson);
           ionCollection.Count.Should().Be(2);
           ionCollection.MetaDataElements.Should().NotBeNull();
           ionCollection.MetaDataElements.Count.Should().Be(1);
        }

        [Fact]
        public void SerializeWithMetaData()
        {
            string collectionJson = @"{
  ""eform"": {
    ""href"": ""https://example.io/users/form""
  },
  ""value"": [
    {
      ""firstName"": ""Bob"",
      ""lastName"": ""Smith""
    },
    {
      ""firstName"": ""Jane"",
      ""lastName"": ""Doe""
    }
  ]
}";
            IonCollection ionCollection = IonCollection.Read(collectionJson);
            string json = ionCollection.ToIonJson(true);

            ionCollection.Count.Should().Be(2);
            ionCollection.MetaDataElements.Should().NotBeNull();
            ionCollection.MetaDataElements.Count.Should().Be(1);
            json.Should().BeEquivalentTo(collectionJson);
        }

        [Fact]
        public void SerializeEmptyWithMetaData()
        {
            string sourceJson = @"{
  ""self"": {
    ""href"": ""https://example.io/users"",
    ""rel"": [
      ""collection""
    ]
  },
  ""value"": []
}";
            IonCollection ionCollection = IonCollection.Read(sourceJson);
            string json = ionCollection.ToIonJson(true);

            ionCollection.Count.Should().Be(0);
            ionCollection.MetaDataElements.Should().NotBeNull();
            ionCollection.MetaDataElements.Count.Should().Be(1);
            json.Should().BeEquivalentTo(sourceJson);
        }

        [Fact]
        public void SerializeAndDeserialize()
        {
            string json = @"{
  ""self"": {
    ""href"": ""https://example.io/users"",
    ""rel"": [
      ""collection""
    ]
  },
  ""desc"": ""Showing 25 of 218 users.  Use the 'next' link for the next page."",
  ""offset"": 0,
  ""limit"": 25,
  ""size"": 218,
  ""first"": {
    ""href"": ""https://example.io/users"",
    ""rel"": [
      ""collection""
    ]
  },
  ""previous"": null,
  ""next"": {
    ""href"": ""https://example.io/users?offset=25"",
    ""rel"": [
      ""collection""
    ]
  },
  ""last"": {
    ""href"": ""https://example.io/users?offset=200"",
    ""rel"": [
      ""collection""
    ]
  },
  ""value"": [
    {
      ""self"": {
        ""href"": ""https://example.io/users/1""
      },
      ""firstName"": ""Bob"",
      ""lastName"": ""Smith"",
      ""birthDate"": ""1977-04-18""
    },
    {
      ""self"": {
        ""href"": ""https://example.io/users/25""
      },
      ""firstName"": ""Jane"",
      ""lastName"": ""Doe"",
      ""birthDate"": ""1980-01-23""
    }
  ]
}";

            IonCollection ionCollection = IonCollection.Read(json);
            string output = ionCollection.ToIonJson(true);

            ionCollection.Count.Should().Be(2);
            ionCollection.MetaDataElements.Should().NotBeNull();
            ionCollection.MetaDataElements.Count.Should().Be(9);
            output.Should().BeEquivalentTo(json);
        }
    }
}
