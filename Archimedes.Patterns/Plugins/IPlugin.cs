using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Plugins
{
    /// <summary>
    /// Base Interface for each Plugin
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Human friendly Name of this Plugin
        /// </summary>
        string Name { get; }

        string Group { get; }

        string DllLocation { get; }

        /// <summary>
        /// Description of the Plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// CopyRight/Manufracturer of the Plugin
        /// </summary>
        string CopyRight { get; }

        Guid GUID { get; }

        /// <summary>
        /// Occurs when the Plugin is Loaded
        /// </summary>
        void OnLoad(string[] startupEventArgs);

        /// <summary>
        /// Occurs when the Plugin should unload
        /// </summary>
        void OnUnLoad();
    }
}
