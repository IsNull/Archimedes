using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Archimedes.Patterns.Utils
{
    public static class ThrowUtil
    {
        /// <summary>
        /// Throws an exception when the obj Parameter is null.
        /// </summary>
        /// <param name="obj"></param>
        [DebuggerStepThrough]
        public static void ThrowIfNull(object obj) {
            if(obj == null)
                throw new ArgumentNullException();
        }
    }
}
