using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Archimedes.Geometry.Builder;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Primitives.Bounds;


namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Represents a closed 2D Area.
    /// </summary>
    public class Polygon2 : IGeometryBase, IClosedGeometry
    {
        #region Fields

        protected Vertices _vertices = new Vertices();
        private bool _boundCircleChanged = false;

        private Vector2 _middlePoint;
        private Circle2 _boundingCircle;

        protected Pen _pen = null;
        protected Brush _fillBrush = null;

        PolygonBoundingBoxAlgorythm _smallestBoundingBoxFinder;
        PolygonBoundingBoxAlgorythm _smallestWidthBoundingBoxFinder;

        #endregion

        #region Constructor

        public Polygon2() { }

        public Polygon2(IEnumerable<Vector2> vertices) {
            this.AddRange(vertices);
        }

        public Polygon2(Polygon2 prototype) {
            this.Prototype(prototype);
        }

        #endregion

        #region Public Properties

        public Brush FillBrush {
            set { _fillBrush = value; }
            get { return _fillBrush; }
        }

        public Pen Pen {
            set { _pen = value; }
            get { return _pen; }
        }

        #endregion

        #region Public Vertex Access Methods

        public void Clear() {
            _vertices.Clear();
            _boundCircleChanged = true;
        }

        public void Add(Vector2 uvertex) {
            _vertices.Add(uvertex);
            _boundCircleChanged = true;
        }
        public void Remove(Vector2 uvertex) {
            _vertices.Remove(uvertex);
            _boundCircleChanged = true;
        }
        public void AddRange(IEnumerable<Vector2> uvertices) {
            _vertices.AddRange(uvertices);
            _boundCircleChanged = true;
        }

        public void CleanUp() {
            _vertices = new Vertices(_vertices.Distinct());
        }

        #endregion

        #region Prototype Methods

        public void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as Polygon2;
            if (prototype == null)
                throw new NotSupportedException();

            this.AddRange(prototype.ToVertices());
            this.Pen = prototype.Pen;
            this.FillBrush = prototype.FillBrush;
        }

        #endregion

        #region Public Methods

        #region Orientation Routines

        /// <summary>
        /// Return True if the polygon is oriented clockwise.
        /// </summary>
        /// <returns></returns>
        public bool IsOrientedClockwise() {
            return (SignedPolygonArea() < 0);
        }


        /// <summary>
        /// If the polygon is oriented counterclockwise, reverse the order of its points.
        /// </summary>
        public void OrientClockwise() {
            if (!IsOrientedClockwise())
                _vertices = new Vertices(_vertices.Reverse());
        }

        /// <summary>
        /// If the polygon is oriented clockwise, reverse the order of its points.
        /// </summary>
        public void OrientCounterClockwise() {
            if (IsOrientedClockwise())
                _vertices = new Vertices(_vertices.Reverse());
        }

        #endregion // Orientation Routines

        #region Area Routines

        /// <summary>
        /// Return the polygon's area in "square units."
        /// Add the areas of the trapezoids defined by the
        /// polygon's edges dropped to the X-axis. When the
        /// program considers a bottom edge of a polygon, the
        /// calculation gives a negative area so the space
        /// between the polygon and the axis is subtracted,
        /// leaving the polygon's area. This method gives odd
        /// results for non-simple polygons.
        /// </summary>
        /// <returns></returns>
        public float Area {
            get {
                // Return the absolute value of the signed area.
                // The signed area is negative if the polyogn is
                // oriented clockwise.
                return Math.Abs(SignedPolygonArea());
            }
        }


        /// <summary>
        /// Return the polygon's area in "square units."
        /// Add the areas of the trapezoids defined by the
        /// polygon's edges dropped to the X-axis. When the
        /// program considers a bottom edge of a polygon, the
        /// calculation gives a negative area so the space
        /// between the polygon and the axis is subtracted,
        /// leaving the polygon's area. This method gives odd
        /// results for non-simple polygons.
        ///
        /// The value will be negative if the polyogn is
        /// oriented clockwise.
        /// </summary>
        /// <returns></returns>
        private float SignedPolygonArea() {
            // Add the first point to the end.
            int num_points = _vertices.Count;
            if (num_points == 0)
                return 0;
            var pts = new Vector2[num_points + 1];
            _vertices.CopyTo(pts, 0);
            pts[num_points] = _vertices[0];

            // Get the areas.
            float area = 0;
            for (int i = 0; i < num_points; i++) {
                area +=
                    (pts[i + 1].X - pts[i].X) *
                    (pts[i + 1].Y + pts[i].Y) / 2;
            }

            // Return the result.
            return area;
        }
        #endregion // Area Routines

        #region Convex Methods

        /// <summary>
        /// Determites if this polygon is convex.
        /// </summary>
        /// <returns>Return True if the polygon is convex.</returns>
        public bool IsConvex() {
            CleanUp();
            // For each set of three adjacent points A, B, C,
            // find the dot product AB · BC. If the sign of
            // all the dot products is the same, the angles
            // are all positive or negative (depending on the
            // order in which we visit them) so the polygon
            // is convex.
            bool got_negative = false;
            bool got_positive = false;
            int num_points = _vertices.Count;
            int B, C;
            for (int A = 0; A < num_points; A++) {
                B = (A + 1) % num_points;
                C = (B + 1) % num_points;

                float cross_product =
                    CrossProductLength(
                        _vertices[A].X, _vertices[A].Y,
                        _vertices[B].X, _vertices[B].Y,
                        _vertices[C].X, _vertices[C].Y);
                if (cross_product < 0) {
                    got_negative = true;
                } else if (cross_product > 0) {
                    got_positive = true;
                }
                if (got_negative && got_positive) return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region Collision Detection Methods

        /// <summary>
        /// Does this polygon contain the given point?
        /// </summary>
        /// <param name="p">The Point to check</param>
        /// <returns>True if the Point is contained in the Polygon</returns>
        public bool Contains(Vector2 p) {
            int counter = 0;
            int i;
            double xinters;
            Vector2 p1, p2;
            int N = _vertices.Count;

            if (!this.BoundingCircle.Contains(p)) {
                return false;
            }

            p1 = _vertices[0];
            for (i = 1; i <= N; i++) {
                p2 = _vertices[i % N];
                if (p.Y > Math.Min(p1.Y, p2.Y)) {
                    if (p.Y <= Math.Max(p1.Y, p2.Y)) {
                        if (p.X <= Math.Max(p1.X, p2.X)) {
                            if (p1.Y != p2.Y) {
                                xinters = (p.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                                if (p1.X == p2.X || p.X <= xinters)
                                    counter++;
                            }
                        }
                    }
                }
                p1 = p2;
            }
            return (counter % 2 != 0);
        }

        /// <summary>
        /// Is the given Polygon fully contained in this one?
        /// </summary>
        /// <param name="poly"></param>
        /// <returns></returns>
        public bool Contains(Polygon2 poly) {
            foreach (var vertex in poly.ToVertices()) {
                if (!this.Contains(vertex))
                    return false;
            }
            return true;
        }

        public bool IntersectsWith(IGeometryBase other) {
            IEnumerable<Vector2> collisionPoints;
            return InterSectsOther(other, false, out collisionPoints);
        }

        public IEnumerable<Vector2> Intersect(IGeometryBase other) {
            IEnumerable<Vector2> collisionPoints;
            InterSectsOther(other, true, out collisionPoints);
            return collisionPoints;
        }


        private bool InterSectsOther(IGeometryBase other, bool collectAllPoints, out IEnumerable<Vector2> _collisionPoints) {
            var collisionPoints = new List<Vector2>();
            _collisionPoints = collisionPoints;

            if (other is IClosedGeometry) { /* if other is not a closed element, it must have one vertex in this polygon */
                foreach (var vertex in this.ToVertices()) {
                    if (other.Contains(vertex)) {
                        collisionPoints.Add(vertex);
                        if (!collectAllPoints) return true;
                    }
                }
            }
            foreach (var vertex in other.ToVertices()) {
                if (this.Contains(vertex)) {
                    collisionPoints.Add(vertex);
                    if (!collectAllPoints) return true;
                }
            }
            return (collisionPoints.Count() != 0);
        }

        public Circle2 BoundingCircle {
            get {
                if(_boundCircleChanged) {
                    double dist;
                    var middlePoint = this.MiddlePoint;
                    double longestDist = 0;

                    foreach(var vertex in this._vertices) {
                        dist = Line2.CalcLenght(middlePoint, vertex);
                        if (longestDist < dist)
                            longestDist = dist;
                    }
                    _boundingCircle = new Circle2(middlePoint, (float)longestDist);
                }
                return _boundingCircle;
            }
        }

        /// <summary>
        /// Returns the Boundingbox which Aera is the smallest
        /// </summary>
        public Rectangle2 FindSmallestBoundingBox() {
            return FindBoundingBox(SmallestBoundingBoxFinderAlgorythm);
        }

        /// <summary>
        /// Returns the Boundingbox which one side (width) is the smallest possible
        /// </summary>
        public Rectangle2 FindSmallestWidthBoundingBox() {
            return FindBoundingBox(SmallestWidthBoundingBoxFinder);
        }

        /// <summary>
        /// Find the Boundingbox with the given Algorythm
        /// </summary>
        /// <param name="boxfindingAlgorythm">Concrete implementation of the bounding finder Algorythm to use</param>
        /// <returns></returns>
        public Rectangle2 FindBoundingBox(PolygonBoundingBoxAlgorythm boxfindingAlgorythm) {
            Rectangle2 rect = new Rectangle2();
            if (this.IsConvex()) {
                boxfindingAlgorythm.SetPolygon(this);
                var vertices = boxfindingAlgorythm.FindBounds();
                if (vertices.Count() == 4)
                    rect = new Rectangle2(vertices);
                if (rect.Size.IsEmpty) // smallest boundingboxfinder failed - use simple boundingbox instead
                    rect = this.BoundingBox.ToGeometry();
            } else
                rect = this.ToConvexPolygon().FindBoundingBox(boxfindingAlgorythm);

            return rect;
        }


        #endregion //End Collision Detection

        #region Centroid/Middlepoint
        /// <summary>
        /// Get the middlepoint of this polygon
        /// </summary>
        /// <returns></returns>
        public Vector2 MiddlePoint {
            get {
                if(_boundCircleChanged) {
                    _middlePoint = FindCentroid();
                }
                return _middlePoint;
            }
            set {
                var move = new Vector2(this.MiddlePoint, value);
                this.Move(move);
            }
        }

        /// <summary>
        /// Find the polygon's centroid.
        /// </summary>
        /// <returns></returns>
        private Vector2 FindCentroid() {

            if (_vertices.Count == 0)
                return new Vector2(); // in case there are no vertices

            // Add the first point at the end of the array.
            int num_points = _vertices.Count;
            var pts = new Vector2[num_points + 1];
            _vertices.CopyTo(pts, 0);
            pts[num_points] = _vertices[0];

            // Find the centroid.
            float x = 0;
            float y = 0;
            float second_factor;
            for (int i = 0; i < num_points; i++) {
                second_factor =
                    pts[i].X * pts[i + 1].Y -
                    pts[i + 1].X * pts[i].Y;
                x += (pts[i].X + pts[i + 1].X) * second_factor;
                y += (pts[i].Y + pts[i + 1].Y) * second_factor;
            }

            // Divide by 6 times the polygon's area.
            float polygon_area = this.Area;
            x /= (6 * polygon_area);
            y /= (6 * polygon_area);

            // If the values are negative, the polygon is
            // oriented counterclockwise so reverse the signs.
            if (x < 0) {
                x = -x;
                y = -y;
            }
            return new Vector2(x, y);
        }

        #endregion

        #region Public Transformator Methods

        /// <summary>
        /// Rotates the Polygon around the origin. If no Origin is specified, the middlepoint is taken by default.
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <param name="origin">Point to rotate around. Default is middlepoint of this polygon</param>
        public void Rotate(float angle, Vector2? origin = null) {
            _vertices = _vertices.RotateVertices(origin ?? this.MiddlePoint, angle);
            Invalidate();
        }


        /// <summary>
        /// Moves the Polygon (each vertex) along the given Vector
        /// </summary>
        /// <param name="v"></param>
        public void Move(Vector2 v) {
            for(int i=0; i < _vertices.Count; i++) {
                _vertices[i] = _vertices[i] + v;
            }
            Invalidate();
        }

        public void Scale(float fact) {
            for (int i = 0; i < _vertices.Count; i++) {
                _vertices[i] = _vertices[i].Scale(fact);
            }
            Invalidate();
        }

        #endregion

        #region To Methods

        /// <summary>
        /// Returns this Polygon as convex polygon. If this polygon is convex, a clone of it is returned.
        /// If this polygon is concave, the Graham's (1972) "points to convex hull" enhanched algorythm is used to generate a convex polygon hull.
        /// </summary>
        /// <returns></returns>
        public Polygon2 ToConvexPolygon() {
            var clone = this.Clone() as Polygon2;
            clone.Clear();
            var convexhull = ConvexHullBuilder.Convexhull(this.ToVertices());
            clone.AddRange(convexhull.ToVertices());
            return clone;
        }

        public Vertices ToVertices() {
            return new Vertices(_vertices.Distinct());
        }

        public Polygon2 ToPolygon2() {
            return this;
        }

        #endregion

        #region IDrawable

        public void Draw(Graphics G) {
            try {
                var path = new GraphicsPath();
                path.AddPolygon(_vertices.ToPointArray());
                if (this.FillBrush != null)
                    G.FillPath(this.FillBrush, path);
                G.DrawPath(_pen, path);
            } catch (Exception) {
                //ignore
            }
        }

        #endregion

        #region IGeometryBase

        public void AddToPath(GraphicsPath path) {
            path.AddPolygon(_vertices.ToPointArray());
        }

        public Vector2 Location {
            get { return this.MiddlePoint; }
            set { this.MiddlePoint = value; }
        }


        public IGeometryBase Clone() {
            return new Polygon2(this);
        }


        public RectangleF BoundingBox {
            get { return _vertices.BoundingBox; }
        }

        #endregion

        #region Cross and Dot Products
        // Return the cross product AB x BC.
        // The cross product is a vector perpendicular to AB
        // and BC having length |AB| * |BC| * Sin(theta) and
        // with direction given by the right-hand rule.
        // For two vectors in the X-Y plane, the result is a
        // vector with X and Y components 0 so the Z component
        // gives the vector's length and direction.
        public static float CrossProductLength(float Ax, float Ay,
            float Bx, float By, float Cx, float Cy) {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }

        // Return the dot product AB · BC.
        // Note that AB · BC = |AB| * |BC| * Cos(theta).
        private static float DotProduct(float Ax, float Ay,
            float Bx, float By, float Cx, float Cy) {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Calculate the dot product.
            return (BAx * BCx + BAy * BCy);
        }
        #endregion // Cross and Dot Products

        PolygonBoundingBoxAlgorythm SmallestBoundingBoxFinderAlgorythm {
            get {
                if (_smallestBoundingBoxFinder == null) {
                    _smallestBoundingBoxFinder = new PolygonSmallestBoundingBoxAlgorythm();
                }
                return _smallestBoundingBoxFinder;
            }
        }

        PolygonBoundingBoxAlgorythm SmallestWidthBoundingBoxFinder {
            get {
                if (_smallestWidthBoundingBoxFinder == null) {
                    _smallestWidthBoundingBoxFinder = new PolygonSmallestWidthBoundingBoxAlgorythm();
                }
                return _smallestWidthBoundingBoxFinder;
            }
        }

        public virtual void Dispose() {
            //Pen.Dispose();
            //FillBrush.Dispose();
        }

        private void Invalidate() {
            _boundCircleChanged = true;
        }
    }
}
