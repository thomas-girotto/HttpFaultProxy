using HttpFaultProxy.Model.Frequencies;
using Xunit;

namespace HttpFaultProxyTests.Model.Frequencies
{
    public class PredictivePerCallFrequencyTests
    {
        [Fact]
        public void Should_trigger_once_every_three_calls()
        {
            var frequency = new PredictivePerCallFrequency(1, 3);

            Assert.True(frequency.ShouldTrigger());
            Assert.False(frequency.ShouldTrigger());
            Assert.False(frequency.ShouldTrigger());
            // Another run just to check that we reinitialized correctly the internal counters
            Assert.True(frequency.ShouldTrigger());
            Assert.False(frequency.ShouldTrigger());
            Assert.False(frequency.ShouldTrigger());
        }
    }
}
