using System;
using System.Collections.Generic;
using System.Linq;
using Archimedes.Maps.Services;
using System.Threading;
using System.ComponentModel;
using Archimedes.Patterns.Services;
using RIEDOMaps.Model.Locations;

namespace Archimedes.Maps.GeoCoding
{
    /// <summary>
    /// Class to assync resolve Locations.
    /// This Class is Thread safe
    /// </summary>
    public class QueuedGeoCodingService : IQueuedGeoCodingService
    {
        #region Fields

        private readonly List<WorldLocation> _locationsToResolve;
        private readonly IGeoCodingService _geocoder;
        private readonly object _locationsLock = new object();
        private readonly BackgroundWorker _locationResolver = new BackgroundWorker();


        #endregion

        #region Events

        /// <summary>
        /// Raised when Location resolving has started
        /// </summary>
        public event EventHandler ResolverStarted;

        /// <summary>
        /// Raised when this Worker has finished.
        /// All Locations are resolved (or impossible to resolve)
        /// </summary>
        public event EventHandler ResolverDone;

        /// <summary>
        /// Raised when a Location could be resolved.
        /// </summary>
        public event EventHandler<LocationResolvedEventArgs> LocationResolved;

        #endregion

        #region Constructor

        public QueuedGeoCodingService()
            : this(ServiceLocator.ResolveService<IGeoCodingService>())
        {
        }

        public QueuedGeoCodingService(IGeoCodingService geocoderService)
        {
            _locationsToResolve = new List<WorldLocation>();
            _geocoder = geocoderService;

            WaitTime = 100;

            _locationResolver.WorkerSupportsCancellation = true;
            _locationResolver.DoWork += OnStartResolveLocations;
            _locationResolver.RunWorkerCompleted += OnResolvingDone;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start Resolving of all locations in list, if resolving is not running yet
        /// </summary>
        public void ResolveLocationsAsync() {
            if (!_locationResolver.IsBusy) {
                if (ResolverStarted != null)
                    ResolverStarted(this, EventArgs.Empty);
                _locationResolver.RunWorkerAsync();
            }
        }

        public void CancelResolvingAsync() {
            if (_locationResolver == null || !_locationResolver.IsBusy)
                throw new InvalidOperationException("Can't cancel as the operation is not pending now!");

            if (!_locationResolver.WorkerSupportsCancellation)
                throw new InvalidOperationException("Can't cancel as the operation is not Cancelable!");

            _locationResolver.CancelAsync();
        }

        /// <summary>
        /// Add a WorldLocation to the ToDo List.
        /// This Method is thread safe.
        /// </summary>
        /// <param name="location"></param>
        public void AddToQueue(WorldLocation location) {
            lock (_locationsLock) {
                _locationsToResolve.Add(location);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// WaitTime in ms between two resolve queries
        /// </summary>
        public int WaitTime {
            get;
            set;
        }

        /// <summary>
        /// Is geocoding in Progress?
        /// </summary>
        public bool IsBusy {
            get { return _locationResolver.IsBusy; }
        }

        #endregion

        #region Multithreaded Event Handlers

        /// <summary>
        /// This Event Handler is the worker thread to resolve Locations ascync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnStartResolveLocations(object sender, DoWorkEventArgs e) {
            GeoCodeStatus status;
            LocationAccuracy accuracy;
            GeoCoordinate locationPoint;
            var worker = sender as BackgroundWorker;
            bool fromCache;
            bool sleepNeeded = false;

            while (true) {
                List<WorldLocation> copy;
                lock (_locationsLock) {
                    if (_locationsToResolve.Any()) {
                        copy = new List<WorldLocation>(_locationsToResolve);
                    } else
                        break;
                }
                foreach(var toResolve in copy){
                    if (!toResolve.IsPositionResolved) {

                        if (_geocoder.GeoCoderResolveCacheOnly(toResolve, out locationPoint)) {
                            status = GeoCodeStatus.Success;
                            toResolve.SetPosition(locationPoint, LocationAccuracy.NotSupported);
                        }
                        else
                        {
                            if (sleepNeeded)
                            {
                                // cool down
                                Thread.Sleep(WaitTime);
                                sleepNeeded = false;
                            }

                            if (_geocoder.GeoCoderResolve(toResolve, false, out locationPoint, out status, out fromCache, out accuracy))
                            {
                                toResolve.SetPosition(locationPoint, accuracy);
                            }
                        }
                        
 
                        switch (status) {

                            case GeoCodeStatus.ExeededQueryLimit:
                                // leave this location object in the list and requery it later again
                                sleepNeeded = true;
                            break;

                            case GeoCodeStatus.ServerError:
                                // leave this location object in the list and requery it later again
                            break;

                                // TODO: only try a certain amount of times - no endles loop here...

                            default:
                            // We had a success or unresolvable error - take the location out of our ToDo List
                            HandleLocation(toResolve, status);
                            break;
                        }

                        // check for cancel
                        if (worker != null && worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return; // exit this thread
                        }
                    } else {
                        HandleLocation(toResolve, GeoCodeStatus.Success);
                    }
                }
            }
        }

        /// <summary>
        /// Handle a resolved location
        /// </summary>
        /// <param name="resolvedTarget"></param>
        /// <param name="geoCoderStatus"></param>
        void HandleLocation(WorldLocation resolvedTarget, GeoCodeStatus geoCoderStatus)
        {
            /*
            if (geoCoderStatus == GeoCodeStatus.Success)
            {
                switch (resolvedTarget.Accuracy)
                {
                        case LocationAccuracy.Unknown:
                        case LocationAccuracy.GeometricCenter:
                        case LocationAccuracy.Approximate:
                        return;
                }
            }
             */


            OnLocationResolved(resolvedTarget, geoCoderStatus);
        }

        /// <summary>
        /// Occurs when a location has been resolved.
        /// The location is removed from the queue and reported to listeners as resolved
        /// </summary>
        /// <param name="resolvedTarget"></param>
        /// <param name="geoCoderStatus"></param>
        void OnLocationResolved(WorldLocation resolvedTarget, GeoCodeStatus geoCoderStatus)
        {
            lock (_locationsLock)
            {
                _locationsToResolve.Remove(resolvedTarget);
            }
            // report the resolved location
            if (LocationResolved != null)
                LocationResolved(this, new LocationResolvedEventArgs(resolvedTarget, geoCoderStatus));
        }

        void OnResolvingDone(object sender, RunWorkerCompletedEventArgs e) {
            if (ResolverDone != null)
                ResolverDone(this, null);
        }

        #endregion
    }


    public class LocationResolvedEventArgs : EventArgs
    {
        public LocationResolvedEventArgs(WorldLocation resolvedTarget, GeoCodeStatus geoCoderStatus)
        {
            ResolvedTarget = resolvedTarget;
            GeoCoderStatus = geoCoderStatus;
        }
        public WorldLocation ResolvedTarget { get; private set; }
        public GeoCodeStatus GeoCoderStatus { get; private set; }
    }

}
