namespace HttpFaultProxy.Model.Frequencies
{
    public class AlwaysFrequency : Frequency
    {
        public override bool ShouldTrigger() => true;
    }
}
