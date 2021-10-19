using Okta.Net.Data;
using Okta.Net.Identity;
using Okta.Net.Identity.Data;
using Okta.Net.Identity.View;
using Okta.Net.IoC;
using Okta.Net.Logging;
using Okta.Net.Policy;
using Okta.Net.Session;
using Okta.Net.View;

namespace Okta.Net
{
	public class ServiceProvider : IServiceProvider
	{
		TinyIoCContainer _container = new TinyIoCContainer();

		public IType GetService<IType>() where IType : class
		{
			return _container.Resolve<IType>();
		}

		public void RegisterService<IType, CType>()
			where IType : class
			where CType : class, IType
		{
			_container.Register<IType, CType>();
		}

		public void RegisterService<IType>(IType implementation) where IType: class
		{
			_container.Register<IType>(implementation);
		}

		static ServiceProvider _defaultServiceProvider;
		static object _defaultLock = new object();
		public static ServiceProvider Default
		{
			get
			{
				if (_defaultServiceProvider == null)
				{
					lock (_defaultLock)
					{
						if (_defaultServiceProvider == null)
						{
							ServiceProvider temp = new ServiceProvider();
							temp.RegisterService<IServiceProvider>(temp);
							temp.RegisterService<IIdentityClient>(new IdentityClient());
							temp.RegisterService<IIdentityViewModelProvider, IdentityViewModelProvider>();
							temp.RegisterService<IIdentityDataProvider, IdentityDataProvider>();
							temp.RegisterService<IPolicyProvider>(new PolicyProvider());
							temp.RegisterService<ISessionProvider>(new SecureSessionProvider());
							temp.RegisterService<IStorageProvider>(new FileStorageProvider());
							temp.RegisterService<ILoggingProvider>(new LoggingProvider());
							temp.RegisterService<IViewProvider>(new ViewProvider());
							_defaultServiceProvider = temp;
						}
					}
				}
				return _defaultServiceProvider;
			}
		}

	}
}
