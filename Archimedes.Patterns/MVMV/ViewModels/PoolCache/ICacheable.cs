using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.MVMV.ViewModels.PoolCache
{
    /// <summary>
    /// Marks the class as Cacheable
    /// </summary>
    public interface ICacheable
    {
        /// <summary>
        /// Raised when the object is no longer needed to be cached
        /// </summary>
        event EventHandler CacheExpired;
    }
}
