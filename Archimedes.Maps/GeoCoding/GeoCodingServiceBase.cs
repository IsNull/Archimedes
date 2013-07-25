using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Maps.Services;
using Archimedes.Patterns.RegEx;

namespace Archimedes.Maps.GeoCoding
{
    /// <summary>
    /// Base implementation for GeoCoding Services
    /// </summary>
    public abstract class GeoCodingServiceBase : IGeoCodingService
    {

        #region Resolve Geo Coordinates

        /// <summary>
        /// Sync resolving of positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool GeoCoderResolve(Location loc, out GeoCoordinate point) {
            GeoCodeStatus dummy;
            bool fromCache;
            return GeoCoderResolve(loc, false, out point, out dummy, out fromCache);
        }

        /// <summary>
        /// Sync resolving of positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool GeoCoderResolve(Location loc, out GeoCoordinate point, out GeoCodeStatus status)
        {
            bool fromCache;
            return GeoCoderResolve(loc, false, out point, out status, out fromCache);
        }

        
        /// <summary>
        /// Sync resolving of cached positions
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool GeoCoderResolveCacheOnly(Location loc, out GeoCoordinate point)
        {
            GeoCodeStatus dummy;
            bool fromCache;
            return GeoCoderResolve(loc, true, out point, out dummy, out fromCache);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="cacheOnly"></param>
        /// <param name="point"></param>
        /// <param name="status"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public abstract bool GeoCoderResolve(Location loc, bool cacheOnly, out GeoCoordinate point, out GeoCodeStatus status, out bool fromCache);

        #endregion


    }
}
