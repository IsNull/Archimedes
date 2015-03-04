using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry.Primitives
{
    public partial class Arc : IGeometryBase
    {

        #region Arc - Rect

        #region Rectangle2


        /// <summary>
        /// Checks if a Rectangle collides with the Arc.
        /// </summary>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>On collision, true is returned, false otherwise</returns>
        private bool InterceptRectWith(Rectangle2 rect) {

            var borderLines = rect.ToLines(); //get 4 borderlines from rect
            if (borderLines != null) {
                return InterceptLinesWith(borderLines);
            }
            return false;
        }

        /// <summary>Checks if a Rectangle collides with the Arc.
        /// 
        /// </summary>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>On collision, a List of interception Points is returned</returns>
        private List<Vector2> InterceptRect(Rectangle2 rect) {
            var intersections = new List<Vector2>();
            var borderLines = rect.ToLines(); //get 4 borderlines from rect
            if (borderLines != null) {
                intersections.AddRange(InterceptLines(borderLines));
            }
            return intersections;
        }

        #endregion

        #region Rectangle

        /// <summary>
        /// Checks if a Rectangle collides with the Arc.
        /// </summary>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>On collision, true is returned, false otherwise</returns>
        private bool InterceptRectWith(RectangleF rect) {
            
            var borderLines = Line2.RectExplode(rect); //get 4 borderlines from rect
            if (borderLines != null) {
                return InterceptLinesWith(borderLines);
            }
            return false;
        }

        /// <summary>Checks if a Rectangle collides with the Arc.
        /// 
        /// </summary>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>On collision, a List of interception Points is returned</returns>
        private List<Vector2> InterceptRect(RectangleF rect) {
            var intersections = new List<Vector2>();
            var borderLines = Line2.RectExplode(rect); //get 4 borderlines from rect
            if (borderLines != null) {
                    intersections.AddRange(InterceptLines(borderLines));
            }
            return intersections;
        }

        #endregion

        private List<Vector2> InterceptLines(IEnumerable<Line2> lines) {
            var intersections = new List<Vector2>();
            foreach (var border in lines) {
                intersections.AddRange(this.InterceptLine(border));
            }
            return intersections;
        }

        private bool InterceptLinesWith(IEnumerable<Line2> lines) {
            var intersections = new List<Vector2>();
            foreach (var border in lines) {
                if (this.InterceptLine(border).Count != 0) {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Arc - Line

        private IEnumerable<Vector2> InterceptLine(Line2 uLine, float range) {
            IEnumerable<Vector2> intersections;
            using (var strechtedLine = (uLine.Clone() as Line2)) {

                strechtedLine.Stretch(range, Direction.LEFT);
                strechtedLine.Stretch(range, Direction.RIGHT);
                intersections = InterceptLine(strechtedLine);
            }
            return intersections;
        }

        /// <summary>
        /// Bow-Line Interception
        /// </summary>
        /// <param name="uLine">Line to check</param>
        /// <returns>Returns the interception Point(s) if the Objects collide</returns>
        private List<Vector2> InterceptLine(Line2 uLine) {
            var intersections = new List<Vector2>();

            using (var clArc = (this.Clone() as Arc)) {
                // clone and round arc
                clArc.Radius = Math.Round(clArc.Radius, 2);
                clArc.Angle = Units.Angle.FromDegrees(Math.Round(clArc.Angle.Degrees, 2));
                clArc.Location = new Vector2((float)Math.Round(clArc.Location.X), (float)Math.Round(clArc.Location.Y));
                //------<

                // intercept with a circle and test inter points if they lie on our arc
                var circle = new Circle2(clArc.MiddlePoint, clArc.Radius);
                foreach (var possiblePnt in circle.Intersect(uLine)) {
                    if (clArc.Contains(possiblePnt)) {
                        intersections.Add(possiblePnt);
                    }
                }
            }
            return intersections;
        }

        /// <summary>
        /// Arc - Line Collision
        /// </summary>
        /// <param name="uLine">Line to check</param>
        /// <returns>Returns true, if the objects collide, false otherwise</returns>
        private bool InterceptLineWith(Line2 uLine) {
            return (InterceptLine(uLine).Count != 0);
        }

        #endregion

        #region Arc - Circle

        private IEnumerable<Vector2> InterceptCircle(Circle2 circle) {
            var intersections = new List<Vector2>();

            var possibles = this.ToCircle().Intersect(circle);
            foreach (var p in possibles)
                if (this.Contains(p))
                    intersections.Add(p);

            return intersections;
        }

        private bool InterceptCircleWith(Circle2 circle) {
            var possibles = this.ToCircle().Intersect(circle);
            foreach (var p in possibles)
                if (this.Contains(p))
                    return true;

            return false;
        }


        #endregion

        #region Arc - Arc

        private IEnumerable<Vector2> InterceptArc(Arc arc) {
            var intersections = new List<Vector2>();

            using(var c1 = this.ToCircle())
            using (var c2 = arc.ToCircle()) {
                var possibles = c1.Intersect(c2);

                foreach (var p in possibles)
                    if (this.Contains(p) && arc.Contains(p))
                        intersections.Add(p);
            }
            return intersections;
        }

        private bool InterceptArcWith(Arc arc) {

            using (var c1 = this.ToCircle())
            using (var c2 = arc.ToCircle()) {
                var possibles = c1.Intersect(c2);

                foreach (var p in possibles)
                    if (this.Contains(p) && arc.Contains(p))
                        return true;
            }
            return false;
        }


        #endregion

        public bool IntersectsWith(IGeometryBase other) {

            if (other is Line2) {
                return this.InterceptLineWith(other as Line2);
            } else if (other is GdiText2 || other is Rectangle2) {
                return this.InterceptRectWith(other.BoundingBox);
            } else if (other is Circle2) {
                return this.InterceptCircleWith(other as Circle2);
            } else if (other is Arc) {
                return this.InterceptArcWith(other as Arc);
            } else {
                return other.IntersectsWith(this);
            }
        }

        public IEnumerable<Vector2> Intersect(IGeometryBase other) {
            var pnts = new List<Vector2>();

            if (other is Line2) {
                pnts.AddRange(this.InterceptLine(other as Line2));
            } else if (other is GdiText2) {
                pnts.AddRange(this.InterceptRect(other.BoundingBox));
            } else if (other is Rectangle2) {
                pnts.AddRange(this.InterceptRect(other as Rectangle2));
            } else if (other is Circle2) {
                pnts.AddRange(this.InterceptCircle(other as Circle2));
            } else if (other is Arc) {
                pnts.AddRange(this.InterceptArc(other as Arc));
            } else {
                if (other is IClosedGeometry)
                    pnts.AddRange(new Polygon2(other.ToVertices()).Intersect(this));
            }
            return pnts;
        }



        /// <summary>
        /// Does a Point lie on the Arc line?
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 Point) {
            bool conatins = false;

            // first, the distance from middlepoint to our Point must be equal to the radius:
            if (Math.Round(Line2.CalcLenght(this.MiddlePoint, Point)) == Math.Round(this.Radius)) {
                using (var clArc = (this.Clone() as Arc)) {
                    // clone and round original values
                    clArc.Radius = Math.Round(clArc.Radius, 2);
                    clArc.Angle = Units.Angle.FromDegrees(Math.Round(clArc.Angle.Degrees, 2));
                    clArc.Location = new Vector2((float)Math.Round(clArc.Location.X), (float)Math.Round(clArc.Location.Y));

                    var bowMiddle = clArc.GetPointOnArc(clArc.Angle / 2);
                    var l1 = new Line2(clArc.Location, bowMiddle);
                    var l2 = new Line2(clArc.GetPointOnArc(clArc.Angle), bowMiddle);
                    var intersection = new Line2(clArc.MiddlePoint, Point);
                    conatins = intersection.InterceptLineWith(l1) || intersection.InterceptLineWith(l2);
                }
            }
            return conatins;
        }
    }
}
