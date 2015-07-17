using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Thrown when configuring a value has failed
    /// </summary>
    public class ValueConfigurationException : Exception
    {

        public ValueConfigurationException(string message, Exception cause)
            : base(message, cause)
        {
            
        }

    }
}
