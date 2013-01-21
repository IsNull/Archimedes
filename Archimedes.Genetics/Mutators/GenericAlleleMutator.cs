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
        


        public static T MutateRandom<T>(T candidate, Allel allel) where T : Candidate, new() 
        {
            var mutant = new T();
            mutant.Prototype(candidate);

            var currentValue = mutant.GetAllelValue(allel);
            var mutantValue = MutateRandom(currentValue, allel);
            mutant.SetAllelValue(allel, mutantValue);

            return mutant;
        }

        public static double MutateRandom(double value, Allel allel)
        {
            double n = _random.NextDouble()*(allel.MaxValue - allel.MinValue);
            return allel.MinValue + n;
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
