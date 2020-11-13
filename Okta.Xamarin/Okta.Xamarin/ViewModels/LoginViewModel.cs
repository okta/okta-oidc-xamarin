using Okta.Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
	public class LoginViewModel : BaseViewModel
    {
        public Command SignInCommand { get; }

        public LoginViewModel()
        {
			SignInCommand = new SignInCommand();
        }
    }
}
