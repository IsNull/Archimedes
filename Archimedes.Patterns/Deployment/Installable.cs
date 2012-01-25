using System;
namespace Archimedes.Patterns.Deployment
{
    /// <summary>
    /// Represents an intem which can be installed
    /// </summary>
    interface Installable
    {
        /// <summary>
        /// Install this item
        /// <exception cref="System.Exception">Thrown when the item failed to install</exception>
        /// </summary>
        void Install();
    }
}
