using System;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Thrown when autowiring (injecting dependencies) has failed.
    /// </summary>
    public class AutowireException : Exception
    {

        public AutowireException(string message) : base(message)
        {
        }

        public AutowireException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
