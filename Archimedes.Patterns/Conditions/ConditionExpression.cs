using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Data;

namespace Archimedes.Patterns.Conditions
{
    /// <summary>
    /// Represents an abstract condition
    /// </summary>
    public abstract class ConditionExpression : Entity<Guid, ConditionExpression>
    {

        public ConditionExpression() { }

        public ConditionExpression(Guid id) { this.ID = id; }

        /// <summary>
        /// Filters a enumerable list of objects by this condtition
        /// </summary>
        /// <param name="dataQry"></param>
        /// <returns></returns>
        public virtual IQueryable<object> Filter(IQueryable<object> dataQry) {
            if (dataQry == null)
                throw new ArgumentNullException("dataQry");

            return dataQry.Where(x => IsMatch(x));
        }

        /// <summary>
        /// Gets the Name of this Condtion
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns true if the given Object matches this Condition
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool IsMatch(object other);
    }
}
