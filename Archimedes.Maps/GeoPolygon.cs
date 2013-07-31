using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    /// <summary>
    /// Represents Geo Polyon data
    /// </summary>
    public interface IGeoPolygonData
    {
        /// <summary>
        /// Returns the amunt of GeoCoordinates in this Polygon
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns true if no coordinates are in this Polygon
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Get the coordinate at the specified index (check bounds!)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        GeoCoordinate this[int index] { get; }

        /// <summary>
        /// Gets the Coordinates of this Geo-Polygon
        /// </summary>
        IEnumerable<GeoCoordinate> GetCoordinates();
    }


    /// <summary>
    /// Represents an editable Geo Polygon consisting of GeoCoordinates
    /// </summary>
    public class GeoPolygon : IGeoPolygonData
    {
        private readonly List<GeoCoordinate> _coordinates = new List<GeoCoordinate>();

        public GeoPolygon()
        {
        }

        public GeoPolygon(IEnumerable<GeoCoordinate> coordinates)
        {
            _coordinates.AddRange(coordinates);
        }

        /// <summary>
        /// Adds the given coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        public virtual void Add(GeoCoordinate coordinate)
        {
            _coordinates.Add(coordinate);
        }

        /// <summary>
        /// Removes the given coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        public virtual void Remove(GeoCoordinate coordinate)
        {
            _coordinates.Remove(coordinate);
        }

        /// <summary>
        /// Returns the amount of geo coordinates in this polygon
        /// </summary>
        public virtual int Count
        {
            get { return _coordinates.Count; }
        }

        /// <summary>
        /// Is this geo polygon empty?
        /// </summary>
        public bool IsEmpty
        {
            get { return !_coordinates.Any(); }
        }

        /// <summary>
        /// Returns the geo coordinate at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
