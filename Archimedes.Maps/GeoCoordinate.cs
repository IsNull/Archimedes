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
        public static readonly GeoCoordinate Empty = new GeoCoordinate();
        [DataMember]
        private double _lat;
        [DataMember]
        private double _lng;

        bool _notEmpty;

        public GeoCoordinate(double lat, double lng)
        {
            this._lat = lat;
            this._lng = lng;
            _notEmpty = true;
        }

        /// <summary>
        /// Returns true if coordinates wasn't assigned
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return !_notEmpty;
            }
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
            set
            {
                this._lat = value;
                _notEmpty = true;
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
            set
            {
                this._lng = value;
                _notEmpty = true;
            }
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
            if (!(obj is GeoCoordinate))
            {
                return false;
            }
            var tf = (GeoCoordinate)obj;
            return (((tf.Lng == this.Lng) && (tf.Lat == this.Lat)) && tf.GetType().Equals(base.GetType()));
        }

        public void Offset(GeoCoordinate pos)
        {
            this.Offset(pos.Lat, pos.Lng);
        }

        public void Offset(double lat, double lng)
        {
            this.Lng += lng;
            this.Lat -= lat;
        }

        public override int GetHashCode()
        {
            return (this.Lng.GetHashCode() ^ this.Lat.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Lat={0}, Lng={1}}}", this.Lat, this.Lng);
        }
    }
}
