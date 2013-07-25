using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Archimedes.Patterns.Conditions
{
    /// <summary>
    /// Comparing Operator
    /// Uses bitwise flags
    /// </summary>
    [Flags]
    [DataContract]
    public enum Operator
    {
        [EnumMember] None = 0,
        [EnumMember] Equal = 1,
        [EnumMember] GreaterThan = 2,
        [EnumMember] SmallerThan = 4,
        [EnumMember] Not = 8
    }
}
