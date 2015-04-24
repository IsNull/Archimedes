using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Represents an axis aligned rectangle
    /// </summary>
    public class RectangleAA2 : IShape
    {
        #region Fields

        private Vector2 _location;
        private SizeD _size;

        #endregion


        public static RectangleAA2 Empty
        {
            get { return new RectangleAA2(Vector2.Zero, SizeD.Empty);}
        }

        #region Constructors


        public RectangleAA2(double x, double y, double width, double height) 
            : this(new Vector2(x,y), new SizeD(width,height))
        {
        }

        public RectangleAA2(Vector2 location, SizeD size)
        {
            _location = location;
            _size = size;
        }


        public RectangleAA2(AARectangle prototype)
            : this(prototype.Location, prototype.Size)
        {
        }

        public RectangleAA2(RectangleAA2 prototype)
        {
            Prototype(prototype);
        }

        #endregion

        #region Properties

        public double X
        {
            get { return _location.X; }
        }

        public double Y
        {
            get { return _location.Y; }
        }

        public double Width
        {
            get { return _size.Width; }
            set { Size = new SizeD(value, _size.Height); }
        }

        public double Height
        {
            get { return _size.Height; }
            set { Size = new SizeD(_size.Width, value); }
        }


        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public SizeD Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Vector2 MiddlePoint
        {
            get { return new Vector2(
                    X + Width/2.0,
                    Y + Height/2.0); }
            set
            {
                if (Size.IsEmpty)
                {
                    Location = value;
                }
                else
                {
                    var delta = new Vector2(MiddlePoint, value); // Old middlepoint to the new
                    Location += delta;  // apply this movement to the location
                }
            }
        }

        public AARectangle BoundingBox
        {
            get { return new AARectangle(Location, Size); }
        }

        public Circle2 BoundingCircle
        {
            get
            {
                var mp = MiddlePoint;
                return new Circle2(mp, Line2.CalcLenght(mp, Location));
            }
        }

        public double Area
        {
            get { return Width * Height; }
        }

        public bool IsEmpty
        {
            get { return _location.IsEmpty && _size.IsEmpty; }
        }

        #endregion

        #region Public methods

        public IGeometry Clone()
        {
            return new RectangleAA2(this);
        }

        public void Prototype(IGeometry prototype)
        {
            var other = prototype as RectangleAA2;
            if (other != null)
            {
                this.Location = other.Location;
                this.Size = other.Size;
            }
            else
            {
                throw new NotSupportedException("Can not prototype this Rectancle by the given Geometry: " + prototype.GetType());
            }

        }

        public void Translate(Vector2 mov)
        {
            Location += mov;
        }

        public void Scale(double factor)
        {
            var mid = MiddlePoint;
            Size = new SizeD(Size.Width * factor, Size.Height * factor);
            MiddlePoint = mid;
        }

        public Vertices ToVertices()
        {
            return BoundingBox.ToVertices();
        }

        public Polygon2 ToPolygon2()
        {
            return new Polygon2(ToVertices());
        }

        #endregion

        #region Collision

        public bool Contains(Vector2 point, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return X <= point.X + tolerance &&
                   point.X < (X + Width - tolerance) &&
                   Y <= point.Y+tolerance &&
                   point.Y < Y + Height + tolerance;
        }

        public bool IntersectsWith(IGeometry other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return ToPolygon2().IntersectsWith(other, tolerance);
        }

        public IEnumerable<Vector2> Intersect(IGeometry other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return ToPolygon2().Intersect(other, tolerance);
        }

        #endregion

    }
}
