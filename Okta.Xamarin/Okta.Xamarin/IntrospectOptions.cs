using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Options for introspection.
    /// </summary>
    public class IntrospectOptions
    {
        public IntrospectOptions()
        {
            AuthorizationServerId = "default";
        }

        /// <summary>
        /// Gets or sets the token that is the target of introspection.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the type of the target token.
        /// </summary>
        public TokenKind TokenType{ get; set; } 

        /// <summary>
        /// Gets or sets the authorization server id.
        /// </summary>
        public string AuthorizationServerId{ get; set; }
    }
}
