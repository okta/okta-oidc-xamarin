using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin
{
    public static class ContentPageExtensions
    {
        public static void DisplayData(this ContentPage contentPage, Dictionary<string, object> data, string stackLayoutElementName)
        {
            StackLayout claimsLayout = (StackLayout)contentPage.FindByName(stackLayoutElementName);
            claimsLayout.Children.Clear();
            foreach (string key in data.Keys)
            {
                Label label = new Label { Text = key };
                label.FontSize = Device.GetNamedSize(NamedSize.Medium, label);
                Label value = new Label { Text = data[key]?.ToString() };
                value.FontSize = Device.GetNamedSize(NamedSize.Small, value);

                claimsLayout.Children.Add(label);
                claimsLayout.Children.Add(value);
            }
        }
    }
}
