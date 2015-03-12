namespace Archimedes.Geometry
{
    /// <summary>
    /// A Ray is a one-direction infinite Line.
    /// </summary>
    public class Ray
    {
        #region Fields

        Vector2 _location;
        Vector2 _direction;

        #endregion

        #region Constructors

        public Ray(Vector2 v, Vector2 startpnt)
            : this(v.X, v.Y, startpnt) { }

        public Ray(Ray prototype)
            : this(prototype.Vector, prototype.Location) { }

        public Ray(double x, double y, Vector2 p) {
            _direction = new Vector2(x, y);
            this.Location = p;
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Startpoint of this Ray
        /// </summary>
        public Vector2 Location {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// Vector of this Ray
        /// </summary>
        public Vector2 Vector {
            get { return _direction; }
        }

        public double YDist {
            get {   // q = y1 - m * x1 
                if (Vector.IsVertical) {
                    return 0;
                } else {
                    return this.Location.Y - (Vector.Slope * this.Location.X);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Test if a Point lies on the Ray
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Contains(Vector2 target, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var v = new Vector2(Location, target);
            return (Vector.IsDirectionEqual(v, tolerance));
        }
        
        public Ray Clone() {
            return new Ray(this);
        }
        #endregion

        #region Intersection Ray - Ray

        public Vector2 Intersect(Ray uRay, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var point = IntersectN(uRay, tolerance);
            if (point.HasValue)
                return point.Value;
            else
                return Vector2.Zero;
        }

        public bool IntersectWith(Ray uRay, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var point = IntersectN(uRay, tolerance);
            return point.HasValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uRay"></param>
        /// <returns></returns>
        private Vector2? IntersectN(Ray uRay, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {

            if (Vector.IsParallelTo(uRay.Vector, tolerance))
            {
                return null;
            }

            double intersectionpntX = 0;
            double intersectionpntY = 0;

            if (!Vector.IsVertical && !uRay.Vector.IsVertical) {    // both NOT vertical
                intersectionpntX = ((-1.0 * (this.YDist - uRay.YDist)) / (Vector.Slope - uRay.Vector.Slope));
                intersectionpntY = (Vector.Slope * intersectionpntX + this.YDist);
            } else if (Vector.IsVertical) {                  // this vertical (so it must lie on this.X)
                intersectionpntX = Vector.X;
                intersectionpntY = (uRay.Vector.Slope * intersectionpntX + uRay.YDist);
            } else if (uRay.Vector.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                intersectionpntX = uRay.Vector.X;
                intersectionpntY = (Vector.Slope * intersectionpntX + this.YDist);
            }

            var intersectionpnt = new Vector2(intersectionpntX, intersectionpntY);

            //check if computed intercept lies on our line.
            if (Contains(intersectionpnt, tolerance) || uRay.Contains(intersectionpnt, tolerance))
            {
                return intersectionpnt;
            }
            return null;
        }
        #endregion


    }
}
