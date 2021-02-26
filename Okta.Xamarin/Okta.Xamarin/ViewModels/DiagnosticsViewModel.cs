// <copyright file="DiagnosticsViewModel.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Okta.Xamarin.Views;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    /// <summary>
    /// View model for Diagnostics page.
    /// </summary>
    public class DiagnosticsViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsViewModel"/> class.
        /// </summary>
        /// <param name="diagnosticsPage">The page.</param>
        public DiagnosticsViewModel(DiagnosticsPage diagnosticsPage)
        {
            this.Page = diagnosticsPage;
            this.StateManager = OktaContext.Current.StateManager;

            // TODO: refactor this so handlers are only attached once. OKTA-373403
            OktaContext.Current.GetUserCompleted += (sender, args) =>
            {
                this.UserInfo = args.UserInfo;
                this.ClaimsPrincipal = args.UserInfo as ClaimsPrincipal; // user info may be Dictionary<string, object> or ClaimsPrincipal or generic type; will be null if incorrect 'as' cast
            };
            OktaContext.Current.IntrospectCompleted += (sender, args) =>
            {
                this.Page.DisplayData(args.Response as Dictionary<string, object>, "Introspection");
            };
            OktaContext.Current.RenewCompleted += (sender, args) =>
            {
                this.Page.DisplayData(args.Response.ToDictionary(), "Renewal");
            };
        }

        protected DiagnosticsPage Page { get; }

        /// <summary>
        /// Gets the revoke access token command.
        /// </summary>
        public RevokeAccessTokenCommand RevokeAccessTokenCommand => new RevokeAccessTokenCommand();

        /// <summary>
        /// Gets the revoke refresh token command.
        /// </summary>
        public RevokeRefreshTokenCommand RevokeRefreshTokenCommand => new RevokeRefreshTokenCommand();

        /// <summary>
        /// Gets the `get claims principal command`.
        /// </summary>
        public GetClaimsPrincipalCommand GetClaimsPrincipalCommand => new GetClaimsPrincipalCommand();

        /// <summary>
        /// Gets the `get user command`.
        /// </summary>
        public GetUserCommand GetUserCommand => new GetUserCommand();

        /// <summary>
        /// Gets the introspect command.
        /// </summary>
        public Command<string> IntrospectCommand => new Command<string>(async (tokenType) => await OktaContext.Current.IntrospectAsync((TokenKind)Enum.Parse(typeof(TokenKind), tokenType)));

        /// <summary>
        /// Gets the renew command.
        /// </summary>
        public Command<string> RenewCommand => new Command<string>(async (refreshIdToken) => await OktaContext.Current.RenewAsync(refreshIdToken.Equals("True")));

        IOktaStateManager stateManager;

        /// <summary>
        /// Gets or sets the Okta state manager.
        /// </summary>
        public IOktaStateManager StateManager
        {
            get { return stateManager; }

            set
            {
                stateManager = value;
                OnPropertyChanged(nameof(StateManager));
            }
        }

        object userInfo;

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        public object UserInfo
        {
            get
            {
                return userInfo;
            }

            set
            {
                userInfo = value;
                OnPropertyChanged(nameof(UserInfo));
            }
        }

        ClaimsPrincipal claimsPrincipal;

        /// <summary>
        /// Gets or sets the claims principal.
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal
        {
            get { return claimsPrincipal; }

            set
            {
                claimsPrincipal = value;
                OnPropertyChanged(nameof(ClaimsPrincipal));
            }
        }

        string introspectResponse;

        /// <summary>
        /// Gets or sets the introspect response json.
        /// </summary>
        public string IntrospectResponseJson
        {
            get{ return introspectResponse; }
            set
            {
                introspectResponse = value;
                OnPropertyChanged(nameof(IntrospectResponseJson));
            }
        }
    }
}
