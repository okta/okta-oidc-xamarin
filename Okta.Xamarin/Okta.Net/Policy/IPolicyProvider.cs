using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Policy
{
	public interface IPolicyProvider
	{
		PolicyValidationResult ValidatePolicy(PolicyValidationOptions policyValidationOptions);


	}
}
