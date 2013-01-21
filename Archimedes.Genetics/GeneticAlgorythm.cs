using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics
{
    /// <summary>
    /// Encapsulates the toplevel logic of the genetic algorythm
    /// 
    /// </summary>
    public abstract class GeneticAlgorythm<T> where T : Candidate
    {
        private int _generation = 0;

        /// <summary>
        /// Start the evolution with the given initial population
        /// </summary>
        /// <param name="initialPopulation"></param>
        public void StartEvolution(List<T> initialPopulation)
        {
            CurrentPopulation = new Population<T>(0, initialPopulation);

            while (!IsAbort())
            {
                CurrentPopulation =  CreateNewPopulationGeneration();
                // calculate fitness

                foreach (var candidate in CurrentPopulation)
                {
                    candidate.Fitness = CalculateFitness(candidate);
                }
            }
        }

        /// <summary>
        /// Creates a new generation of population
        /// </summary>
        /// <returns></returns>
        protected virtual Population<T> CreateNewPopulationGeneration()
        {
            //  selection
            var newPopulation = Selection(CurrentPopulation.GetSnapShot());
            // recombination
            newPopulation = Recombination(newPopulation);
            // mutation
            newPopulation = Mutation(newPopulation);

            return new Population<T>(++_generation, newPopulation);
        }

        /// <summary>
        /// Select a subset of the 
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        protected abstract IEnumerable<T> Selection(IEnumerable<T> currentPopulation);

        /// <summary>
        /// Recombine existing candidates into new ones
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        protected abstract IEnumerable<T> Recombination(IEnumerable<T> currentPopulation);

        /// <summary>
        /// Mutate existing, add new mutated
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        protected abstract IEnumerable<T> Mutation(IEnumerable<T> currentPopulation); 

        /// <summary>
        /// Do we have to abort the evolution?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAbort()
        {
            return CurrentPopulation == null || CurrentPopulation.Generation >= MaxGeneration || CurrentPopulation.Count == 0;
        }

        public int MaxGeneration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PopulationSize { get; set; }

        /// <summary>
        /// Gets the current population
        /// </summary>
        public Population<T> CurrentPopulation { get; protected set; }


        /// <summary>
        /// Calculate the fitness for the given candidate
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected abstract double CalculateFitness(T candidate);

    }
}
