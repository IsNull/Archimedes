using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns
{
    /// <summary>
    /// Clonable Instance type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T> where T : class
    {
        /// <summary>
        /// Clones this instance
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
