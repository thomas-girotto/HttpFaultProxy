using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpFaultProxy.Model.Frequencies
{
    public class RandomPerCallFrequency : Frequency
    {
        private readonly Random random = new Random(); 
        private int nbOfCallsDone = 0;
        private int nbOfTriggersDone = 0;
        private int nextTriggerPosition = 0;
        private List<int> callsThatTrigger;

        public RandomPerCallFrequency(int nbOfActivation, int outOfTotalNbOfCalls)
        {
            if (nbOfActivation > outOfTotalNbOfCalls)
            {
                throw new ArgumentException($"{nameof(nbOfActivation)} should be lower than {nameof(outOfTotalNbOfCalls)}");
            }

            // We generate 1 ... 100 list and then we remove a random position in this list to only keep the right number of trigger we want.
            // The values in the remaining list will indicate the calls that should trigger.
            // This is a trick to avoid the problem of repetition if we just generate random to directly get the calls that should trigger.
            var possibleActivations = Enumerable.Range(1, outOfTotalNbOfCalls).ToList();
            for(int i = 0; i < outOfTotalNbOfCalls - nbOfActivation; i++)
            {
                var callToRemove = random.Next(0, possibleActivations.Count);
                possibleActivations.RemoveAt(callToRemove);
            }
            callsThatTrigger = possibleActivations;
        }

        public override bool ShouldTrigger()
        {
            nbOfCallsDone++;
            if (nbOfTriggersDone < callsThatTrigger.Count && nbOfCallsDone == callsThatTrigger[nextTriggerPosition])
            {
                nbOfTriggersDone++;
                nextTriggerPosition++;
                return true;
            }
            return false;
        }
    }
}
