using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    public class GeoPolygon
    {
        private readonly List<GeoCoordinate> _coordinates = new List<GeoCoordinate>();

        public GeoPolygon()
        {
        }

        public GeoPolygon(IEnumerable<GeoCoordinate> coordinates)
        {
            _coordinates.AddRange(coordinates);
        }


        public virtual void Add(GeoCoordinate coordinate)
        {
            _coordinates.Add(coordinate);
        }

        public virtual void Remove(GeoCoordinate coordinate)
        {
            _coordinates.Remove(coordinate);
        }

        public virtual int Count
        {
            get { return _coordinates.Count; }
        }

        public bool IsEmpty
        {
            get { return !_coordinates.Any(); }
        }


        public GeoCoordinate this[int index]
        {
            get { return _coordinates[index]; }
        }


        /// <summary>
        /// Gets the Coordinates of this Geo-Polygon
        /// </summary>
        public virtual IEnumerable<GeoCoordinate> GetCoordinates()
        {
            return new List<GeoCoordinate>(_coordinates);
        }

    }
}
