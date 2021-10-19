using Newtonsoft.Json;
using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Okta.Net.Configuration
{
	public class ProfileIdentityClientConfiguration : IdentityClientConfiguration
	{
		public ProfileIdentityClientConfiguration()
		{
			File = new FileInfo(Profile.PathFor($"~/.okta/{Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location)}/identityclient.yaml"));
		}

		[JsonIgnore]
		[YamlIgnore]
		public FileInfo File { get; set; }

		public ProfileIdentityClientConfiguration Load()
		{
			if (File.Exists)
			{
				IdentityClientConfiguration existing = System.IO.File.ReadAllText(File.FullName).FromYaml<IdentityClientConfiguration>();

				this.CopyProperties(existing);
			}
			else
			{
				this.CopyProperties(IdentityClientConfiguration.Default);
			}

			return this;
		}

		public void Save()
		{
			if (!File.Directory.Exists)
			{
				File.Directory.Create();
			}

			System.IO.File.WriteAllText(File.FullName, this.ToYaml());
			File.Refresh();
		}
	}
}
