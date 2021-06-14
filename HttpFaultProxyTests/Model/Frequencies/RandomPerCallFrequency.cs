using HttpFaultProxy.Model.Frequencies;
using Xunit;

namespace HttpFaultProxyTests.Model.Frequencies
{
    public class RandomPerCallFrequencyTests
    {
        [Theory]
        [InlineData(1, 100)]
        [InlineData(99, 100)]
        [InlineData(1, 2)]
        public void Should_trigger_the_right_amount_of_time(int expectedNbOfTriggers, int outOfNbOfCalls)
        {
            var frequency = new RandomPerCallFrequency(expectedNbOfTriggers, outOfNbOfCalls);
            var actualNbOfTriggers = 0;
            for (int i = 0; i < outOfNbOfCalls; i++)
            {
                if (frequency.ShouldTrigger())
                {
                    actualNbOfTriggers++;
                }
            }
            Assert.Equal(expectedNbOfTriggers, actualNbOfTriggers);
        }
    }
}
