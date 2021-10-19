using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Configuration
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
				pathSegments[0].Replace("~", homePath)
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
