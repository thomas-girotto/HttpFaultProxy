using System;
using System.Threading.Tasks;

namespace HttpFaultProxy
{
    public interface IScheduler
    {
        Task DelayAsync(TimeSpan delay, System.Threading.CancellationToken requestAborted);
    }
}
