using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics.Recombinator
{
    public class GenericRecombinator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// 75% chance that the dominators allel is used
        /// </summary>
        private const double DEFAULT_DOMINATOR_ALLEL_PROBABILITY = 0.75;


        /// <summary>
        /// Hard recombination of two candidates. 
        /// This mens that either the allel value of the dominator or the submissive is applied.
        /// The chance that the dominators value is defined by dominatorAllelProbability 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dominator"></param>
        /// <param name="submissive"></param>
        /// <param name="childCreator"> </param>
        /// <param name="dominatorAllelProbability"></param>
        /// <returns></returns>
        public static T RecombinateHard<T>(T dominator, T submissive, Func<T> childCreator, double dominatorAllelProbability = DEFAULT_DOMINATOR_ALLEL_PROBABILITY) where T : Candidate
        {
            var child = childCreator();

            // apply each allel from either the dominator or the submissive parent
            foreach (var allel in dominator.Allels)
            {
                // genes of the successor parent are dominating
                // we simulate this with a monte carlo selection
                // of the given dominatorAllelProbability

                double value = (random.NextDouble() < dominatorAllelProbability) ? dominator.GetAllelValue(allel) : submissive.GetAllelValue(allel);
                child.SetAllelValue(allel, value);
            }
            return child;
        }

        /// <summary>
        /// Soft recombination of two candidates. 
        /// This means that a mix from the allel values is applied to the child 
        /// The chance that the dominators value is defined by dominatorAllelProbability 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dominator">The stronger parent</param>
        /// <param name="submissive">The weaker parent</param>
        /// <param name="childCreator"> </param>
        /// <param name="dominatorAllelProbability">Aritmethic weighted mean of the dominator 0.0 - 1.0</param>
        /// <returns></returns>
        public static T RecombinateSoft<T>(T dominator, T submissive, Func<T> childCreator, double dominatorAllelProbability = DEFAULT_DOMINATOR_ALLEL_PROBABILITY) 
            where T : Candidate
        {
            var child = childCreator();

            // apply each allel from either the dominator or the submissive parent
            foreach (var allel in dominator.Allels)
            {
                var dominatorValue = dominator.GetAllelValue(allel);
                var submissiveValue = submissive.GetAllelValue(allel);

                // we mix the values by a aritmethic weighted mean

                var value = (dominatorValue * dominatorAllelProbability) +
                            (submissiveValue * (1.0d - dominatorAllelProbability));

                child.SetAllelValue(allel, value);
            }
            return child;
        }


    }
}
