using Okta.Xamarin.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
			this.Title = "About";
			this.OpenOktaApiReferenceCommand = new Command(async () => await Browser.OpenAsync("https://developer.okta.com/docs/reference/"));
			this.SignInCommand = new SignInCommand();
			this.SignOutCommand = new SignOutCommand();
		}

		/// <summary>
		/// Gets the command that opens Okta Api reference.
		/// </summary>
		public ICommand OpenOktaApiReferenceCommand { get; }

		/// <summary>
		/// Gets or sets the command used to sign in.
		/// </summary>
		public SignInCommand SignInCommand { get; set; }

		/// <summary>
		/// Gets or sets the command used to sign out.
		/// </summary>
		public SignOutCommand SignOutCommand { get; set; }
	}
}