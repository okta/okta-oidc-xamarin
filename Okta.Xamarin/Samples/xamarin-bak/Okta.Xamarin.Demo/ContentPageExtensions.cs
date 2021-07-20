using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo
{
	public static class ContentPageExtensions
	{
		public static void DisplayMessage(this ContentPage contentPage, string message, string viewLayoutName = "ResponseDisplay")
		{
			contentPage.ClearChildren(viewLayoutName);
			contentPage.SetChildData(viewLayoutName, new Dictionary<string, object>()
			{
				{ "", message},
			});
			Thread.Sleep(300);
		}

		public static void ClearChildren(this ContentPage contentPage, string viewLayoutName)
		{
			Layout<View> viewLayout = (Layout<View>)contentPage.FindByName(viewLayoutName);
			viewLayout.Children.Clear();
		}

		public static void SetChildData(this ContentPage contentPage, string viewLayoutName, Dictionary<string, object> data)
		{
			Layout<View> viewLayout = (Layout<View>)contentPage.FindByName(viewLayoutName);
			SetChildData(viewLayout, data);
		}

		public static void SetChildData(this Layout<View> layout, Dictionary<string, object> data)
		{		
			layout.Children.Clear();
			foreach (string key in data.Keys)
			{
				Label label = new Label { Text = key };
				label.FontSize = Device.GetNamedSize(NamedSize.Medium, label);
				Label value = new Label { Text = data[key]?.ToString() };
				value.FontSize = Device.GetNamedSize(NamedSize.Small, value);

				layout.Children.Add(label);
				layout.Children.Add(value);
			}
		}
	}
}
