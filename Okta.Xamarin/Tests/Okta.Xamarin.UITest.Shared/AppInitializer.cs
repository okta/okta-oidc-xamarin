using System;
using System.IO;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Okta.Xamarin.UITest
{
    public partial class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            string projectHome = Environment.GetEnvironmentVariable("OKTA_XAMARIN_HOME");
            projectHome = string.IsNullOrEmpty(projectHome) ? Environment.CurrentDirectory : projectHome;
            if (platform == Platform.Android)
            {
                string apkPath = Path.Combine(projectHome, "apparchives", "Android", "apk", "com.okta.xamarin.android-Signed.apk");
                return ConfigureApp.Android.ApkFile(apkPath).StartApp();
            }

            return ConfigureApp.iOS.InstalledApp("com.okta.xamarin.ios").StartApp();
        }
    }
}
