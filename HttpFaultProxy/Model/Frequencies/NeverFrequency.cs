namespace HttpFaultProxy.Model.Frequencies
{
    public class NeverFrequency : Frequency
    {
        public override bool ShouldTrigger() => false;
    }
}
