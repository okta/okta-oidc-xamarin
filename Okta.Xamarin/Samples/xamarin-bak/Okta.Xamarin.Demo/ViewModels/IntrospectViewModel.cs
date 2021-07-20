using Okta.Xamarin.Demo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
	public class IntrospectViewModel : BaseViewModel
	{
		public IntrospectViewModel(IntrospectPage demoPage)
		{
			Page = demoPage;
			OktaContext.AddIntrospectStartedListener(OnIntrospectStarted);
			OktaContext.AddIntrospectCompletedListener(OnIntrospectCompleted);
			if (IntrospectResponse != null)
			{
				SetIntrospectResponseDisplay();
			}
			else
			{
				Page?.ClearChildren("ResponseDisplay");
			}
		}

		protected internal static Dictionary<string, object> IntrospectResponse { get; set; }

		public Command IntrospectAccessTokenCommand => new Command(async () =>
		{
			if (!string.IsNullOrEmpty(OktaContext.AccessToken))
			{
				await OktaContext.Current.IntrospectAsync(TokenKind.AccessToken);
			}
		});
		
		public Command GoToRenewPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(RenewPage)));

		public Command GoToRevokePageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(RevokePage)));

		protected void OnIntrospectStarted(object sender, IntrospectEventArgs introspectEventArgs)
		{
			Page?.ClearChildren("ResponseDisplay");
			ShowWorkingImage();			
			Thread.Sleep(300);
		}

		protected void OnIntrospectCompleted(object sender, IntrospectEventArgs introspectEventArgs)
		{
			IntrospectResponse = introspectEventArgs.Response as Dictionary<string, object>;
			SetIntrospectResponseDisplay();
			HideWorkingImage();
		}

		private void SetIntrospectResponseDisplay()
		{
			Page?.SetChildData("ResponseDisplay", IntrospectResponse);
		}
	}
}
