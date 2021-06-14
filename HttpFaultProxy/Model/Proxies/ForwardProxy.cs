using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpFaultProxy.Model.Proxies
{
    /// <summary>
    /// Just forward the request without any policy
    /// </summary>
    public class ForwardProxy : IProxy
    {
        private readonly HttpClient httpClient;

        public ForwardProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken requestAborted)
        {
            return await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, requestAborted);
        }
    }
}
