using HttpFaultProxy.Model.Frequencies;
using HttpFaultProxy.Options;

namespace HttpFaultProxy.Model.Proxies
{
    public class ProxyFactory
    {
        private readonly IProxy defaultProxy;
        private readonly IScheduler scheduler;

        public ProxyFactory(IProxy defaultProxy, IScheduler scheduler)
        {
            this.defaultProxy = defaultProxy;
            this.scheduler = scheduler;
        }

        public IProxy Default => defaultProxy;

        public IProxy Create(RouteOptions options)
        {
            if (options.Delay.HasValue)
            {
                return new ProxyWithDelay(defaultProxy, scheduler, options.Delay.Value, FrequencyFactory.Create(options.Frequency));
            }

            return defaultProxy;
        }
    }
}
