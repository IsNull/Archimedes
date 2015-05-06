using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// Thrown when autowiring (injecting dependencies) has failed.
    /// </summary>
    public class AutowireException : Exception
    {
        public AutowireException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
