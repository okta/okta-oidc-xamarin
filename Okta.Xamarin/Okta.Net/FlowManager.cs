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
	public class FlowManager : IFlowManager
	{
		public FlowManager() : this(Net.ServiceProvider.Default)
		{ }

		public FlowManager(IServiceProvider serviceProvider) : 
			this(serviceProvider, serviceProvider.GetService<IIdentityClient>(), serviceProvider.GetService<IIdentityDataProvider>(), serviceProvider.GetService<IPolicyProvider>(), serviceProvider.GetService<ISessionProvider>(), serviceProvider.GetService<IStorageProvider>(), serviceProvider.GetService<ILoggingProvider>(), serviceProvider.GetService<IViewProvider>())
		{
		}

		public FlowManager(IServiceProvider serviceProvider, IIdentityClient idxClient, IIdentityDataProvider dataProvider, IPolicyProvider policyProvider, ISessionProvider sessionProvider, IStorageProvider storageProvider, ILoggingProvider loggingProvider, IViewProvider viewProvider)
		{
			this.ServiceProvider = serviceProvider;
			this.IdentityClient = idxClient;
			this.DataProvider = dataProvider;
			this.PolicyProvider = policyProvider;
			this.SessionProvider = sessionProvider;
			this.StorageProvider = storageProvider;
			this.LoggingProvider = loggingProvider;
			this.ViewProvider = viewProvider;
		}

		public event EventHandler<FlowManagerEventArgs> FlowStarting;
		public event EventHandler<FlowManagerEventArgs> FlowStartCompleted;
		public event EventHandler<FlowManagerEventArgs> FlowStartExceptionThrown;
		public event EventHandler<FlowManagerEventArgs> FlowContinuing;
		public event EventHandler<FlowManagerEventArgs> FlowContinueCompleted;
		public event EventHandler<FlowManagerEventArgs> FlowContinueExceptionThrown;
		public event EventHandler<FlowManagerEventArgs> Validating;
		public event EventHandler<FlowManagerEventArgs> ValidateCompleted;
		public event EventHandler<FlowManagerEventArgs> ValidateExceptionThrown;

		public IServiceProvider ServiceProvider { get; }

		public IIdentityClient IdentityClient { get; }

		public IIdentityDataProvider DataProvider { get; }

		public IPolicyProvider PolicyProvider { get; }

		public ISessionProvider SessionProvider { get; }

		public IStorageProvider StorageProvider { get; }

		public ILoggingProvider LoggingProvider { get; }

		public IViewProvider ViewProvider { get; }

		public Task<IIdentityIntrospection> Continue(IIdentityInteraction identitySession)
		{
			throw new NotImplementedException();
		}

		public Task<IIdentityIntrospection> ProceedAsync(IdentityRequest identityRequest)
		{
			throw new NotImplementedException();
		}

		public async Task<IIdentityIntrospection> StartAsync()
		{
			try
			{
				FlowStarting?.Invoke(this, new FlowManagerEventArgs
				{
					FlowManager = this
				});

				IIdentityInteraction interaction = await DataProvider.StartSessionAsync();
				_ = Task.Run(() => SessionProvider.Set(interaction.State, interaction.ToJson()));

				IIdentityIntrospection form = await DataProvider.GetFormDataAsync(interaction.InteractionHandle);

				FlowStartCompleted?.Invoke(this, new FlowManagerEventArgs
				{
					FlowManager = this,
					Session = interaction,
					Form = form
				});

				return form;
			}
			catch (Exception ex)
			{
				FlowStartExceptionThrown?.Invoke(this, new FlowManagerEventArgs
				{
					FlowManager = this,
					Exception = ex
				});

				return new IdentityIntrospection { Exception = ex };
			}
		}

		public async Task<IPolicyValidationResult> ValidatePolicyConformanceAsync(IIdentityIntrospection identitySession)
		{
			return PolicyProvider.ValidatePolicy(new PolicyValidationOptions { IdentityForm = identitySession });
		}
	}
}
