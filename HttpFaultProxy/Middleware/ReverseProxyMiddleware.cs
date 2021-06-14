using HttpFaultProxy.Model.Proxies;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpFaultProxy.Middleware
{
    public class ReverseProxyMiddleware
    {
        private readonly IProxyProvider proxyProvider;

        public ReverseProxyMiddleware(RequestDelegate _, IProxyProvider proxyProvider)
        {
            this.proxyProvider = proxyProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var targetRequestMessage = CreateTargetMessage(context);
            var proxy = proxyProvider.Get(targetRequestMessage.RequestUri!.ToString());
            using var responseMessage = await proxy.SendAsync(targetRequestMessage, context.RequestAborted);
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            CopyFromTargetResponseHeaders(context, responseMessage);
            await responseMessage.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
            
            return;
        }

        private HttpRequestMessage CreateTargetMessage(HttpContext context)
        {
            var requestMessage = new HttpRequestMessage();
            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);
            var uri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            requestMessage.RequestUri = new Uri(uri);
            requestMessage.Headers.Host = context.Request.Host.ToString();
            requestMessage.Method = new HttpMethod(context.Request.Method);

            return requestMessage;
        }

        private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod) &&
              !HttpMethods.IsHead(requestMethod) &&
              !HttpMethods.IsDelete(requestMethod) &&
              !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in context.Request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }
    }
}