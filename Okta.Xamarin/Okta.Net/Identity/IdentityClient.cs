using Microsoft.IdentityModel.Tokens;
using Okta.Net.Data;
using Okta.Net.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Okta.Net.Configuration;

namespace Okta.Net.Identity
{
	public class IdentityClient : IIdentityClient
	{
		protected const string IonJsonMediaType = "application/ion+json; okta-version=1.0.0";
		protected const string JsonMediaType = "application/json";

		private HttpClient _httpClient;

		public IdentityClient(IIdentityClientConfigurationProvider clientConfigurationProvider = null) : this(new HttpMessageInterceptor(), false)
		{
			IdentityClientConfigurationProvider = clientConfigurationProvider ?? new ProfileIdentityClientConfigurationProvider();
		}

		public IdentityClient(HttpMessageHandler httpMessageHandler, bool disposeHandler = false)
		{
			this.IdentityClientConfigurationProvider = new ProfileIdentityClientConfigurationProvider();
			this._httpClient = new HttpClient(httpMessageHandler, disposeHandler);
		}

		public event EventHandler<InteractEventArgs> InteractStarted;
		public event EventHandler<InteractEventArgs> InteractCompleted;
		public event EventHandler<InteractEventArgs> InteractExceptionThrown;

		public event EventHandler<IntrospectEventArgs> IntrospectStarted;
		public event EventHandler<IntrospectEventArgs> IntrospectCompleted;
		public event EventHandler<IntrospectEventArgs> IntrospectExceptionThrown;

		public event EventHandler<RedeemInteractionCodeEventArgs> RedeemInteractionCodeStarted;
		public event EventHandler<RedeemInteractionCodeEventArgs> RedeemInteractionCodeCompleted;
		public event EventHandler<RedeemInteractionCodeEventArgs> RedeemInteractionCodeExceptionThrown;

		IdentityClientConfiguration _configuration;
		object _configurationLock = new object();
		public IdentityClientConfiguration Configuration
		{
			get
			{
				if (_configuration == null)
				{
					lock (_configurationLock)
					{
						if (_configuration == null)
						{
							_configuration = IdentityClientConfigurationProvider.GetConfiguration();
						}
					}
				}

				return _configuration;
			}
			set => _configuration = value;
		}

		public async Task<IIdentityInteraction> InteractAsync(IIdentityInteraction session = null)
		{
			string state = session?.State ?? GenerateSecureRandomString(16);
			string codeVerifier = GenerateSecureRandomString(86);
			string codeChallenge = GenerateCodeChallenge(codeVerifier, out string codeChallengeMethod);

			try
			{
				InteractStarted?.Invoke(this, new InteractEventArgs
				{
					ClientId = Configuration.ClientId,
					Scopes = Configuration.Scopes,
					CodeChallengeMethod = codeChallengeMethod,
					RedirectUri = Configuration.RedirectUri,
					State = state
				});

				Dictionary<string, string> requestBody = new Dictionary<string, string>()
				{
					{ "scope", string.Join(" ", Configuration.Scopes) },
					{ "client_id", Configuration.ClientId },
					{ "code_challenge_method", codeChallengeMethod },
					{ "code_challenge", codeChallenge },
					{ "redirect_uri", Configuration.RedirectUri },
					{ "state", state }
				};

				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, GetRequestUri("v1/interact"));
				requestMessage.Content = new FormUrlEncodedContent(requestBody);
				HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

				IdentityResponse identityResponse = new IdentityResponse(responseMessage);
				if (identityResponse.HasException)
				{
					throw identityResponse.Exception;
				}
				InteractCompleted?.Invoke(this, new InteractEventArgs
				{
					Scopes = Configuration.Scopes,
					CodeChallengeMethod = codeChallengeMethod,
					RedirectUri = Configuration.RedirectUri,
					State = state
				});

				IdentityInteraction identitySession = identityResponse.As<IdentityInteraction>();
				identitySession.Raw = identityResponse.Raw;
				identitySession.CodeVerifier = codeVerifier;
				identitySession.CodeChallenge = codeChallenge;
				identitySession.CodeChallengeMethod = codeChallengeMethod;
				identitySession.State = state;
				return identitySession;
			}
			catch (Exception ex)
			{
				InteractExceptionThrown?.Invoke(this, new InteractEventArgs
				{
					Scopes = Configuration.Scopes,
					CodeChallengeMethod = codeChallengeMethod,
					RedirectUri = Configuration.RedirectUri,
					State = state,
					Exception = ex
				});

				return new IdentityInteraction() { Exception = ex };
			}
		}

		public async Task<IIdentityIntrospection> IntrospectAsync(IIdentityInteraction state)
		{
			return await IntrospectAsync(state.InteractionHandle);
		}

