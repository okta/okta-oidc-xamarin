// <copyright file="DiagnosticsPage.xaml.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Okta.Xamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DiagnosticsPage : ContentPage
	{
		public DiagnosticsPage()
		{
			InitializeComponent();
			BindingContext = new DiagnosticsViewModel(this);
			OktaContext.AddTokenRevokedListener((sender, args) => 
			{
				RevokeTokenEventArgs revokeTokenEventArgs = (RevokeTokenEventArgs)args;
				OktaStateManager = revokeTokenEventArgs.StateManager;
				SetMessage($"Token revoked: {OktaStateManager.LastApiResponse?.StatusCode}");
			});
		}

		public OktaStateManager OktaStateManager
		{
			get;
			set;
		}

		public void SetMessage(string text)
		{
			Label messageLabel = (Label)this.FindByName("Message");
			messageLabel.Text = text;
		}
	}
}
