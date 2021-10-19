using Okta.Net.Data;
using Okta.Net.Identity;
using Okta.Net.Identity.Data;
using Okta.Net.Policy;
using Okta.Net.Session;
using Okta.Net.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net
{
    public interface IFlowManager : IHasServiceProvider
    {
		event EventHandler<FlowManagerEventArgs> FlowStarting;
		event EventHandler<FlowManagerEventArgs> FlowStartCompleted;
		event EventHandler<FlowManagerEventArgs> FlowStartExceptionThrown;

		event EventHandler<FlowManagerEventArgs> FlowContinuing;
		event EventHandler<FlowManagerEventArgs> FlowContinueCompleted;
		event EventHandler<FlowManagerEventArgs> FlowContinueExceptionThrown;

		event EventHandler<FlowManagerEventArgs> Validating;
		event EventHandler<FlowManagerEventArgs> ValidateCompleted;
		event EventHandler<FlowManagerEventArgs> ValidateExceptionThrown;

		IIdentityClient IdentityClient { get; }
		IIdentityDataProvider DataProvider { get; }
		IPolicyProvider PolicyProvider { get; }
		ISessionProvider SessionProvider { get; }
		IStorageProvider StorageProvider { get; }
		ILoggingProvider LoggingProvider  { get; }
		IViewProvider ViewProvider { get; }

		Task<IIdentityIntrospection> StartAsync();

		Task<IIdentityIntrospection> Continue(IIdentityInteraction identitySession);

		Task<IPolicyValidationResult> ValidatePolicyConformanceAsync(IIdentityIntrospection idxState);

		Task<IIdentityIntrospection> ProceedAsync(IdentityRequest idxRequest);
    }
}
