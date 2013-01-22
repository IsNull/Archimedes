using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Archimedes.Genetics
{
    /// <summary>
    /// Encapsulates the toplevel logic of the genetic algorythm
    /// Subclasses specialize the algorythm to solve the actual problem
    /// </summary>
    public abstract class GeneticAlgorythm<T> where T : Candidate
    {
        #region Fields

        protected Random Random = new Random();

        private int _generation = 0;
        private double _mutationProbability = 0.9;
        private double _recombinationProbability = 0.9;
        private int _maxGeneration = 50;
        private int _maxPopulation = 500;

        #endregion

        public event EventHandler GenerationDoneEvent;

        public int Generation
        {
            get { return _generation; }
        }

        public virtual void Reset()
        {
            _generation = 0;
            BestCandidate = null;
            CurrentPopulation = null;
        }


        /// <summary>
        /// Start the evolution with the given initial population
        /// </summary>
        /// <param name="initialPopulation"></param>
        /// <param name="cancellationToken"> </param>
        public void StartEvolution(IEnumerable<T> initialPopulation, CancellationToken cancellationToken)
        {
            
            CurrentPopulation = new Population<T>(0, initialPopulation);
            CurrentPopulation.AsParallel().ForAll(CalculateFitness);


            if (GenerationDoneEvent != null)
                GenerationDoneEvent(this, EventArgs.Empty);


            while (!IsAbort())
            {
                CurrentPopulation = CreateNewPopulationGeneration(CurrentPopulation.GetSnapShot());

                // calculate fitness

                CurrentPopulation.AsParallel().ForAll(CalculateFitness);


                CurrentPopulation.SortDescending();
                // trim population
                CurrentPopulation = new Population<T>(CurrentPopulation.Generation, Selection(CurrentPopulation.GetSnapShot()));

                // remember the best candidate so far

                var populationSuccessor = CurrentPopulation.FirstOrDefault();
                if(populationSuccessor != null)
                {
                    if (BestCandidate == null || BestCandidate.Fitness < populationSuccessor.Fitness)
                        BestCandidate = populationSuccessor;
                }

                if (GenerationDoneEvent != null)
                    GenerationDoneEvent(this, EventArgs.Empty);

                if(cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Creates a new generation of population
        /// </summary>
        /// <returns></returns>
        protected virtual Population<T> CreateNewPopulationGeneration(IEnumerable<T> parents)
        {
            //  selection
            var newPopulation = Selection(parents).ToList();

            int count = newPopulation.Count();
            //Console.WriteLine("population count" + count + "max population:" +  MaxPopulation);

            // recombination
            newPopulation = Recombination(newPopulation).ToList();

            // mutation
            newPopulation = Mutation(newPopulation).ToList();

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
        protected virtual IEnumerable<T> Recombination(IEnumerable<T> currentPopulation)
        {
            var population = currentPopulation.ToList();
            var pop = new List<T>(population);

            if (population.Count > 2)
            {
                foreach (var candidate in population)
                {
                    if (Random.NextDouble() < RecombinationProbability)
                    {
                        pop.Add(Recombine(candidate, population[Random.Next(0, population.Count)]));
                    }
                }
            }

            return pop;
        }

        /// <summary>
        /// Mutate existing as new mutated candidates
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> Mutation(IEnumerable<T> currentPopulation)
        {
            var population = currentPopulation.ToList();

            var pop = new List<T>(population);


            foreach (var candidate in population)
            {
                if(Random.NextDouble() < MutationProbability)
                {
                    pop.Add(Mutate(candidate));
                }
            }


            return pop;
        }

        public abstract T Mutate(T candidate);

        public abstract T Recombine(T parent1, T submissive);

        /// <summary>
        /// Do we have to abort the evolution?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAbort()
        {
            return CurrentPopulation == null || CurrentPopulation.Generation >= MaxGeneration || CurrentPopulation.Count == 0;
        }


        /// <summary>
        /// Gets the best Candidate
        /// </summary>
        public T BestCandidate { get; set; }


        /// <summary>
        /// Maximum generation in evolution
        /// </summary>
        public int MaxGeneration
        {
            get { return _maxGeneration; }
            set { _maxGeneration = value; }
        }

        /// <summary>
        /// The size of a Population
        /// </summary>
        public int MaxPopulation
        {
            get { return _maxPopulation; }
            set { _maxPopulation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MutationProbability
        {
            get { return _mutationProbability; }
            set { _mutationProbability = value; }
        }

        public double RecombinationProbability
        {
            get { return _recombinationProbability; }
            set { _recombinationProbability = value; }
        }

        /// <summary>
        /// Gets the current population
        /// </summary>
        public Population<T> CurrentPopulation { get; protected set; }


        /// <summary>
        /// Calculate the fitness for the given candidate
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected abstract void CalculateFitness(T candidate);

    }
}
