using HttpFaultProxy.Model.Frequencies;

namespace HttpFaultProxy.Options
{
    public class FrequencyOptions
    {
        public FrequencyType Type { get; set; }
        public int NbOfTriggers { get; set; }
        public int OutOfNbOfCalls { get; set; }
    }
}
