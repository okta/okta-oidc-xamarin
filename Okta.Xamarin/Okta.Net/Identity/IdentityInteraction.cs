using Newtonsoft.Json;
using Okta.Net.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public class IdentityInteraction : IdentityResponse, IIdentityInteraction
	{
		public IdentityInteraction() : this(new SecureSessionProvider())
		{
		}

		internal IdentityInteraction(SecureSessionProvider sessionProvider)
		{
			this.SecureSessionProvider = sessionProvider;
		}

		[JsonIgnore]
		internal IdentityResponse IdentityResponse { get; set; }

		[JsonIgnore]
		public SecureSessionProvider SecureSessionProvider { get; set; }

		public string CodeVerifier { get; set; }
		public string CodeChallenge { get; set; }
		public string CodeChallengeMethod { get; set; }
		
		public string State { get; set; }

		public string ToJson(bool indented = false)
		{
			return JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
		}

		public void Save()
		{
			Save(SecureSessionProvider);
		}

		public void Save(SecureSessionProvider secureSessionProvider)
		{
			if (SecureSessionProvider != secureSessionProvider)
			{
				SecureSessionProvider = secureSessionProvider;
			}
			secureSessionProvider.Set(State, ToJson());
		}

		public void Load(string state)
		{
			IdentityInteraction identitySession = Load(SecureSessionProvider, state);
			this.CodeVerifier = identitySession.CodeVerifier;
			this.CodeChallenge = identitySession.CodeChallenge;
			this.CodeChallengeMethod = identitySession.CodeChallengeMethod;
			this.InteractionHandle = identitySession.InteractionHandle;
			this.State = identitySession.State;
		}

		public static IdentityInteraction Load(ISessionProvider sessionProvider, string state)
		{
			return sessionProvider.Get<IdentityInteraction>(state);
		}
	}
}
