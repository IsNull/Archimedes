using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archimedes.Maps.Services;
using RIEDOMaps.Model.Locations;

namespace Archimedes.Maps.GeoCoding
{

    /// <summary>
    /// Class to assync resolve Locations.
    /// This Class is Thread safe
    /// </summary>
    public class GeoCoderResolveTask
    {
        #region Fields

        readonly List<WorldLocation> _locationsToResolve;
        readonly IGeoCodingService _geoCodeService;

        #endregion

        #region Constructor

        public GeoCoderResolveTask(IEnumerable<WorldLocation> locations, IGeoCodingService geoCodeService)
        {
            _locationsToResolve = new List<WorldLocation>(locations);
            WaitTime = 100;
            _geoCodeService = geoCodeService;
        }

        #endregion

        #region Public static Methods

        /// <summary>
        /// Sync resolving of positions
        /// </summary>
        /// <returns></returns>
        public GeoCoderResponse GeoCoderResolve(Location loc) {
            var resolverTask = GeoCoderResolveAsync(loc);
            resolverTask.Wait();
            return resolverTask.Result;
        }

        /// <summary>
        /// ASync resolving of positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Task<GeoCoderResponse> GeoCoderResolveAsync(Location loc) {
            return GeoCoderResolveAsync(loc, false);
        }

        /// <summary>
        /// Sync resolving of cached positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Task<GeoCoderResponse> GeoCoderResolveCacheOnlyAsync(Location loc) {
            return GeoCoderResolveAsync(loc, true);
        }

        /// <summary>
        ///  Sync resolving of positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="cacheOnly"></param>
        /// <returns></returns>
        public async Task<GeoCoderResponse> GeoCoderResolveAsync(Location loc, bool cacheOnly) {

            bool fromCache = false;
            var status = GeoCodeStatus.None;
            GeoCoordinate? p = null;

            await Task.Run(() =>
            {
               GeoCoordinate result;
               _geoCodeService.GeoCoderResolve(loc, cacheOnly, out result, out status, out fromCache);
            });
            return new GeoCoderResponse(p, status, fromCache);;
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

        #endregion

        #region Public Methods

        public async Task ResolveLocationsAsync(CancellationToken ct) {

                bool sleepNeeded = false;

                while (_locationsToResolve.Any()) {

                    var copy = new List<WorldLocation>(_locationsToResolve);

                    foreach (var toResolve in copy) {
                        if (!toResolve.IsPositionResolved) {

                            var res = await GeoCoderResolveCacheOnlyAsync(toResolve);

                            if (res.HasPoint) {
                                toResolve.Position = res.ResolvedPoint.Value;
                                sleepNeeded = false;
                            } else {
                                res = await GeoCoderResolveAsync(toResolve, false);
                                toResolve.Position = res.ResolvedPoint.Value;

                                if (sleepNeeded) {
                                    // wait the time beeing
                                    Thread.Sleep(WaitTime);
                                    sleepNeeded = false;
                                } else {
                                    sleepNeeded = true;
                                }
                            }

                            switch (res.Status) {

                                case GeoCodeStatus.ExeededQueryLimit:
                                // leave this location object in the list and requery it later again
                                break;

                                case GeoCodeStatus.ServerError:
                                // leave this location object in the list and requery it later again
                                break;


                                default:
                                // We had a success or unresolvable error - take the location out of our ToDo List
                                ReportLocation(toResolve, res.Status);
                                break;
                            }

                            // check for cancel
                            ct.ThrowIfCancellationRequested();
                        } else {
                            ReportLocation(toResolve, GeoCodeStatus.Success);
                        }
                    }
                }
        }

        protected virtual void ReportLocation(WorldLocation location, GeoCodeStatus status)
        {
            _locationsToResolve.Remove(location);
        }



        #endregion
    }

    
}
