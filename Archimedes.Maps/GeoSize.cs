using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    public struct GeoSize
    {
        public static readonly GeoSize Empty = new GeoSize();

        private double _heightLat;
        private double _widthLng;

        public GeoSize(GeoSize size)
        {
            this._widthLng = size._widthLng;
            this._heightLat = size._heightLat;
        }

        public GeoSize(GeoCoordinate pt)
        {
            this._heightLat = pt.Lat;
            this._widthLng = pt.Lng;
        }

        public GeoSize(double heightLat, double widthLng)
        {
            this._heightLat = heightLat;
            this._widthLng = widthLng;
        }

        public static GeoSize operator +(GeoSize sz1, GeoSize sz2)
        {
            return Add(sz1, sz2);
        }

        public static GeoSize operator -(GeoSize sz1, GeoSize sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(GeoSize sz1, GeoSize sz2)
        {
            return ((sz1.WidthLng == sz2.WidthLng) && (sz1.HeightLat == sz2.HeightLat));
        }

        public static bool operator !=(GeoSize sz1, GeoSize sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator GeoCoordinate(GeoSize size)
        {
            return new GeoCoordinate(size.HeightLat, size.WidthLng);
        }

        public bool IsEmpty
        {
            get { return ((this._widthLng == 0d) && (this._heightLat == 0d)); }
        }

        public double WidthLng
        {
            get { return this._widthLng; }
            set { this._widthLng = value; }
        }

        public double HeightLat
        {
            get { return this._heightLat; }
            set { this._heightLat = value; }
        }

        public static GeoSize Add(GeoSize sz1, GeoSize sz2)
        {
            return new GeoSize(sz1.HeightLat + sz2.HeightLat, sz1.WidthLng + sz2.WidthLng);
        }

        public static GeoSize Subtract(GeoSize sz1, GeoSize sz2)
        {
            return new GeoSize(sz1.HeightLat - sz2.HeightLat, sz1.WidthLng - sz2.WidthLng);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeoSize))
            {
                return false;
            }
            var ef = (GeoSize) obj;
            return (((ef.WidthLng == this.WidthLng) && (ef.HeightLat == this.HeightLat)) &&
                    ef.GetType().Equals(base.GetType()));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (this.WidthLng.GetHashCode() ^ this.HeightLat.GetHashCode());
        }

        public GeoCoordinate ToPointLatLng()
        {
            return (GeoCoordinate) this;
        }

        public override string ToString()
        {
            return ("{WidthLng=" + this._widthLng.ToString(CultureInfo.CurrentCulture) + ", HeightLng=" +
                    this._heightLat.ToString(CultureInfo.CurrentCulture) + "}");
        }

    }
}
