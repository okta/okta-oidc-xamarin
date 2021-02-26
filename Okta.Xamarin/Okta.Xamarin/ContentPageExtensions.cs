// <copyright file="ContentPageExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Xamarin.Forms;

namespace Okta.Xamarin
{
    /// <summary>
    /// Extension metods for ContentPage.
    /// </summary>
    public static class ContentPageExtensions
    {
        /// <summary>
        /// Adds the specified data to the stack layout with the specified name.
        /// </summary>
        /// <param name="contentPage">The content page</param>
        /// <param name="data">The data</param>
        /// <param name="stackLayoutElementName">The name of the stack layout.</param>
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
