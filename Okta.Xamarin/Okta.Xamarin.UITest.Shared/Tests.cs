using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Okta.Xamarin.UITest
{
    public partial class Tests
    {
        IApp app;
        Platform platform;
        List<FileInfo> screenshots;
        public Tests(Platform platform)
        {
            this.platform = platform;
            this.screenshots = new List<FileInfo>();
        }

		// TODO: move tests to samples-xamarin repository
        //[SetUp] 
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        //[Test]
        public void OpeningScreenHasSigninButton()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("AboutPageButtonSignIn"));
            screenshots.Add(app.Screenshot("Opening Screen"));

            Assert.IsTrue(results.Any());
        }
    }
}
