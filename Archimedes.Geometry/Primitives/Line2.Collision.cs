using System;
using System.Collections.Generic;
using System.Drawing;

namespace Archimedes.Geometry.Primitives
{
    public partial class Line2 : IGeometryBase
    {


        /// <summary>
        /// Calculates the interception point of two Lines.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns a Point if func succeeds. If there is no interception, empty point is returned.</returns>
        private PointF? InterceptLine(Line2 other) {
            PointF interPnt = new PointF();

            if (other == null || this.EqualSlope(other)) {
                return null;            // Lines are parralell
            }

            //intercept of two endless lines
            if (!this.IsVertical && !other.IsVertical) {    // both NOT vertical
                interPnt.X = (float)((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                interPnt.Y = (float)(this.Slope * interPnt.X + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                interPnt.X = this.P1.X;
                interPnt.Y = (float)(other.Slope * interPnt.X + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                interPnt.X = other.P1.X;
                interPnt.Y = (float)(this.Slope * interPnt.X + this.YMovement);
            }
            //check if computed intercept lies on our line.
            if (this.Contains(interPnt) && other.Contains(interPnt)) {
                return interPnt;
            }else
                return null;
        }

        /// <summary>
        /// Is there a interception?
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns true/false.</returns>
        public bool InterceptLineWith(Line2 other) {
            var IsP = new PointF();

            if (other == null || this.EqualSlope(other))
                return false;

            //intercept of two endless lines
            if ((!this.IsVertical) && (!other.IsVertical)) {    // both NOT vertical
                IsP.X = (float)((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                IsP.Y = (float)(this.Slope * IsP.X + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                IsP.X = this.P1.X;
                IsP.Y = (float)(other.Slope * IsP.X + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                IsP.X = other.P1.X;
                IsP.Y = (float)(this.Slope * IsP.X + this.YMovement);
            }

            // check if computed intercept lies on our line.
            return (this.Contains(IsP, 1) && other.Contains(IsP, 1));
        }

        /// <summary>
        /// Returns true if there is at least one Interception Point.
        /// </summary>
        /// <param name="Rect1"></param>
        /// <returns></returns>
        public bool InterceptRectWith(RectangleF Rect1) {
            
            // is Start/EndPoint in the Rectangle?
            if (Rect1.Contains(this.P1) || Rect1.Contains(this.P2))
                return true; 
            // crosses the Line a Rectangle Border?
            var BorderLines = Line2.RectExplode(Rect1); //get 4 borderlines from rect
            foreach (Line2 border in BorderLines) {
                if (this.InterceptLineWith(border))
                    return true;
            }
            return false;
        }


        /// <summary>Returns every intersection Point of the Rect::Line if any. Max returned Points are 2.
        /// This Method actaully can't handle full contained Lines in the Rect - use the InterceptRectWith to determite if there is collision.
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Intercepts">Points</param>
        /// <returns>Returns count of Interception Points</returns>
        public List<PointF> InterceptRect(RectangleF Rect1) {
            List<PointF> intercepts = new List<PointF>(2);
            short i = 0;
            var borderLines = Line2.RectExplode(Rect1); //get 4 borderlines from rect

            foreach (Line2 border in borderLines) {
                if (this.InterceptLineWith(border)) {
                    // found interception
                    var pnt = this.InterceptLine(border);
                    if(pnt.HasValue)
                        intercepts.Add(pnt.Value);
                    i++;
                    if (i == 2) break;  
                }
            }
            return intercepts;
        }



        /// <summary>
        /// Checks if a Point is on the 2dLine (or in its range)
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Range"></param>
        /// <returns>true/false</returns>
        public bool Contains(PointF Point, double Range) {

            // we can't check directly with math functions,
            // as it is possible that the slope is undefinied. (on vertical lines)

            if (this.IsVertical) {
                // vertical means that the slope is undefinied
                if (Math.Abs(this.P1.X - Point.X) <= Range) {
                    return (((Point.Y >= this.P1.Y) && (Point.Y <= this.P2.Y)) || ((Point.Y <= this.P1.Y) && (Point.Y >= this.P2.Y)));
                } else 
                    return false;
            } else {
                if (Math.Round(Math.Abs(Point.Y - (Point.X * (float)this.Slope + (float)this.YMovement)), 2) <= Range) {
                    return (((Point.X >= this.P1.X) && (Point.X <= this.P2.X)) || ((Point.X <= this.P1.X) && (Point.X >= this.P2.X)));
                } else
                    return false;
            }
        }
    }
}
