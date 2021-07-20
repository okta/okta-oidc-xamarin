using Okta.Xamarin.Demo.Views;
using Okta.Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
	public class RevokeViewModel : BaseViewModel
	{
		public RevokeViewModel(RevokePage revokePage)
		{
			this.Page = revokePage;
			OktaContext.AddRevokeStartedListener(OnRevokeStarted);
			OktaContext.AddRevokeCompletedListener(OnRevokeCompleted);
		}

		protected new RevokePage Page { get; }

		public Command RevokeAccessTokenCommand => new Command(async () =>
		{
			if (!string.IsNullOrEmpty(OktaContext.AccessToken))
			{
				await OktaContext.Current.RevokeAsync();
			}
		});

		public Command GoToIntrospectPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(IntrospectPage)));

		public Command GoToRenewPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(RenewPage)));

		protected void OnRevokeStarted(object sender, RevokeEventArgs revokeEventArgs)
		{
			Page?.DisplayMessage("Revoking access token...");
			ShowWorkingImage();
		}

		protected void OnRevokeCompleted(object sender, RevokeEventArgs revokeEventArgs)
		{
			HttpResponseMessage responseMessage = revokeEventArgs.Response as HttpResponseMessage;
			Page?.DisplayMessage($"Response status code: {responseMessage?.StatusCode}");
			IntrospectViewModel.IntrospectResponse = null;
			HideWorkingImage();
		}
	}
}
