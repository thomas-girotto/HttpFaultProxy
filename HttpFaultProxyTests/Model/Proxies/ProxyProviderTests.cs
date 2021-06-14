using HttpFaultProxy.Model.Frequencies;
using HttpFaultProxy.Model.Proxies;
using HttpFaultProxy.Options;
using Microsoft.Extensions.Options;
using MockGen;
using System;
using System.Collections.Generic;
using Xunit;

namespace HttpFaultProxyTests.Model.Proxies
{
    public class ProxyProviderTests
    {
        private ProxyOptions BuildOptions(string matchRoute, bool withDelay)
        {
            return new ProxyOptions
            {
                Routes = new List<RouteOptions>
                {
                    new RouteOptions
                    {
                        Match = matchRoute,
                        Delay = withDelay ? TimeSpan.FromSeconds(1) : null,
                        Frequency = new FrequencyOptions
                        {
                            Type = FrequencyType.Always,
                            NbOfTriggers = 1,
                            OutOfNbOfCalls = 2
                        }
                    }
                }
            };
        }

        [Fact]
        public void Should_get_a_delay_proxy_When_route_match_and_delay_is_configured()
        {
            // Given
            var mockOptions = MockG.Generate<IOptionsMonitor<ProxyOptions>>().New();
            mockOptions.CurrentValue.Get.Returns(BuildOptions("api/foo", true));
            var proxyProvider = new ProxyProvider(new ProxyFactory(null, null), mockOptions.Build());

            // When
            var proxy = proxyProvider.Get("api/foo");

            // Then
            Assert.IsType<ProxyWithDelay>(proxy);
        }

        [Fact]
        public void Should_get_the_default_proxy_When_route_does_not_match_any_config()
        {
            // Given
            var mockOptions = MockG.Generate<IOptionsMonitor<ProxyOptions>>().New();
            mockOptions.CurrentValue.Get.Returns(BuildOptions("api/foo", true));
            var defaultProxy = MockG.Generate<IProxy>().New();
            var proxyFactory = new ProxyFactory(defaultProxy.Build(), null);
            var proxyProvider = new ProxyProvider(proxyFactory, mockOptions.Build());

            // When
            var proxy = proxyProvider.Get("api/bar");

            // Then
            Assert.Equal(proxyFactory.Default, proxy);
        }

        [Theory]
        [InlineData("http://localhost:123/api/foo")]
        [InlineData("http://localhost:123/api/bar")]
        public void Should_match_route_with_regex(string route)
        {
            // Given
            var regex = "http://localhost:123/*";
            var mockOptions = MockG.Generate<IOptionsMonitor<ProxyOptions>>().New();
            mockOptions.CurrentValue.Get.Returns(BuildOptions(regex, true));
            var proxyProvider = new ProxyProvider(new ProxyFactory(null, null), mockOptions.Build());

            // When
            var proxy = proxyProvider.Get(route);

            // Then
            Assert.IsType<ProxyWithDelay>(proxy);
        }
    }
}
