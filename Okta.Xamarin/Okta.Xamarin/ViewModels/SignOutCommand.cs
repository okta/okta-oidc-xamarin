using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
	public class SignOutCommand : Command
	{
		public SignOutCommand()
			: base(async () => await OktaContext.Current.SignOutAsync())
		{
		}
	}
}
