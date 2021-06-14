using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpFaultProxy.Model.Proxies
{
    public interface IProxy
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken requestAborted);
    }
}