		public async Task<IIdentityIntrospection> IntrospectAsync(string interactionHandle)
		{
			try
			{
				IntrospectStarted?.Invoke(this, new IntrospectEventArgs
				{
					InteractionHandle = interactionHandle,
				});

				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://{Configuration.OktaDomain}/idp/idx/introspect");
				requestMessage.Headers.Add("Accept", IonJsonMediaType);
				string json = JsonConvert.SerializeObject(new { interactionHandle = interactionHandle });
				requestMessage.Content = new StringContent(json, Encoding.UTF8, JsonMediaType);
				HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

				IdentityIntrospection identityForm = new IdentityIntrospection(responseMessage);
				identityForm.EnsureSuccess();

				IntrospectCompleted?.Invoke(this, new IntrospectEventArgs
				{
					InteractionHandle = interactionHandle,
					IdxState = identityForm
				});

				return identityForm;
			}
			catch (Exception ex)
			{
				IntrospectExceptionThrown?.Invoke(this, new IntrospectEventArgs
				{
					InteractionHandle = interactionHandle,
					Exception = ex
				});

				return new IdentityIntrospection { Exception = ex };
			}
		}

		public async Task<TokenResponse> RedeemInteractionCodeAsync(IIdentityInteraction identitySession, string interactionCode)
		{
			string responseBody = null;
			try
			{
				RedeemInteractionCodeStarted?.Invoke(this, new RedeemInteractionCodeEventArgs
				{
					IdentityClient = this,
					IdentitySession = identitySession,
					InteractionCode = interactionCode
				});

				if (string.IsNullOrEmpty(interactionCode))
				{
					throw new ArgumentNullException(nameof(interactionCode), "Interaction code not specified.");
				}

				if (string.IsNullOrEmpty(identitySession.CodeVerifier))
				{
					throw new ArgumentNullException(nameof(identitySession.CodeVerifier), "CodeVerifier not specified.");
				}

				Dictionary<string, string> requestBody = new Dictionary<string, string>()
				{
					{ "grant_type", "interaction_code" },
					{ "client_id", Configuration.ClientId },
					{ "interaction_code", interactionCode },
					{ "code_verifier", identitySession.CodeVerifier }
				};

				if (!string.IsNullOrEmpty(Configuration.ClientSecret))
				{
					requestBody.Add("client_secret", Configuration.ClientSecret);
				}

				HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, GetRequestUri("v1/interact"));
				requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				requestMessage.Content = new FormUrlEncodedContent(requestBody);
				HttpResponseMessage responseMessage = await this._httpClient.SendAsync(requestMessage);
				responseBody = await responseMessage.Content.ReadAsStringAsync();
				TokenResponse tokenResponse = new TokenResponse(responseMessage);
				tokenResponse.EnsureSuccess();

				RedeemInteractionCodeCompleted?.Invoke(this, new RedeemInteractionCodeEventArgs
				{
					IdentityClient = this,
					TokenResponse = tokenResponse,
					IdentitySession = identitySession,
					InteractionCode = interactionCode
				});

				return tokenResponse;
			}
			catch (Exception ex)
			{
				RedeemInteractionCodeExceptionThrown?.Invoke(this, new RedeemInteractionCodeEventArgs
				{
					IdentityClient = this,
					IdentitySession = identitySession,
					InteractionCode = interactionCode
				});

				return new TokenResponse { Raw = responseBody, Exception = ex };
			}
		}

		protected IIdentityClientConfigurationProvider IdentityClientConfigurationProvider { get; set; }

		private string GenerateSecureRandomString(int length)
		{
			using (RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider())
			{
				byte[] data = new byte[length];
				randomNumberGenerator.GetBytes(data);

				return Base64UrlEncoder.Encode(data);
			}
		}

		private string GenerateCodeChallenge(string codeVerifier, out string codeChallengeMethod)
		{
			codeChallengeMethod = "S256";

			using(SHA256 algorithm = SHA256.Create())
			{
				byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

				return Base64UrlEncoder.Encode(data);
			}
		}

		private Uri GetRequestUri(string path)
		{
			return GetRequestUri(Configuration.IssuerUri, path);
		}

		private Uri GetRequestUri(string issuerUri, string path)
		{
			string result = issuerUri;
			if (IsDomainRoot(issuerUri))
			{
				result = Path.Combine(result, "oauth2", path);
			}
			else
			{
				result = Path.Combine(result, path);
			}

			result = result.Replace("\\", "/");

			return new Uri(result);
		}

		private bool IsDomainRoot(string uri)
		{
			string path = new Uri(uri).AbsolutePath;
			string[] splitUri = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (splitUri.Length >= 2 &&
			"oauth2".Equals(splitUri[0]) &&
			!string.IsNullOrEmpty(splitUri[1]))
			{
				return false;
			}

			return true;
		}
	}
}
