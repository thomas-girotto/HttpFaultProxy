using HttpFaultProxy.Options;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HttpFaultProxy.Model.Proxies
{
    public class ProxyProvider : IProxyProvider
    {
        private readonly ProxyFactory proxyFactory;
        private List<(string routeMatch, IProxy proxy)> proxiesPerRoute = new List<(string, IProxy)>();
        
        public ProxyProvider(ProxyFactory proxyFactory, IOptionsMonitor<ProxyOptions> options)
        {
            BuildProxyInstances(proxyFactory, options.CurrentValue);
            options.OnChange(newOptions => BuildProxyInstances(proxyFactory, newOptions));
            this.proxyFactory = proxyFactory;
        }

        public IProxy Get(string uri)
        {
            foreach (var proxyPerRoute in proxiesPerRoute)
            {
                if (new Regex(proxyPerRoute.routeMatch).IsMatch(uri))
                {
                    return proxyPerRoute.proxy;
                }
            }

            return proxyFactory.Default;
        }

        private void BuildProxyInstances(ProxyFactory proxyFactory, ProxyOptions options)
        {
            foreach (var route in options.Routes)
            {
                proxiesPerRoute.Add((route.Match, proxyFactory.Create(route)));
            }
        }
    }
}
