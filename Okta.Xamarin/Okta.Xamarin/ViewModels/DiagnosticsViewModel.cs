// <copyright file="DiagnosticsViewModel.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Views;
using System;
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
            OktaContext.Current.GetUserCompleted += (sender, args) =>
            {
				this.userInfo = args.UserInfo;
				this.ClaimsPrincipal = args.UserInfo as ClaimsPrincipal; // user info may be Dictionary<string, object> or ClaimsPrincipal or generic type; will be null if incorrect 'as' cast
			};
        }

        protected DiagnosticsPage Page { get; }

        public RevokeAccessTokenCommand RevokeAccessTokenCommand => new RevokeAccessTokenCommand();

        public GetClaimsPrincipalCommand GetClaimsPrincipalCommand => new GetClaimsPrincipalCommand();

		public GetUserCommand GetUserCommand => new GetUserCommand();

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

        public string UserName
        {
            get
            {
                return ClaimsPrincipal?.Identity?.Name;
            }
        }

        public ClaimsPrincipal ClaimsPrincipal
        {
            get { return claimsPrincipal; }

            set
            {
                claimsPrincipal = value;
                OnPropertyChanged(nameof(ClaimsPrincipal));
                OnPropertyChanged(nameof(UserName));
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
