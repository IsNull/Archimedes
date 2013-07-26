using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Maps.Services;
using Archimedes.Patterns.Services;

namespace Archimedes.Maps.Routes
{
    /// <summary>
    /// Represents a set of coordinates which for a route
    /// </summary>
    public class GeoRoute : GeoPolygon
    {
        private readonly IGeoProjectionService _projectionService = ServiceLocator.ResolveService<IGeoProjectionService>();


        /// <summary>
        /// Constructs a new GeoRoute
        /// </summary>
        /// <param name="points"></param>
        /// <param name="name"></param>
        public GeoRoute(IEnumerable<GeoCoordinate> points, string name)
            : base(points)
        {
            Name = name;
        }

        #region Properties

        /// <summary>
        /// Route info
        /// </summary>
        public string Name;

        /// <summary>
        /// Route start point
        /// </summary>
        public GeoCoordinate? From
        {
            get
            {
                if (!IsEmpty)
                {
                    return this[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Route end point
        /// </summary>
        public GeoCoordinate? To
        {
            get
            {
                if (!IsEmpty)
                {
                    return this[this.Count - 1];
                }
                return null;
            }
        }

        #endregion


        /// <summary>
        /// Route distance (in km)
        /// </summary>
        public double Distance
        {
            get
            {
                double distance = 0.0;

                if (From.HasValue && To.HasValue)
                {
                    for (int i = 1; i < Count; i++)
                    {
                        distance += _projectionService.GetDistance(this[i - 1], this[i]);
                    }
                }

                return distance;
            }
        }

    }
}
