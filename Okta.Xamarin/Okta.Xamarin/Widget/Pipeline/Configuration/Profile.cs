// <copyright file="Profile.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Okta.Xamarin.Widget.Pipeline.Configuration
{
    public class Profile
    {
        public static string PathFor(string tildePrefixedProfilePath)
        {
            return PathFor(tildePrefixedProfilePath.Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string PathFor(params string[] pathSegments)
        {
            if (pathSegments.Length == 0)
            {
                return null;
            }

            if (!pathSegments[0].StartsWith("~"))
            {
                return Path.Combine(pathSegments);
            }

            string homePath = GetPath();
            return Path.Combine(new string[1]
            {
                pathSegments[0].Replace("~", homePath),
            }.Concat(pathSegments.Skip(1)).ToArray());
        }

        public static string GetPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.GetEnvironmentVariable("USERPROFILE") ?? Path.Combine(Environment.GetEnvironmentVariable("HOMEDRIVE"), Environment.GetEnvironmentVariable("HOMEPATH"));
            }

            string environmentVariable = Environment.GetEnvironmentVariable("HOME");
            if (string.IsNullOrEmpty(environmentVariable))
            {
                throw new Exception("Home directory not found. The HOME environment variable is not set.");
            }

            return environmentVariable;
        }
    }
}
