using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Okta.Xamarin.TinyIoC;

namespace Okta.Xamarin.Test
{
    public class OktaPlatformShould
    {
        [Fact]
        public async void ReturnCurrentContext()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            OktaContext context = await OktaPlatformBase.InitAsync(client);

            OktaContext.Current.Should().Be(context);
        }

        [Fact]
        public async void ReturnSpecifiedContext()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            OktaContext testContext = new OktaContext();
            OktaContext returnedContext = await OktaPlatformBase.InitAsync(client, testContext);

            OktaContext.Current.Should().NotBe(returnedContext);
            returnedContext.Should().Be(testContext);
        }

        [Fact]
        public async void SetClient()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            OktaContext context = await OktaPlatformBase.InitAsync(client);

            context.OidcClient.Should().Be(client);
        }

        [Fact]
        public async void SetConfig()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            IOktaConfig config = Substitute.For<IOktaConfig>();
            client.Config = config;
            OktaContext context = await OktaPlatformBase.InitAsync(client);

            context.OktaConfig.Should().Be(config);
        }

        [Fact]
        public async void RegisterServices()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            IOktaConfig config = Substitute.For<IOktaConfig>();
            client.Config = config;
            OktaContext context = await OktaPlatformBase.InitAsync(client);

            TinyIoCContainer container = context.IoCContainer;

            container.Resolve<IOidcClient>().Should().Be(client);
            container.Resolve<IOktaConfig>().Should().Be(config);
            container.Resolve<OktaContext>().Should().Be(context);
        }
    }
}
