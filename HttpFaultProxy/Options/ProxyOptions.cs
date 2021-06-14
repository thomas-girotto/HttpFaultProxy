using System.Collections.Generic;

namespace HttpFaultProxy.Options
{
    public class ProxyOptions
    {
        public const string SectionName = "Routes";
        public List<RouteOptions> Routes { get; set; } = new List<RouteOptions>();
    }
}
