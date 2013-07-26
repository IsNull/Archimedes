using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    public struct GeoRectangle
    {

        public static readonly GeoRectangle Empty = new GeoRectangle();
        private double lng;
        private double lat;
        private double widthLng;
        private double heightLat;

        public GeoRectangle(double lat, double lng, double widthLng, double heightLat)
        {
            this.lng = lng;
            this.lat = lat;
            this.widthLng = widthLng;
            this.heightLat = heightLat;
        }

        public GeoRectangle(GeoCoordinate location, GeoSize size)
        {
            this.lng = location.Lng;
            this.lat = location.Lat;
            this.widthLng = size.WidthLng;
            this.heightLat = size.HeightLat;
        }

        public static GeoRectangle FromLTRB(double leftLng, double topLat, double rightLng, double bottomLat)
        {
            return new GeoRectangle(topLat, leftLng, rightLng - leftLng, topLat - bottomLat);
        }

        public GeoCoordinate LocationTopLeft
        {
            get { return new GeoCoordinate(this.Lat, this.Lng); }
            set
            {
                this.Lng = value.Lng;
                this.Lat = value.Lat;
            }
        }

        public GeoCoordinate LocationRightBottom
        {
            get
            {
                var ret = new GeoCoordinate(this.Lat, this.Lng);
                ret.Offset(HeightLat, WidthLng);
                return ret;
            }
        }

        public GeoCoordinate LocationMiddle
        {
            get
            {
                var ret = new GeoCoordinate(this.Lat, this.Lng);
                ret.Offset(HeightLat/2, WidthLng/2);
                return ret;
            }
        }

        public GeoSize Size
        {
            get { return new GeoSize(this.HeightLat, this.WidthLng); }
            set
            {
                this.WidthLng = value.WidthLng;
                this.HeightLat = value.HeightLat;
            }
        }

        public double Lng
        {
            get { return this.lng; }
            set { this.lng = value; }
        }

        public double Lat
        {
            get { return this.lat; }
            set { this.lat = value; }
        }

        public double WidthLng
        {
            get { return this.widthLng; }
            set { this.widthLng = value; }
        }

        public double HeightLat
        {
            get { return this.heightLat; }
            set { this.heightLat = value; }
        }

        public double Left
        {
            get { return this.Lng; }
        }

        public double Top
        {
            get { return this.Lat; }
        }

        public double Right
        {
            get { return (this.Lng + this.WidthLng); }
        }

        public double Bottom
        {
            get { return (this.Lat - this.HeightLat); }
        }

        public bool IsEmpty
        {
            get
            {
                if (this.WidthLng > 0d)
                {
                    return (this.HeightLat <= 0d);
                }
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeoRectangle))
            {
                return false;
            }
            var ef = (GeoRectangle) obj;
            return ((((ef.Lng == this.Lng) && (ef.Lat == this.Lat)) && (ef.WidthLng == this.WidthLng)) &&
                    (ef.HeightLat == this.HeightLat));
        }

        public static bool operator ==(GeoRectangle left, GeoRectangle right)
        {
            return ((((left.Lng == right.Lng) && (left.Lat == right.Lat)) && (left.WidthLng == right.WidthLng)) &&
                    (left.HeightLat == right.HeightLat));
        }

        public static bool operator !=(GeoRectangle left, GeoRectangle right)
        {
            return !(left == right);
        }

        public bool Contains(double lat, double lng)
        {
            return ((((this.Lng <= lng) && (lng < (this.Lng + this.WidthLng))) && (this.Lat >= lat)) &&
                    (lat > (this.Lat - this.HeightLat)));
        }

        public bool Contains(GeoCoordinate pt)
        {
            return this.Contains(pt.Lat, pt.Lng);
        }

        public bool Contains(GeoRectangle rect)
        {
            return ((((this.Lng <= rect.Lng) && ((rect.Lng + rect.WidthLng) <= (this.Lng + this.WidthLng))) &&
                     (this.Lat >= rect.Lat)) && ((rect.Lat - rect.HeightLat) >= (this.Lat - this.HeightLat)));
        }

        public override int GetHashCode()
        {
            if (this.IsEmpty)
            {
                return 0;
            }
            return (((this.Lng.GetHashCode() ^ this.Lat.GetHashCode()) ^ this.WidthLng.GetHashCode()) ^
                    this.HeightLat.GetHashCode());
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

        public override string ToString()
        {
            return ("{Lat=" + this.Lat.ToString(CultureInfo.CurrentCulture) + ",Lng=" +
                    this.Lng.ToString(CultureInfo.CurrentCulture) + ",WidthLng=" +
                    this.WidthLng.ToString(CultureInfo.CurrentCulture) + ",HeightLat=" +
                    this.HeightLat.ToString(CultureInfo.CurrentCulture) + "}");
        }
    }
}
