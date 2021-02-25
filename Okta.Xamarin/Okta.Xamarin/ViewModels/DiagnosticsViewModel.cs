// <copyright file="DiagnosticsViewModel.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class DiagnosticsViewModel : BaseViewModel
    {
        public DiagnosticsViewModel(DiagnosticsPage diagnosticsPage)
        {
            this.Page = diagnosticsPage;
            this.StateManager = OktaContext.Current.StateManager;

            // TODO: refactor this so handlers are only attached once.
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

        public RevokeAccessTokenCommand RevokeAccessTokenCommand => new RevokeAccessTokenCommand();

		public RevokeRefreshTokenCommand RevokeRefreshTokenCommand => new RevokeRefreshTokenCommand();

        public GetClaimsPrincipalCommand GetClaimsPrincipalCommand => new GetClaimsPrincipalCommand();

        public GetUserCommand GetUserCommand => new GetUserCommand();

        public Command<string> IntrospectCommand => new Command<string>(async (tokenType) => await OktaContext.Current.IntrospectAsync((TokenKind)Enum.Parse(typeof(TokenKind), tokenType)));

        public Command<string> RenewCommand => new Command<string>(async (refreshIdToken) => await OktaContext.Current.RenewAsync(refreshIdToken.Equals("True")));

        IOktaStateManager stateManager;

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
