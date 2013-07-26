using System;
using Archimedes.Maps.GeoCoding;

namespace Archimedes.Maps.Services
{
    /// <summary>
    /// This service manages a batch queue of locations to be geocoded
    /// If a location has been processed it will raise the LocationResolved
    /// </summary>
    public interface IQueuedGeoCodingService
    {
        #region Events

        /// <summary>
        /// Raised when Location resolving has started
        /// </summary>
        event EventHandler ResolverStarted;

        /// <summary>
        /// Raised when a Location could be resolved.
        /// </summary>
        event EventHandler<LocationResolvedEventArgs> LocationResolved;

        /// <summary>
        /// Raised when this Worker has finished.
        /// All Locations are resolved (or impossible to resolve)
        /// </summary>
        event EventHandler ResolverDone;

        #endregion

        /// <summary>
        /// Adds the given location to the queue to be resolved
        /// </summary>
        /// <param name="location"></param>
        void AddToQueue(WorldLocation location);

        void CancelResolvingAsync();

        void ResolveLocationsAsync();

        /// <summary>
        /// Is the service currently busy?
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Time between a location resolving
        /// </summary>
        int WaitTime { get; set; }
    }
}
