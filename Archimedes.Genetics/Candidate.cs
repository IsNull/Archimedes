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
        private readonly int _originGeneration;

        /// <summary>
        /// Creates a new Candidate from the given Origin generation
        /// </summary>
        /// <param name="originGeneration"></param>
        protected Candidate(int originGeneration)
        {
            _originGeneration = originGeneration;
        }

        #region Handling Genes / Allel

        /// <summary>
        /// Get the value assigned to the given Allel
        /// </summary>
        /// <param name="allel"></param>
        /// <returns></returns>
        public abstract double GetAllelValue(Allel allel);

        /// <summary>
        /// Set the value of the given Allel
        /// </summary>
        /// <param name="allel"></param>
        /// <param name="value"></param>
        public abstract void SetAllelValue(Allel allel, double value);

        /// <summary>
        /// Gets all allels (gen properties)
        /// </summary>
        public abstract Allel[] Allels { get; }

        #endregion

        /// <summary>
        /// Prototype / Copy the values of the given candidate to this one
        /// </summary>
        /// <param name="prototype"></param>
        public abstract void Prototype(Candidate prototype);


        /// <summary>
        /// Gets/Sets the Fitness of this candidate
        /// Fitness will be null until it has been calculated
        /// </summary>
        public double? Fitness { get; set; }

        /// <summary>
        /// Gets the Origin Generation from which this Candidate was created
        /// </summary>
        public int OriginGeneration
        {
            get { return _originGeneration; }
        }

        #region Implementation of IComparable<in Candidate>

        /// <summary>
        /// Compare this Candidate with another based on their Fitness
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(Candidate other)
        {
            if (this.Fitness.HasValue)
                return this.Fitness.Value.CompareTo(other.Fitness);
            return -1;
        }

        #endregion

        public override string ToString()
        {
            return "Fitness: " + Fitness + ", from Generation: " + OriginGeneration;
        }
    }
}
