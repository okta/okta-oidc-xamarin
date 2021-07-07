using Okta.Xamarin.Demo.Views;
using Okta.Xamarin.Models;
using Okta.Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
	public partial class ProfileViewModel : BaseViewModel
	{
		public ProfileViewModel(ProfilePage profilePage)
		{
			this.Page = profilePage;
			this.OktaStateManager = OktaContext.Current.StateManager;
			OktaContext.AddSignInCompletedListener(OnSignInCompleted);
			OktaContext.AddSignOutCompletedListener(OnSignOutCompleted);
		}

		protected ProfilePage Page { get; }

		public Command GoToDemoPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(DemoPage)));
		public Command SignInCommand => new SignInCommand();
		public Command SignOutCommand => new SignOutCommand();

		string accessToken = string.Empty;

		public string AccessToken
		{
			get { return accessToken; }
			set { SetProperty(ref accessToken, value); }
		}

		string header = string.Empty;

		public string Header
		{
			get { return header; }
			set { SetProperty(ref header, value); }
		}

		string payload = string.Empty;

		public string Payload
		{
			get { return payload; }
			set { SetProperty(ref payload, value); }
		}

		string signature = string.Empty;

		public string Signature
		{
			get { return signature; }
			set { SetProperty(ref signature, value); }
		}

		BearerToken bearerToken;

		public BearerToken BearerToken
		{
			get { return bearerToken; }

			set
			{
				SetProperty(ref bearerToken, value);
				Header = bearerToken?.Header;
				Payload = bearerToken?.Payload;
				Signature = bearerToken?.Signature;
				SetClaims();
			}
		}

		IOktaStateManager stateManager;

		public override IOktaStateManager OktaStateManager
		{
			get { return stateManager; }

			set
			{
				stateManager = value;
				AccessToken = stateManager?.AccessToken;
				BearerToken = new BearerToken(AccessToken);
			}
		}

		private void SetClaims()
		{
			Page?.SetChildData("Claims", this.BearerToken?.GetClaims());
		}
	}
}
