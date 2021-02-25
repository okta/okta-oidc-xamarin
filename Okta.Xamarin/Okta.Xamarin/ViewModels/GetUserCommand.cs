using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
	public class GetUserCommand: Command
	{
		public GetUserCommand() : base(async () => await OktaContext.Current.GetUserAsync())
		{ }
	}
}
