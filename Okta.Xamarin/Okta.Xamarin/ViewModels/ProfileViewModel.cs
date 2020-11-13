using Okta.Xamarin.Models;
using Okta.Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
	public partial class ProfileViewModel : BaseViewModel
    {
		public ProfileViewModel(ProfilePage profilePage)
		{
			this.Page = profilePage;
			if(Instance == null)
			{
				Instance = this;
			};
			this.StateManager = OktaContext.Current.StateManager;
			OktaContext.AddSignInCompletedListener(OnSignInCompleted);
			OktaContext.AddSignOutCompletedListener(OnSignOutCompleted);
        }

		protected ProfilePage Page { get; }

		public static ProfileViewModel Instance { get; set; }

		public SignInCommand SignInCommand => new SignInCommand();
		public SignOutCommand SignOutCommand => new SignOutCommand();

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

		OktaState stateManager;
		public OktaState StateManager 
		{
			get { return stateManager; }
			set
			{
				stateManager = value;
				AccessToken = stateManager.AccessToken;
				BearerToken = new BearerToken(AccessToken);
			}
		}

		protected void OnSignInCompleted(object sender, EventArgs e)
		{
			this.StateManager = ((SignInEventArgs)e).StateManager;
		}

		protected void OnSignOutCompleted(object sender, EventArgs e)
		{
			this.StateManager = ((SignOutEventArgs)e).StateManager;
		}

		private void SetClaims()
		{
			Page?.SetClaims(this.BearerToken?.GetClaims());
		}
    }
}
