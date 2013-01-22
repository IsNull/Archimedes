using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics
{
    /// <summary>
    /// Describes a solution set
    /// </summary>
    public class Population<T> : IEnumerable<T> where T : Candidate
    {
        private readonly List<T> _populationMembers;


        public  Population(int generation)
            : this(generation, new List<T>())
        {

        }

        public Population(int generation, IEnumerable<T> candidates)
        {
            Generation = generation;
            _populationMembers = new List<T>(candidates);
        }

        public int Count
        {
            get { return _populationMembers.Count; }
        }


        public virtual void SortDescending()
        {
            SortDescending(_populationMembers);
        }

        public static void SortDescending(List<T> populationCandidates)
        {
            populationCandidates.Sort();
            populationCandidates.Reverse();
        }

        /// <summary>
        /// Gets the Generation of this population
        /// </summary>
        public int Generation { get; protected set; }

        /// <summary>
        /// Add the candidate to this population
        /// </summary>
        /// <param name="candidate"></param>
        public void Add(T candidate)
        {
            _populationMembers.Add(candidate);
        }

        public IEnumerable<T> GetSnapShot()
        {
            return new List<T>(_populationMembers);
        } 

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return _populationMembers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _populationMembers.GetEnumerator();
        }

        #endregion
    }
}
