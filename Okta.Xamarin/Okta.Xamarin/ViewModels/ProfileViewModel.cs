using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin.ViewModels
{
	public partial class ProfileViewModel : BaseViewModel
	{
		public string AccessToken
		{
			get => OktaContext.Current.StateManager?.AccessToken;
		}
	}
}
