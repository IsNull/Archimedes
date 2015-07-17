using System.Collections.Generic;
using Archimedes.Patterns;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Loads the default configuration using the application.properties files if available.
        /// </summary>
        /// <param name="commandLineArgs"></param>
        void LoadConfiguration(string[] commandLineArgs = null);

        /// <summary>
        /// Gets the current configuration
        /// </summary>
        Properties Configuration { get; }
    }
}
