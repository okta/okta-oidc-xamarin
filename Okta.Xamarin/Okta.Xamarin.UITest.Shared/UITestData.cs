using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Okta.Xamarin;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace Okta.Xamarin.UITest
{
    public class UITestData
    {
        public static UITestData Load(string filePath = "~/.okta/UITestData.json")
        {
            string uiTestDataFilePath = ResolveHomePath(filePath);
            if (!File.Exists(uiTestDataFilePath))
            {
                throw new ArgumentException($"Specified test data file was not found: ({uiTestDataFilePath})");
            }
            return JsonConvert.DeserializeObject<UITestData>(File.ReadAllText(uiTestDataFilePath));
        }

        private static readonly Lazy<UITestData> lazyUiTestData = new Lazy<UITestData>(() => Load());
        private static UITestData current;

        public static UITestData Current
        {
            get
            {
                if(current == null)
                {
                    current = lazyUiTestData.Value;
                }
                return current;
            }

            set => current = value;
        }

        /// <summary>
        /// The user name used to test sign in.
        /// </summary>
        public string TestUserName { get; set; }

        /// <summary>
        /// The password used to test sign in.
        /// </summary>
        public string TestPassword { get; set; }

        /// <summary>
        /// Ensures that TestUserName and TestPassword properties are set.  Throws ArgumentException
        /// if required values are missing.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrEmpty(TestUserName))
            {
                throw new ArgumentException("TestUserName not set");
            }
            if (string.IsNullOrEmpty(TestPassword))
            {
                throw new ArgumentException("TestPassword not set");
            }
        }

        private static string ResolveHomePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            if (path.StartsWith("~"))
            {
                string homePath = GetHomePath();
                string trimmed = path.Substring(1).Trim();
                if (trimmed.StartsWith("/"))
                {
                    trimmed = trimmed.Substring(1);
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Path.Combine(homePath, trimmed);
                }
                return Path.Combine(homePath, trimmed);
            }
            return path;
        }

        private static string GetHomePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
            return Environment.GetEnvironmentVariable("HOME");
        }
    }
}
