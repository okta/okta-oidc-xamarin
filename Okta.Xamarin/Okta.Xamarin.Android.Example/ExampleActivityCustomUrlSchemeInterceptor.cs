using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Okta.Xamarin.Android.Example
{
	[Activity(Label = "ExampleActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[
	IntentFilter
	(
		actions: new[] { Intent.ActionView },
		Categories = new[]
				{
					Intent.CategoryDefault,
					Intent.CategoryBrowsable
				},
		DataSchemes = new[]
				{
					"com.okta.xamarin.android.exampleapp"
				},
		DataPath = "/callback"
	)
]
	public class ExampleActivityCustomUrlSchemeInterceptor : ActivityCustomUrlSchemeInterceptor
	{

	}
}