using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Units;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Represents an rectangle in 2D space, which can be arbitary rotated.
    /// </summary>
    public class Rectangle2 : IShape
    {
        #region Fields

        //Internal Data which defines a rotatable rect

        Vector2 _middlePoint;
        double _width;
        double _height;
        Angle _rotateAngle = Units.Angle.Zero; // Rotation is considered centric

        #endregion

        #region Public Propertys

        public Angle Angle
        {
            get { return _rotateAngle; }
            set { _rotateAngle = value; }
        }

        public bool IsRotated {
            get { return (this.Angle != Units.Angle.Zero); }
        }

        public double Height
        {
            get {
                return _height;
            }
            set {
                _height = value;
            }
        }

        public double Width
        {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }


        public SizeD Size {
            get {
                return new SizeD(_width, _height);
            }
            set {
                _width = value.Width;
                _height = value.Height;
            }
        }

        /// <summary>
        /// Area of this Rectangle
        /// </summary>
        public double Area
        {
            get { return Height * Width; }
        }

        #endregion

        #region Constructor's

        /// <summary>
        /// Creates an empty rectangle
        /// </summary>
        public Rectangle2() { }

        /// <summary>
        /// Creates a rectangle with 4 edge points.
        /// </summary>
        /// <param name="vertices"></param>
        public Rectangle2(Vector2[] vertices) {

            if (vertices.Count() != 4)
                throw new ArgumentException("You must submit 4 vertices!");

            //Calc width and height Vectors
            var vW = new Vector2(vertices[0], vertices[1]);
            var vH = new Vector2(vertices[1], vertices[2]);

            //Use Vector Geometry to walk to the middlepoint
            _middlePoint = vertices[0] + ((vW / 2) + (vH / 2));

            _rotateAngle = vW.AngleSignedTo(Vector2.UnitX, true);
            _width = vW.Length;
            _height = vH.Length;
        }

        public Rectangle2(double x, double y, double uwidth, double uheight)
            : this(x, y, uwidth, uheight, Angle.Zero)
        {
        }


        public Rectangle2(double x, double y, double uwidth, double uheight, Angle angle)
        {
            _width = uwidth;
            _height = uheight;
            this.Location = new Vector2(x, y);
            this.Angle = angle;
        }

        public Rectangle2(Vector2 uLocation, SizeD uSize)
            : this(uLocation, uSize, Angle.Zero)
        {
        }

        public Rectangle2(Vector2 uLocation, SizeD uSize, Angle angle)
        {
            this.Size = uSize;
            this.Location = uLocation;
            this.Angle = angle;
        }

        public Rectangle2(AARectangle rect)
            : this(rect, Angle.Zero)
        {
        }

        public Rectangle2(AARectangle rect, Angle angle)
        {
            this.Size = rect.Size;
            this.Location = rect.Location;
            this.Angle = angle;
        }

        public Rectangle2(Rectangle2 prototype)
        {
            Prototype(prototype);
        }

        public Rectangle2(Rectangle2 prototype, Angle angle)
        {
            Prototype(prototype);
            Angle = angle;
        }

        /// <summary>
        /// Prototype methods, which copies al values from the prototype to this instance.
        /// </summary>
        /// <param name="iprototype"></param>
        public void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as Rectangle2;
            if (prototype == null)
                throw new NotSupportedException();

            this.Size = prototype.Size;
            this.MiddlePoint = prototype.MiddlePoint;
            this.Angle = prototype.Angle;
            this._pen = prototype.Pen;
            this.FillBrush = prototype.FillBrush;
        }

        #endregion

        #region Public Misc Methods

        /// <summary>
        /// Gets the Vertices (Points) of this Rectangle
        /// </summary>
        /// <returns></returns>
        public Vertices ToVertices() {

            var location = new Vector2(MiddlePoint.X - Width / 2, MiddlePoint.Y - Height / 2);

            var vertices = new Vertices()
            {
                location,
                new Vector2(location.X + Width, location.Y),
                new Vector2(location.X + Width, location.Y + Height),
                new Vector2(location.X, location.Y + Height)
            };

            return  vertices.RotateVertices(this.MiddlePoint, Angle);
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(this.ToVertices())
            {
                Pen = this.Pen,
                FillBrush = this.FillBrush
            };
        }

        public AARectangle ToRectangleF(bool forceConversation = false)
        {
            if (this.Angle != Angle.Zero || forceConversation)
                throw new NotSupportedException("Can not transform rotated Rectangle2 to RectangleF!");
            return new AARectangle(this.Location, this.Size);
        }

        /// <summary>
        /// Creates the 4 Lines which encloses this Rectangle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Line2> ToLines() {

            var vertices = this.ToVertices().ToArray();

            if (vertices.Count() != 4)
                return new Line2[0]; 

            var lines = new[] 
            {
                new Line2(vertices[0], vertices[1]),
                new Line2(vertices[1], vertices[2]),
                new Line2(vertices[2], vertices[3]),
                new Line2(vertices[3], vertices[0])
            };

            return lines;
        }

        #endregion

        #region IGeometryBase

        /// <summary>
        /// Gets or sets the Location of the upper Left Corner of this Rectangle
        /// </summary>
        public Vector2 Location {
            get {
                var upperleftCorner = new Vector2(MiddlePoint.X - Width / 2, MiddlePoint.Y - Height / 2);
                if (this.IsRotated) { //optimisation - do only if we have a rotated rect
                    var vToUpperLeft = new Vector2(MiddlePoint, upperleftCorner);
                    upperleftCorner = MiddlePoint + vToUpperLeft.GetRotated(Angle);
                }
                return upperleftCorner;
            }
            set {
                Translate(new Vector2(Location, value));
            }
        }

        /// <summary>
        /// Translate this Rectangle along the given Vector
        /// </summary>
        /// <param name="mov"></param>
        public void Translate(Vector2 mov) {
            MiddlePoint += mov;
        }

        public void Scale(double fact)
        {
            throw new NotImplementedException();
        }

        public Vector2 MiddlePoint {
            get { return this._middlePoint; }
            set { this._middlePoint = value; }
        }

        public IGeometryBase Clone() {
            return new Rectangle2(this);
        }


        #endregion

        public override string ToString() {
            return string.Format(@"[{0}/{1}] {2}°", _width, _height, _rotateAngle); //rotation is considered centric
        }

        #region IGeometryBase Collision Detection

        public bool Contains(Vector2 point, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            if (this.IsRotated)
                return this.ToPolygon2().Contains(point, tolerance);
            else //optimisation - use simple rect if no rotation is given
                return this.ToRectangleF().Contains(point); // TODO Handle Tolerance!
        }

        public Circle2 BoundingCircle {
            get {
                return this.ToPolygon2().BoundingCircle;
            }
        }

        public AARectangle BoundingBox {
            get { return this.ToPolygon2().BoundingBox; }
        }

        public bool IntersectsWith(IGeometryBase other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            foreach (var line in this.ToLines()) {
                if (other.IntersectsWith(line, tolerance))
                    return true;
            }
            return false;
        }
        public IEnumerable<Vector2> Intersect(IGeometryBase other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var intersections = new List<Vector2>();

            foreach (var line in this.ToLines())
                intersections.AddRange(other.Intersect(line, tolerance));

            return intersections;
        }


        #endregion

        #region Drawing

        Pen _pen = null;
        Brush _brush = null;

        public virtual void Dispose()
        {
            Pen.Dispose();
            FillBrush.Dispose();
        }

        public Brush FillBrush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        public Pen Pen
        {
            get { return _pen; }
            set { _pen = value; }
        }

        public virtual void Draw(Graphics g)
        {
            this.ToPolygon2().Draw(g);
        } 
        #endregion
    }
}
