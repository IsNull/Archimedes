using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics.Mutators
{

    public class GenericAlleleMutator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Full mutation pitch 100% - the new mutated value replaced the old one completly 
        /// </summary>
        private const double FULL_MUTATION_PITCH = 1.0;

        /// <summary>
        /// Max pitch is 15% on slight mutations
        /// </summary>
        private const double DEFAULT_SLIGHT_MUTATION_PITCH = 0.02;
        

        public static T MutateRandom<T>(T candidate, Allel allel, Func<T> childCreator) where T : Candidate
        {
            var mutant = childCreator();
            mutant.Prototype(candidate);

            var currentValue = mutant.GetAllelValue(allel);
            var mutantValue = MutateRandom(currentValue, allel);
            mutant.SetAllelValue(allel, mutantValue);

            return mutant;
        }


        public static T MutateSlightlyRandom<T>(T candidate, Allel allel, Func<T> childCreator, double maxMutationPitch = DEFAULT_SLIGHT_MUTATION_PITCH) where T : Candidate
        {
            var mutant = childCreator();
            mutant.Prototype(candidate);

            var currentValue = mutant.GetAllelValue(allel);
            var mutantValue = MutateSlightlyRandom(currentValue, allel, maxMutationPitch);
            mutant.SetAllelValue(allel, mutantValue);

            return mutant;
        }

        /// <summary>
        /// Mutates the given value randomly
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allel"></param>
        /// <returns></returns>
        public static double MutateRandom(double value, Allel allel)
        {
            return MutateSlightlyRandom(value, allel, FULL_MUTATION_PITCH);
        }



        /// <summary>
        /// Mutates the given value slightly randomly
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allel"></param>
        /// <param name="maxMutationPitch">0.0 - 1.0</param>
        /// <returns></returns>
        public static double MutateSlightlyRandom(double value, Allel allel, double maxMutationPitch = DEFAULT_SLIGHT_MUTATION_PITCH)
        {
            double n = _random.NextDouble() * (allel.MaxValue - allel.MinValue);
            double newValue = allel.MinValue + n;
            
            // the new value is completly random. the max mutation pitch factor is
            // now used to determine, how much influence the new value has

            if (maxMutationPitch < FULL_MUTATION_PITCH)
            {
                //
                // a pitch of 1 would mean the old value is completly replaced
                // a pitch of 0.5 would mean that they are the avaerage
                // a pitch of 0 means that the new value has no effect
                // 
                newValue = value * (1.0d - maxMutationPitch) + newValue * (maxMutationPitch);
            }

            return newValue;
        }


        /// <summary>
        /// Mutate the value randomly, but ensure the mutatet value is bigger
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allel"></param>
        /// <returns></returns>
        public static double MutatePositive(double value, Allel allel)
        {
            if (allel.IsOnlyPositive)
            {
                double offset = (allel.MaxValue - value)*_random.NextDouble();
                return offset + value;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mutate the value randomly, but ensure the mutatet value is bigger
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allel"></param>
        /// <returns></returns>
        public static double MutateNegative(double value, Allel allel)
        {
            double offset = (value - allel.MinValue) * _random.NextDouble();
            return value - offset;
        }


        /// <summary>
        /// Gets a random Allel
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public static Allel PickRandomAllel(Candidate candidate)
        {
            return candidate.Allels[_random.Next(0, candidate.Allels.Length)];
        }


    }
}
