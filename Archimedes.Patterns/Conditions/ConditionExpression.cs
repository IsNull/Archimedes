using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Data;

namespace Archimedes.Patterns.Conditions
{
    public abstract class ConditionExpression : Entity<Guid, ConditionExpression>
    {

        public ConditionExpression() { }

        public ConditionExpression(Guid id) { this.ID = id; }


        public virtual IQueryable<object> Filter(IQueryable<object> dataQry) {
            if (dataQry == null)
                throw new ArgumentNullException("dataQry");

            return dataQry.Where(x => IsMatch(x));
        }

        public abstract string Name { get; }

        public abstract bool IsMatch(object other);
    }
}
