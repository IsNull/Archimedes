using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Maps.GeoCoding;

namespace Archimedes.Maps.Services
{
    /// <summary>
    /// Provides basic geocoding services
    /// </summary>
    public interface IGeoCodingService
    {
        /// <summary>
        /// Resolve the given address to a world coordinate
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        bool GeoCoderResolve(Location loc, out GeoCoordinate point);


        /// <summary>
        /// Resolve the given address to a world coordinate
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        bool GeoCoderResolveCacheOnly(Location loc, out GeoCoordinate point);

        /// <summary>
        /// Resolve the given address to a world coordinate
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="point"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool GeoCoderResolve(Location loc, out GeoCoordinate point, out GeoCodeStatus status);


        /// <summary>
        /// Resolve the given address to a world coordinate
        /// </summary>
        /// <param name="loc">The address</param>
        /// <param name="cacheOnly">Only check the cache</param>
        /// <param name="point">Out: The found coordinates</param>
        /// <param name="status">Out: GeoCoderStatus</param>
        /// <param name="fromCache">True indicates that the returned position was fetched from the cache</param>
        /// <param name="accuracy"> </param>
        /// <returns></returns>
        bool GeoCoderResolve(Location loc, bool cacheOnly, out GeoCoordinate point, out GeoCodeStatus status, out bool fromCache, out LocationAccuracy accuracy);

        /// <summary>
        /// Resolve the given address to a world coordinate
        /// </summary>
        /// <param name="keywords"> </param>
        /// <param name="cacheOnly">Only check the cache</param>
        /// <param name="point">Out: The found coordinates</param>
        /// <param name="status">Out: GeoCoderStatus</param>
        /// <param name="fromCache">True indicates that the returned position was fetched from the cache</param>
        /// <param name="accuracy"> </param>
        /// <returns></returns>
        bool GeoCoderResolve(string keywords, bool cacheOnly, out GeoCoordinate point, out GeoCodeStatus status, out bool fromCache, out LocationAccuracy accuracy);


        /// <summary>
        /// Resolve the given world coordinate to an address
        /// </summary>
        /// <param name="point">The point to check for</param>
        /// <param name="loc">Out: The found address</param>
        /// <returns>Status of the query</returns>
        GeoCodeStatus GeoCoderReverseResolve(GeoCoordinate point, out Location loc);



    }
}
