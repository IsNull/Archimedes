using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;

namespace Archimedes.Geometry
{
    /// <summary>
    /// A Ray is a one-direction infinite Line.
    /// </summary>
    public class Ray : IDrawable
    {
        #region Fields

        Vector2 _location;
        Pen _pen = null;
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

        /// <summary>
        /// Pen used to draw this Ray
        /// </summary>
        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Test if a Point lies on the Ray
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Contains(Vector2 target) {
            var v = new Vector2(this.Location, target);
            return (Vector.IsDirectionEqual(v));
        }
        
        public Ray Clone() {
            return new Ray(this);
        }
        #endregion

        #region Intersection Ray - Ray

        public Vector2 Intersect(Ray uRay) {
            var point = IntersectN(uRay);
            if (point.HasValue)
                return point.Value;
            else
                return new Vector2();
        }

        public bool IntersectWith(Ray uRay) {
            var point = IntersectN(uRay);
            return point.HasValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uRay"></param>
        /// <returns></returns>
        Vector2? IntersectN(Ray uRay) {
           

            if (Vector.IsParallel(uRay.Vector)) {
                return null;
            }

            double intersectionpntX = 0;
            double intersectionpntY = 0;

            if (!Vector.IsVertical && !uRay.Vector.IsVertical) {    // both NOT vertical
                intersectionpntX = (float)((-1 * (this.YDist - uRay.YDist)) / (Vector.Slope - uRay.Vector.Slope));
                intersectionpntY = (float)(Vector.Slope * intersectionpntX + this.YDist);
            } else if (Vector.IsVertical) {                  // this vertical (so it must lie on this.X)
                intersectionpntX = Vector.X;
                intersectionpntY = (float)(uRay.Vector.Slope * intersectionpntX + uRay.YDist);
            } else if (uRay.Vector.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                intersectionpntX = uRay.Vector.X;
                intersectionpntY = (float)(Vector.Slope * intersectionpntX + this.YDist);
            }

            var intersectionpnt = new Vector2(intersectionpntX, intersectionpntY);

            //check if computed intercept lies on our line.
            if (this.Contains(intersectionpnt) || uRay.Contains(intersectionpnt)) {
                return intersectionpnt;
            }
            return null;
        }
        #endregion

        #region Drawing Methods

        public void Draw(Graphics g) {
            if (this.Pen != null) {
                var v = Vector;
                v.Length = 500;
                var end = this.Location + v;
                g.DrawLine(this.Pen, this.Location, end);
            }
        }

        #endregion
    }
}
