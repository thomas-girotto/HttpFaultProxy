using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpFaultProxy
{
    public class Scheduler : IScheduler
    {
        public async Task DelayAsync(TimeSpan delay, CancellationToken requestAborted)
        {
            await Task.Delay(delay, requestAborted);
        }
    }
}
