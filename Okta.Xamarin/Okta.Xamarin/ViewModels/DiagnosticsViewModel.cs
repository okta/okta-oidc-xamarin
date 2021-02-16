// <copyright file="DiagnosticsViewModel.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Views;

namespace Okta.Xamarin.ViewModels
{
    public class DiagnosticsViewModel : BaseViewModel
    {
        public DiagnosticsViewModel(DiagnosticsPage diagnosticsPage)
        {
            this.Page = diagnosticsPage;
            this.StateManager = OktaContext.Current.StateManager;
        }

        protected DiagnosticsPage Page { get; }

        public RevokeAccessTokenCommand RevokeAccessTokenCommand => new RevokeAccessTokenCommand();

        OktaStateManager stateManager;

        public OktaStateManager StateManager
        {
            get { return stateManager; }
            
            set
            {
                stateManager = value;
                OnPropertyChanged(nameof(StateManager));
            }
        }
    }
}
