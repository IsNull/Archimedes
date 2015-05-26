using System;

namespace Archimedes.Patterns.Configuration
{
    public class UnknownParameterException : Exception
    {
        public UnknownParameterException(string parameter)
            : base("The requiered parameter '" + parameter  + "' was not specified in the App settings!")
        {
            
        }
    }
}
 