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

        bool GeoCoderResolve(Location loc, out GeoCoordinate point);

        bool GeoCoderResolve(Location loc, out GeoCoordinate point, out GeoCodeStatus status);

        bool GeoCoderResolveCacheOnly(Location loc, out GeoCoordinate point);

        bool GeoCoderResolve(Location loc, bool cacheOnly, out GeoCoordinate point, out GeoCodeStatus status, out bool fromCache);

    }
}
