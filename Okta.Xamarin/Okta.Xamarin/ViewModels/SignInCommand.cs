using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
	public class SignInCommand : Command
	{
		public SignInCommand()
			: base(async () => await OktaContext.Current.SignIn())
		{
		}
	}
}
