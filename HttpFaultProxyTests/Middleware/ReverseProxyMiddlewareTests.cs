using HttpFaultProxy;
using System;
using System.Net.Http;
using Xunit;
using MockGen;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace HttpFaultProxyTests
{
    public class ReverseProxyMiddlewareTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        private HttpClient CreateClient(HttpMessageHandler fakeHandlerToInject)
        {
            return factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                        services.AddSingleton(new HttpClient(fakeHandlerToInject))))
                .CreateClient();
        }

        public ReverseProxyMiddlewareTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Should_forward_route_information_to_http_client()
        {
            // Given
            HttpRequestMessage proxiedRequest = null;
            var fakeHandler = MockG.Generate<FakeHttpMessageHandler>().New();
            fakeHandler
                .SendAsync(Arg<HttpRequestMessage>.Any, Arg<CancellationToken>.Any)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK))
                .AndExecute((request, cancellationToken) => proxiedRequest = request);

            var client = CreateClient(fakeHandler.Build());
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://someserver:123/api/path?query=foo"));

            // When
            var response = await client.SendAsync(request);
            
            // Then
            Assert.Equal("http", proxiedRequest.RequestUri.Scheme);
            Assert.Equal("someserver", proxiedRequest.RequestUri.Host);
            Assert.Equal(123, proxiedRequest.RequestUri.Port);
            Assert.Equal("/api/path?query=foo", proxiedRequest.RequestUri.PathAndQuery);
        }

        [Fact]
        public async Task Should_forward_request_headers_to_http_client()
        {
            // Given
            HttpRequestMessage proxiedRequest = null;
            var fakeHandler = MockG.Generate<FakeHttpMessageHandler>().New();
            fakeHandler
                .SendAsync(Arg<HttpRequestMessage>.Any, Arg<CancellationToken>.Any)
                .Returns(new HttpResponseMessage(HttpStatusCode.OK))
                .AndExecute((request, cancellationToken) => proxiedRequest = request);

            var client = CreateClient(fakeHandler.Build());
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://someserver"));
            var headerValues = new string[] { "value1", "value2" };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "some-token");
            request.Headers.TryAddWithoutValidation("Some-Header", headerValues);

            // When
            var response = await client.SendAsync(request);

            // Then
            Assert.Equal(headerValues, proxiedRequest.Headers.GetValues("Some-Header"));
            Assert.Equal(new string[] { "Bearer some-token" }, proxiedRequest.Headers.GetValues("Authorization"));
        }

        [Fact]
        public async Task Should_forward_response_from_server_to_the_client()
        {
            // Given
            var fakeHandler = MockG.Generate<FakeHttpMessageHandler>().New();
            var serverResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var serverResponsePayload = new byte[] { 1, 2, 3, 1, 4 };
            serverResponse.Content = new ByteArrayContent(serverResponsePayload);
            fakeHandler
                .SendAsync(Arg<HttpRequestMessage>.Any, Arg<CancellationToken>.Any)
                .Returns(serverResponse);

            var client = CreateClient(fakeHandler.Build());
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://someserver"));

            // When
            var response = await client.SendAsync(request);

            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualRespone = await response.Content.ReadAsByteArrayAsync();
            Assert.Equal(serverResponsePayload, actualRespone);
        }
    }
}
