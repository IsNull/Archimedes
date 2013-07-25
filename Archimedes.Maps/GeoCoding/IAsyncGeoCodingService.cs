using System;

namespace Archimedes.Maps.GeoCoding
{
    /// <summary>
    /// This Service exports Methods to async resolve (geocoding) a couple of locations
    /// </summary>
    public interface IAsyncGeoCodingService
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

        void AddToQueue(WorldLocation location);

        void CancelResolvingAsync();

        void ResolveLocationsAsync();

        bool IsBusy { get; }

        int WaitTime { get; set; }
    }
}
