using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Maps.Routes;

namespace Archimedes.Maps.Services
{
    /// <summary>
    /// Provides route/direction services
    /// </summary>
    public interface IGeoRouteService
    {
        /// <summary>
        /// Find a route between two points
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom">Route-Point resolution</param>
        /// <returns></returns>
        GeoRoute GetRoute(GeoCoordinate start, GeoCoordinate end, bool avoidHighways, bool walkingMode, int zoom);
    }
}
