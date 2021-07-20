using Okta.Xamarin.Demo.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
	public class DemoPageViewModel : BaseViewModel
	{
		public DemoPageViewModel(DemoPage demoPage)
		{
			this.Page = demoPage;
		}

		public Command GoToIntrospectPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(IntrospectPage)));

		public Command GoToRenewPageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(RenewPage)));

		public Command GoToRevokePageCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(RevokePage)));
	}
}
