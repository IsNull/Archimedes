using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Maps;
using Archimedes.Maps.GeoCoding;

namespace RIEDOMaps.Model.Locations
{
    /// <summary>
    /// Represents a GeoCoder Response
    /// </summary>
    public struct GeoCoderResponse
    {
        /// <summary>
        /// Status of the Geocoder
        /// </summary>
        public GeoCodeStatus Status { get; private set; }

        /// <summary>
        /// Accuracy of the resolved point
        /// </summary>
        public LocationAccuracy Accuracy { get; private set; }

        /// <summary>
        /// Gets the Resolved point
        /// </summary>
        public GeoCoordinate? ResolvedPoint { get; private set; }

        public bool HasPoint
        {
            get { return ResolvedPoint.HasValue; }
        }

        /// <summary>
        /// Returns true if the response was from the cache
        /// </summary>
        public bool FromCache { get; private set; }

        public GeoCoderResponse(GeoCoordinate? point, GeoCodeStatus status, bool fromCache, LocationAccuracy accuracy)
            : this()
        {
            Status = status;
            ResolvedPoint = point;
            FromCache = fromCache;
            Accuracy = accuracy;
        }
    }
}
