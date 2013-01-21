using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics
{
    /// <summary>
    /// Base class for a solution candidate
    /// </summary>
    public abstract class Candidate : IComparable<Candidate>
    {
        private Dictionary<Allel, double> _gen;

        protected Candidate(Dictionary<Allel, double> gen)
        {
            _gen = gen;
        }

        /// <summary>
        /// Gets the Fitness of this candidate
        /// </summary>
        public double Fitness { get; internal set; }

        #region Implementation of IComparable<in Candidate>

        public int CompareTo(Candidate other)
        {
            return this.Fitness.CompareTo(other.Fitness);
        }

        #endregion
    }
}
