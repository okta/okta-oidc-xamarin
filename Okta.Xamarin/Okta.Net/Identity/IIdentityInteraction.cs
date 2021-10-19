using Okta.Net.Session;
using System;

namespace Okta.Net.Identity
{
	public interface IIdentityInteraction : IIdentityResponse
	{
		SecureSessionProvider SecureSessionProvider { get; set; }

		string State { get; set; }
		string CodeChallenge { get; set; }
		string CodeChallengeMethod { get; set; }
		string CodeVerifier { get; set; }

		void Load(string state);
		void Save();
		void Save(SecureSessionProvider secureSessionProvider);
		string ToJson(bool indented = false);
	}
}