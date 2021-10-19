using Bam.Ion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Okta.Net.Identity
{
	/// <summary>
	/// Represents the authentication form data model.
	/// </summary>
	public class IdentityIntrospection : IdentityResponse, IIdentityIntrospection
	{
		public IdentityIntrospection() { }
		internal IdentityIntrospection(HttpResponseMessage responseMessage) : base(responseMessage)
		{ }

		public IonObject Ion => IonObject.ReadObject(Raw);
	}
}
