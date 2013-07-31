using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Maps.Services;

namespace Archimedes.Maps.Routes
{
    /// <summary>
    /// Implements a simple memory cache for routes
    /// </summary>
    public class GeoRouteServiceCachedProxy : IGeoRouteService
    {
        private readonly List<MapRouteCacheItem> _cachedRoutes = new List<MapRouteCacheItem>();
        private readonly object _cachedRoutesLock = new object();
        private readonly IGeoRouteService _geoRouteService;

        /// <summary>
        /// Creates a new proxy for caching routes in memory
        /// </summary>
        /// <param name="routeService">The original route service which is used to fetch a route when it is not yet cached</param>
        public GeoRouteServiceCachedProxy(IGeoRouteService routeService)
        {
            _geoRouteService = routeService;
        }


        public GeoRoute GetRoute(GeoCoordinate start, GeoCoordinate end, int zoom)
        {
            return GetRoute(start, end, false, false, zoom);
        }

        public GeoRoute GetRoute(GeoCoordinate start, GeoCoordinate end, bool avoidHighways, bool walkingMode, int zoom)
        {
            GeoRoute route = null;
            IEnumerable<MapRouteCacheItem> routes;

            lock (_cachedRoutesLock)
            {
                routes = (from r in _cachedRoutes
                          where r.Route.From == start && r.Route.To == end && r.Zoom == zoom
                          select r).ToList();
            }


            if (routes.Any())
            {
                route = routes.First().Route;
            }
            else
            {
                var routeResoltion = zoom; // Math.Min(10, zoom);

                route = _geoRouteService.GetRoute(start, end, avoidHighways, walkingMode, routeResoltion);
                if (route != null)
                    CacheRoute(route, zoom);
            }
            return route;
        }


        private void CacheRoute(GeoRoute route, int zoom)
        {
            lock (_cachedRoutesLock)
            {
                _cachedRoutes.Add(new MapRouteCacheItem() { Route = route, Zoom = zoom, Count = 0 });
            }
        }

        struct MapRouteCacheItem
        {
            public GeoRoute Route;
            public int Zoom;
            public int Count;
        }
    }
}
