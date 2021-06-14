using HttpFaultProxy.Model.Frequencies;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpFaultProxy.Model.Proxies
{
    public class ProxyWithDelay : IProxy
    {
        private readonly IProxy decorated;
        private readonly IScheduler scheduler;
        private readonly TimeSpan delay;
        private readonly Frequency frequency;

        public ProxyWithDelay(IProxy decorated, IScheduler wait, TimeSpan delay, Frequency frequency)
        {
            this.decorated = decorated;
            this.scheduler = wait;
            this.delay = delay;
            this.frequency = frequency;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken requestAborted)
        {
            if (frequency.ShouldTrigger())
            {
                await scheduler.DelayAsync(delay, requestAborted);
            }
            return await decorated.SendAsync(request, requestAborted);
        }
    }
}
