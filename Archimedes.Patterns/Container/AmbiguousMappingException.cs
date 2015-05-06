using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// Thrown when a component implementation could not be resolved because
    /// there is an Ambiguous mapping.
    /// </summary>
    public class AmbiguousMappingException : Exception
    {
        public AmbiguousMappingException(string message) :base(message) { }
    }
}
