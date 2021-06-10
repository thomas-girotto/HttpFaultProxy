using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpFaultProxyTests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
