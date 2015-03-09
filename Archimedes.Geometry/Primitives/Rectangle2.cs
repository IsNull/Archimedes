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
    /// // TODO Creating a rectangle with location + angle is currently broken!
    /// </summary>
    public class Rectangle2 : IShape
    {
        #region Fields

        //Internal Data which defines a rotatable rect

        private Vector2 _location;
        private double _width;
        private double _height;
        private Angle _rotation = Units.Angle.Zero; // Rotation is considered centric

        #endregion

        #region Static Builder methods

        /// <summary>
        /// Returns a new, empty rectangle
        /// </summary>
        public static Rectangle2 Empty
        {
            get { return new Rectangle2(0, 0, 0, 0, Angle.Zero); }
        }

        /// <summary>
        /// Constructs a rectangle from 4 points in 2D space.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static Rectangle2 FromVertices(Vector2[] vertices)
        {
            if (vertices.Count() != 4)
                throw new ArgumentException("You must submit 4 vertices!");

            // Calc width and height Vectors
            var vWidth = new Vector2(vertices[0], vertices[1]);
            var vHeight = new Vector2(vertices[1], vertices[2]);

            // Use Vector Geometry to walk to the middlepoint
            var middlePoint = vertices[0] + ((vWidth / 2.0) + (vHeight / 2.0));

            var rotation = vWidth.AngleSignedTo(Vector2.UnitX, true);
            var width = vWidth.Length;
            var height = vHeight.Length;

            return new Rectangle2(0, 0, width, height, rotation)
            {
                MiddlePoint = middlePoint
            };
        }

        #endregion

        #region Constructor's


        /// <summary>
        /// Creates a new Rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="uwidth"></param>
        /// <param name="uheight"></param>
        /// <param name="rotation"></param>
        public Rectangle2(double x, double y, double uwidth, double uheight, Angle? rotation = null)
        {
            _width = uwidth;
            _height = uheight;
            this.Location = new Vector2(x, y);
            this.Rotation = rotation ?? Angle.Zero;
        }

        public Rectangle2(Vector2 location, SizeD size, Angle? rotation = null)
            : this(location.X, location.Y, size.Width, size.Height,  rotation ?? Angle.Zero)
        {
        }

        public Rectangle2(AARectangle rect, Angle? rotation = null)
            : this(rect.Location, rect.Size, rotation ?? Angle.Zero) { }

        public Rectangle2(Rectangle2 prototype)
        {
            Prototype(prototype);
        }

        /// <summary>
        /// Prototype methods, which copies al values from the prototype to this instance.
        /// </summary>
        /// <param name="iprototype"></param>
        public void Prototype(IGeometryBase iprototype)
        {
            var prototype = iprototype as Rectangle2;
            if (prototype == null)
                throw new NotSupportedException();

            this.Size = prototype.Size;
            this.MiddlePoint = prototype.MiddlePoint;
            this.Rotation = prototype.Rotation;
            this._pen = prototype.Pen;
            this.FillBrush = prototype.FillBrush;
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// Gets / Sets the rotation of this rectangle
        /// </summary>
        public Angle Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public bool IsRotated {
            get { return (this.Rotation != Units.Angle.Zero); }
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

        /// <summary>
        /// Gets or sets the Location of the upper Left Corner of this Rectangle
        /// </summary>
        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public Vector2 MiddlePoint
        {
            get { return CalcMiddlepoint(Location, Size, Rotation); }
            set
            {
                var move = new Vector2(MiddlePoint, value);
                Location = Location + move;
            }
        }

        #endregion

        /// <summary>
        /// Calculates the top left corner of this rectangle based on the middlepoint.
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="size"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private static Vector2 CalcTopLeftCorner(Vector2 middlePoint, SizeD size, Angle rotation)
        {
            var upperleftCorner = new Vector2(
                    middlePoint.X - (size.Width / 2.0),
                    middlePoint.Y - (size.Height / 2.0)
                    );
            if (rotation != Angle.Zero)
            { // If we have a rotated rect we need to apply this
                var vToUpperLeft = new Vector2(middlePoint, upperleftCorner);
                upperleftCorner = middlePoint + vToUpperLeft.GetRotated(rotation);
            }
            return upperleftCorner;
        }

        /// <summary>
        /// Calculates the top left corner of this rectangle based on the middlepoint.
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="size"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private static Vector2 CalcMiddlepoint(Vector2 topLeft, SizeD size, Angle rotation)
        {
            var middle = new Vector2(
                    topLeft.X + (size.Width / 2.0),
                    topLeft.Y + (size.Height / 2.0)
                    );
            if (rotation != Angle.Zero)
            { // If we have a rotated rect we need to apply this
                var vector = new Vector2(topLeft, middle);
                middle = topLeft + vector.GetRotated(rotation);
            }
            return middle;
        }
 

        #region Public Misc Methods

        /// <summary>
        /// Gets the Vertices (Points) of this Rectangle
        /// </summary>
        /// <returns></returns>
        public Vertices ToVertices() {

            var location = Location;

            var vertices = new Vertices()
            {
                location,
                new Vector2(location.X + Width, location.Y),
                new Vector2(location.X + Width, location.Y + Height),
                new Vector2(location.X, location.Y + Height)
            };

            return vertices.RotateVertices(location, Rotation);
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
            if (this.Rotation != Angle.Zero || forceConversation)
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



        public IGeometryBase Clone() {
            return new Rectangle2(this);
        }


        #endregion

        public override string ToString() {
            return string.Format(@"[{0}/{1}] {2}°", Width, Height, Rotation); //rotation is considered centric
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
