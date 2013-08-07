using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    /// <summary>
    /// Represents a single coordinate point
    /// </summary>
    [Serializable]
    [DataContract]
    public struct GeoCoordinate
    {
        [NonSerialized]
        public static readonly GeoCoordinate Zero = new GeoCoordinate();

        [DataMember]
        private readonly double _lat;
        [DataMember]
        private readonly double _lng;

        public GeoCoordinate(double lat, double lng)
        {
            this._lat = lat;
            this._lng = lng;
        }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat
        {
            get
            {
                return this._lat;
            }
        }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Lng
        {
            get
            {
                return this._lng;
            }
        }

        public bool IsZero
        {
            get { return this == Zero; }
        }

        #region Operators

        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            return ((left.Lng == right.Lng) && (left.Lat == right.Lat));
        }

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !(left == right);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GeoCoordinate && Equals((GeoCoordinate) obj);
        }

        public bool Equals(GeoCoordinate other)
        {
            return _lat.Equals(other._lat) && _lng.Equals(other._lng);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_lat.GetHashCode() * 397) ^ _lng.GetHashCode();
            }
        }

        public GeoCoordinate Offset(GeoCoordinate pos)
        {
            return this.Offset(pos.Lat, pos.Lng);
        }

        public GeoCoordinate Offset(double lat, double lng)
        {
            return new GeoCoordinate(Lat + lat, Lng + lng);
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Lat={0}, Lng={1}}}", this.Lat, this.Lng);
        }
    }
}
