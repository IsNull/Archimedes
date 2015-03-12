using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Archimedes.Geometry.Units;

namespace Archimedes.Geometry.Primitives
{
    public class Rectangle2 : IShape
    {
         #region Fields

        // Internal we hold the data in a polygon
        private readonly Polygon2 _rect;

        #endregion

        #region Static Builder methods

        /// <summary>
        /// Returns a new, empty rectangle
        /// </summary>
        public static Rectangle2 Empty
        {
            get { return new Rectangle2(new []
            {
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero
            }); }
        }

        /// <summary>
        /// Constructs a rectangle from 4 points in 2D space.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static Rectangle2 FromVertices(Vector2[] vertices)
        {
            return new Rectangle2(vertices);
        }

        #endregion

        #region Constructor's

        public Rectangle2(Vector2 location, SizeD size, Angle? rotation = null)
            : this(location.X, location.Y, size.Width, size.Height, rotation ?? Angle.Zero)
        {
        }

        public Rectangle2(AARectangle rect, Angle? rotation = null)
            : this(rect.Location, rect.Size, rotation ?? Angle.Zero) { }


        /// <summary>
        /// Creates a new rectangle from 4 points
        /// </summary>
        /// <param name="vertices"></param>
        public Rectangle2(Vector2[] vertices)
        {
            if (vertices == null) throw new ArgumentNullException("vertices");
            if (vertices.Count() != 4) throw new ArgumentException("You must submit 4 vertices!");
            
            // TODO Check vertices that they are a rectangle!
            

            _rect = new Polygon2(vertices);
        }

        /// <summary>
        /// Creates a new Rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rotation"></param>
        public Rectangle2(double x, double y, double width, double height, Angle? rotation = null)
        {
            var topLeft = new Vector2(x, y);
            var topRight = new Vector2(x + width, y);
            var bottomRight = new Vector2(x + width, y + height);
            var bottomLeft = new Vector2(x, y + height);

            _rect = new Polygon2(new[] { topLeft, topRight, bottomRight, bottomLeft });

            // Rotate if necessary
            if (rotation.HasValue && rotation.Value != Angle.Zero)
            {
                _rect.Rotate(rotation.Value, topLeft);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prototype"></param>
        public Rectangle2(Rectangle2 prototype)
        {
            _rect = new Polygon2();
            Prototype(prototype);
        }

        /// <summary>
        /// Prototype methods, which copies al values from the prototype to this instance.
        /// </summary>
        /// <param name="iprototype"></param>
        public void Prototype(IGeometry iprototype)
        {
            var prototype = iprototype as Rectangle2;
            if (prototype == null)
                throw new NotSupportedException("iprototype must be of type Rectangle2!");

            _rect.Clear();
            _rect.AddRange(prototype.ToVertices());
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// Gets / Sets the rotation of this rectangle
        /// </summary>
        public Angle Rotation
        {
            get
            {
                var vertices = _rect.ToVertices();
                var vWidth = new Vector2(vertices[0], vertices[1]);
                var rotation = vWidth.AngleSignedTo(Vector2.UnitX, true);
                return rotation;
            }
        }

        public bool IsRotated {
            get { return (this.Rotation != Angle.Zero); }
        }


        public double Width
        {
            get
            {
                var sideVector = new Vector2(_rect[0], _rect[1]);
                return sideVector.Length;
            }

            set
            {
                var wVector = new Vector2(_rect[0], _rect[1]);
                if (wVector.Length == 0)
                {
                    wVector = Vector2.UnitX;
                }
                var moveVector = wVector.WithLength(value);
                _rect[1] = _rect[0] + moveVector;
                _rect[2] = _rect[3] + moveVector;
            }
        }


        public double Height
        {
            get
            {
                var sideVector = new Vector2(_rect[1], _rect[2]);
                return sideVector.Length;
            }

            set
            {
                var hVector = new Vector2(_rect[1], _rect[2]);
                if (hVector.Length == 0)
                {
                    hVector = Vector2.UnitY;
                }
                var moveVector = hVector.WithLength(value);
                _rect[2] = _rect[1] + moveVector;
                _rect[3] = _rect[0] + moveVector;
            }
        }


        /**
         * 
         */
        public SizeD Size {
            get {
                return new SizeD(Width, Height);
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
            get { 
                return _rect[0]; 
            }
            set
            {
                var current = Location;
                var delta = new Vector2(current, value);
                _rect.Translate(delta);
            }
        }

        
        public Vector2 MiddlePoint
        {
            get
            {
                return _rect.MiddlePoint;
            }

            [DebuggerStepThrough]
            set
            {
                if (Width == 0 && Height == 0)
                {
                    throw new NotSupportedException("You can not set the middlepoint of a rectangle which has no size!");
                }
                _rect.MiddlePoint = value;
            }
        }

        #endregion


        #region Public Misc Methods

        /// <summary>
        /// Gets the Vertices (Points) of this Rectangle
        /// </summary>
        /// <returns></returns>
        public Vertices ToVertices()
        {
            return _rect.ToVertices();
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(_rect);
        }

        public AARectangle ToAARectangle(bool forceConversation = false)
        {
            if (this.Rotation != Angle.Zero || forceConversation)
                throw new NotSupportedException("Can not transform rotated Rectangle2 to RectangleF!");
            return new AARectangle(this.Location, this.Size);
        }

        /// <summary>
        /// Creates the 4 Lines which encloses this Rectangle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Line2> ToLines()
        {
            return _rect.ToLines();
        }

        #endregion

        #region IGeometryBase

        /// <summary>
        /// Translate this Rectangle along the given Vector
        /// </summary>
        /// <param name="mov"></param>
        public void Translate(Vector2 mov) {
            _rect.Translate(mov);
        }

        public void Scale(double fact)
        {
            _rect.Scale(fact);
        }


        public IGeometry Clone() {
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
                return _rect.Contains(point, tolerance);
            else //optimisation - use simple rect if no rotation is given
                return this.ToAARectangle().Contains(point); // TODO Handle Tolerance!
        }

        public Circle2 BoundingCircle {
            get {
                return _rect.BoundingCircle;
            }
        }

        public AARectangle BoundingBox {
            get { return _rect.BoundingBox; }
        }

        public bool IntersectsWith(IGeometry other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return _rect.IntersectsWith(other, tolerance);
        }
        public IEnumerable<Vector2> Intersect(IGeometry other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return _rect.Intersect(other, tolerance);
        }


        #endregion


    }
}
