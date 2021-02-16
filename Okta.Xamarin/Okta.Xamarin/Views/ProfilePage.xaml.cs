// <copyright file="ProfilePage.xaml.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Okta.Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Okta.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel(this);
        }

        public void SetClaims(Dictionary<string, object> claims)
        {
            StackLayout claimsLayout = (StackLayout)this.FindByName("Claims");
            claimsLayout.Children.Clear();
            foreach (string key in claims.Keys)
            {
                Label label = new Label { Text = key };
                label.FontSize = Device.GetNamedSize(NamedSize.Medium, label);
                Label value = new Label { Text = claims[key]?.ToString() };
                value.FontSize = Device.GetNamedSize(NamedSize.Small, value);

                claimsLayout.Children.Add(label);
                claimsLayout.Children.Add(value);
            }
        }
    }
}
