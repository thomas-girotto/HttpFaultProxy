namespace HttpFaultProxy.Model.Frequencies
{
    public enum FrequencyType
    {
        Unknown = 0,
        PerCall,
        PerCallDeterministic,
        Always,
        Never
    }
}
