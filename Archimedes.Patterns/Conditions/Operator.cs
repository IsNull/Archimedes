using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Conditions
{
    /// <summary>
    /// Comparing Operator
    /// Uses bitwise flags
    /// </summary>
    [Flags]
    public enum Operator
    {
        None = 0,
        Equal = 1,
        GreaterThan = 2,
        SmallerThan = 4,
        Not = 8
    }
}
