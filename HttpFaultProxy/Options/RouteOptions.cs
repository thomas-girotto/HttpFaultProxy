using System;

namespace HttpFaultProxy.Options
{
    public class RouteOptions
    {
        public string Match { get; set; }
        public FrequencyOptions Frequency { get; set; }
        public TimeSpan? Delay { get; set; }
    }
}
