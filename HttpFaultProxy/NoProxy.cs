using System;
using System.Net;

namespace HttpFaultProxy
{
    public class NoProxy : IWebProxy
    {
        public ICredentials? Credentials { get; set; }

        public Uri? GetProxy(Uri destination) => null;

        public bool IsBypassed(Uri host) => true;
    }
}
