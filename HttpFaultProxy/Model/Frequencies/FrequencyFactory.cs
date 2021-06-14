using HttpFaultProxy.Options;

namespace HttpFaultProxy.Model.Frequencies
{
    public static class FrequencyFactory
    {
        public static Frequency Create(FrequencyOptions options)
        {
            switch(options.Type)
            {
                case FrequencyType.Always:
                    return new AlwaysFrequency();
                case FrequencyType.PerCall:
                    return new RandomPerCallFrequency(options.NbOfTriggers, options.OutOfNbOfCalls);
                case FrequencyType.PerCallDeterministic:
                    return new PredictivePerCallFrequency(options.NbOfTriggers, options.OutOfNbOfCalls);
                case FrequencyType.Never:
                    return new NeverFrequency();
                default:
                    return new NeverFrequency();
            }
        }
    }
}
