using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// A high level container providing access to Okta functionality.
    /// </summary>
    /// <typeparam name="TClient">The type of the client.</typeparam>
    /// <typeparam name="TConfig">The type of the config.</typeparam>
    public class OktaContext<TClient, TConfig> : OktaContext
        where TClient : IOidcClient, new()
        where TConfig : IOktaConfig, new()
    {
        /// <summary>
        /// Gets or sets the default client.
        /// </summary>
        public new TClient OidcClient { get; set; }

        /// <summary>
        /// Gets or sets the default okta config.
        /// </summary>
        public new TConfig OktaConfig { get; set; }

        /// <summary>
        /// Sign in using the specified config.
        /// </summary>
        /// <param name="oktaConfig">The config to use.</param>
        /// <returns>OktaState.</returns>
        public async Task<OktaStateManager> SignIn(TConfig oktaConfig = default)
        {
            return await this.SignIn(new TClient
            {
                Config = oktaConfig == null ? this.OktaConfig : oktaConfig,
            });
        }

        /// <summary>
        /// Sign in using the specified client.
        /// </summary>
        /// <param name="oidcClient">The client to use.</param>
        /// <returns>OktaState.</returns>
        public async Task<OktaStateManager> SignIn(TClient oidcClient = default)
        {
            oidcClient = oidcClient == null ? this.OidcClient : oidcClient;
            return await this.SignInAsync((IOidcClient)oidcClient);
        }
    }
}
