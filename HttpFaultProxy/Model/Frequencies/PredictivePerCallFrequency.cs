using System;

namespace HttpFaultProxy.Model.Frequencies
{
    /// <summary>
    /// Triggers systematically first for the given amount of activations, and then do not trigger until the total
    /// number of calls has been reach
    /// </summary>
    public class PredictivePerCallFrequency : Frequency
    {
        private int totalCallsLimit = 0;
        private int totalActivationsLimit = 0;
        private int nbOfActivationDone = 0;
        private int nbOfCallsDone = 0;

        public PredictivePerCallFrequency(int nbOfActivation, int outOfTotalNbOfCalls) 
        {
            if (nbOfActivation > outOfTotalNbOfCalls)
            {
                throw new ArgumentException($"{nameof(nbOfActivation)} should be lower than {nameof(outOfTotalNbOfCalls)}");
            }

            totalCallsLimit = outOfTotalNbOfCalls;
            totalActivationsLimit = nbOfActivation;
        }

        public override bool ShouldTrigger()
        {
            var activate = false;
            nbOfCallsDone++;
            if (nbOfActivationDone < totalActivationsLimit)
            {
                nbOfActivationDone++;
                activate = true;
            }

            if (nbOfCallsDone == totalCallsLimit)
            {
                nbOfActivationDone = 0;
                nbOfCallsDone = 0;
            }

            return activate;
        }

        private int GreatestCommonDivisor(int a, int b)
        {
            if (a % b == 0)
            {
                return b;
            }
            return GreatestCommonDivisor(b, a % b);
        }
    }
}
