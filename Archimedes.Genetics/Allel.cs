using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Genetics
{
    /// <summary>
    /// Represents a property of a candidates Gen
    /// </summary>
    public class Allel
    {

        public Allel(string name)
            : this(name, double.MinValue, double.MaxValue)
        {
        }

        public Allel(string name, double min, double max)
        {
            Name = name;

            MinValue = min;
            MaxValue = max;
        }


        /// <summary>
        /// Name of this Allel
        /// </summary>
        public string Name { get; protected set; }


        public bool IsOnlyPositive
        {
            get { return MinValue >= 0; }
        }

        /// <summary>
        /// Min Value of this allel
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Max value of this allel
        /// </summary>
        public double MaxValue { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
