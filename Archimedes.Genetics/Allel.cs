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
        public string Name { get; set; }

        public Allel(string name)
        {
            Name = name;
        }
    }
}
