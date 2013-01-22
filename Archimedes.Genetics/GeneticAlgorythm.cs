using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Genetics.Mutators;

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
        private double _mutationProbability = 0.75;
        private double _recombinationProbability = 0.75;
        private int _maxGeneration = 150;
        private int _maxPopulation = 500;
        private int _maxAge = 10;
        private int _eliteDevelopmentDeepnes = 20;

        #endregion

        #region Events

        /// <summary>
        /// Raised when a Generation has been created and rated
        /// </summary>
        public event EventHandler GenerationDoneEvent;

        #endregion

        #region Settings Properties


        /// <summary>
        /// Maximum Age of an Candidate
        /// </summary>
        public int MaxAge
        {
            get { return _maxAge; }
            set { _maxAge = value; }
        }


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
        /// How many elite development samples shall be done?
        /// 0 means no elite development
        /// </summary>
        public int EliteDevelopmentDeepnes
        {
            get { return _eliteDevelopmentDeepnes; }
            set { _eliteDevelopmentDeepnes = value; }
        }

        /// <summary>
        /// Probability that a mutation occurs
        /// </summary>
        public double MutationProbability
        {
            get { return _mutationProbability; }
            set { _mutationProbability = value; }
        }

        /// <summary>
        /// Probability that a candidate has a child
        /// </summary>
        public double RecombinationProbability
        {
            get { return _recombinationProbability; }
            set { _recombinationProbability = value; }
        }

        #endregion


        /// <summary>
        /// Gets the current Generation
        /// </summary>
        public int Generation
        {
            get { return _generation; }
        }

        /// <summary>
        /// Reset the algorythm implementation
        /// </summary>
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
        /// <param name="cancellationToken">Token used to cancel the evolution prematurely</param>
        public void StartEvolution(IEnumerable<T> initialPopulation, CancellationToken cancellationToken)
        {
            
            // initialisation
            CurrentPopulation = new Population<T>(0, initialPopulation);
            CurrentPopulation.AsParallel().ForAll(CalculateFitness);
            CurrentPopulation.SortDescending();

            if (GenerationDoneEvent != null)
                GenerationDoneEvent(this, EventArgs.Empty);


            // start evolution main loop

            while (!IsAbort())
            {
                NextGeneration();

                // generate a new population based upon the current parents
                var newIndividuums = CreateWeakElitePopulationGeneration(CurrentPopulation.GetSnapShot()).ToList();

                // calculate fitness and sort the candidates
                EnsureFitnessIsCalculated(newIndividuums);

                // trim population
                CurrentPopulation = new Population<T>(Generation, Selection(newIndividuums));

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

        protected void EnsureFitnessIsCalculated(List<T> population)
        {
            population.AsParallel().ForAll(CalculateFitness);
            Population<T>.SortDescending(population);
        }

        /// <summary>
        /// Move on to the next Generation
        /// </summary>
        protected void NextGeneration()
        {
            _generation++;
        }

        /// <summary>
        /// Creates a new generation of population
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<T> CreateElitePopulationGeneration(IEnumerable<T> parents)
        {
            //  selection of elite parents
            var eliteParents = Selection(parents).ToList();

            // recombination
            var newPopulation = Recombination(eliteParents).ToList();
            newPopulation.AddRange(eliteParents);

            // mutation
            newPopulation = Mutation(newPopulation).ToList();

            return newPopulation;
        }

        /// <summary>
        /// Creates a new generation of population
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<T> CreateWeakElitePopulationGeneration(IEnumerable<T> parents)
        {
            //  selection
            var eliteParents = Selection(parents).ToList();

            // recombination
            var newPopulation = Recombination(eliteParents).ToList();

            // mutation
            newPopulation = Mutation(newPopulation).ToList();

            // age will kill some
            newPopulation = HandleAge(newPopulation).ToList();

            // ensure that the population has enough candidates
            newPopulation = RePopulate(parents, newPopulation).ToList();

            // develop the elite
            newPopulation = EliteDevelopment(newPopulation).ToList();


            return newPopulation;
        }

        /// <summary>
        /// Returns a subset of the given population, based upon the age (simulathe death)
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> HandleAge(IEnumerable<T> population)
        {
            return from p in population
                   where MaxAge <= 0 || (Generation - p.OriginGeneration) < MaxAge
                   select p;
        }

        /// <summary>
        ///  Refill population from parents or any other source
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="newPopulation"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> RePopulate(IEnumerable<T> parents, IEnumerable<T> newPopulation)
        {
            var newGeneration = new List<T>(newPopulation);

            if (MaxPopulation > newGeneration.Count)
            {
                // refill population from parents
                var myparents = HandleAge(parents).Take(MaxPopulation - newGeneration.Count);
                myparents = Mutation(myparents);

                newGeneration.AddRange(myparents);
            }
            return newGeneration;
        }


        

        /// <summary>
        /// Develops the given populations elite.
        /// The idea is to improve local elites with slight asjustments to their genes.
        /// This method guarantees an improovment or at least a stagnation of the top fitness.
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> EliteDevelopment(IEnumerable<T> population)
        {

            if (_eliteDevelopmentDeepnes < 1) return population;


            var mypopulation = population.ToList();
            if (BestCandidate != null) // ensure that our current top candidate is in our population
                mypopulation.Add(BestCandidate);

            EnsureFitnessIsCalculated(mypopulation);

            // take the best 5 candidates
            var elite = mypopulation.Distinct().Take(5).ToList();

            // slightly mutate the elite candidates

            var eliteMutants = new List<T>();

            foreach (var eliteCandidate in elite)
            {
                for (int i = 0; i < _eliteDevelopmentDeepnes; i++)
                {
                    var allel = GenericAlleleMutator.PickRandomAllel(eliteCandidate);
                    var mutant = GenericAlleleMutator.MutateSlightlyRandom(eliteCandidate, allel, ChildCurrentGenerationCreator);

                    eliteMutants.Add(mutant);
                }
            }

            eliteMutants.AddRange(elite);
            EnsureFitnessIsCalculated(eliteMutants);

            mypopulation.RemoveAll(elite.Contains);

            mypopulation.AddRange(eliteMutants.Take(5));

            return mypopulation;
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
            var pop = new List<T>();

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
            var mutatedPopulation = new List<T>(population.Count);


            foreach (var candidate in population)
            {
                if(Random.NextDouble() < MutationProbability)
                {
                    // mutate
                    mutatedPopulation.Add(Mutate(candidate));
                }else
                {
                    // leave it as is
                    mutatedPopulation.Add(candidate);
                }
            }

            return mutatedPopulation;
        }

        /// <summary>
        /// Mutate the given candidate
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public abstract T Mutate(T candidate);

        /// <summary>
        /// Recombine a child from the given parents
        /// </summary>
        /// <param name="dominator"></param>
        /// <param name="submissive"></param>
        /// <returns></returns>
        public abstract T Recombine(T dominator, T submissive);

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
        /// Gets the current population
        /// </summary>
        public Population<T> CurrentPopulation { get; protected set; }


        /// <summary>
        /// Calculate the fitness for the given candidate
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        protected abstract void CalculateFitness(T candidate);

        /// <summary>
        /// 
        /// </summary>
        protected abstract Func<T> ChildCurrentGenerationCreator { get; }
    

    }
}
