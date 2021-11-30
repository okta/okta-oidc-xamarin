using FluentAssertions;
using Okta.Net.Identity;
using System.IO;
using Xunit;

namespace Okta.Net.Test.Unit.IonForms
{
	public class IdentityIntrospectionShould
	{
		[Fact]
		public void HaveIonObject()
		{
			IdentityIntrospection introspection = new IdentityIntrospection();
			string introspectJson = File.ReadAllText("./Unit/IonForms/test-introspect-response.json");
			introspection.Raw = introspectJson;

			introspection.IonObject.Should().NotBeNull();
		}
	}
}
