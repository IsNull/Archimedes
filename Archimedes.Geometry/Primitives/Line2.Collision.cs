using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Archimedes.Geometry.Primitives
{
    public partial class Line2 : IGeometryBase
    {


        /// <summary>
        /// Calculates the interception point of two Lines.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns a Point if func succeeds. If there is no interception, empty point is returned.</returns>
        private Vector2? InterceptLine(Line2 other)
        {

            double interPntX = 0;
            double interPntY = 0;

            if (other == null || this.EqualSlope(other)) {
                return null;            // Lines are parralell
            }

            //intercept of two endless lines
            if (!this.IsVertical && !other.IsVertical) {    // both NOT vertical
                interPntX = ((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                interPntY = (this.Slope * interPntX + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                interPntX = this.Start.X;
                interPntY = (other.Slope * interPntX + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                interPntX = other.Start.X;
                interPntY = (this.Slope * interPntX + this.YMovement);
            }

            var interPnt = new Vector2(interPntX, interPntY);

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
        public bool InterceptLineWith(Line2 other)
        {
            double IsPX = 0;
            double IsPY = 0;

            if (other == null || this.EqualSlope(other))
                return false;

            //intercept of two endless lines
            if ((!this.IsVertical) && (!other.IsVertical)) {    // both NOT vertical
                IsPX = (float)((-1 * (this.YMovement - other.YMovement)) / (this.Slope - other.Slope));
                IsPY = (float)(this.Slope * IsPX + this.YMovement);
            } else if (this.IsVertical) {                  // this vertical (so it must lie on this.X)
                IsPX = this.Start.X;
                IsPY = (float)(other.Slope * IsPX + other.YMovement);
            } else if (other.IsVertical) {                // Line2 vertical (so it must lie on Line2.X)
                IsPX = other.Start.X;
                IsPY = (float)(this.Slope * IsPX + this.YMovement);
            }

            var IsP = new Vector2(IsPX, IsPY);

            // check if computed intercept lies on our line.
            return (this.Contains(IsP, 1) && other.Contains(IsP, 1));
        }

        /// <summary>
        /// Returns true if there is at least one Interception Point.
        /// </summary>
        /// <param name="rect1"></param>
        /// <returns></returns>
        public bool InterceptRectWith(RectangleF rect1) {
            
            // is Start/EndPoint in the Rectangle?
            if (rect1.Contains(this.Start) || rect1.Contains(this.End))
                return true; 
            // crosses the Line a Rectangle Border?
            var borderLines = Line2.RectExplode(rect1); //get 4 borderlines from rect

            // check if any of the borderlines intercept with this line
            return borderLines.Any(border => this.InterceptLineWith(border));
        }


        /// <summary>Returns every intersection Point of the Rect::Line if any. Max returned Points are 2.
        /// This Method actaully can't handle full contained Lines in the Rect - use the InterceptRectWith to determite if there is collision.
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Intercepts">Points</param>
        /// <returns>Returns count of Interception Points</returns>
        public List<Vector2> InterceptRect(RectangleF Rect1) {
            var intercepts = new List<Vector2>(2);
            short i = 0;
            var borderLines = Line2.RectExplode(Rect1); //get 4 borderlines from rect

            foreach (var border in borderLines) {
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
        /// <param name="pos"></param>
        /// <param name="Range"></param>
        /// <returns>true/false</returns>
        public bool Contains(Vector2 pos, double Range) {

            // we can't check directly with math functions,
            // as it is possible that the slope is undefinied. (on vertical lines)

            if (this.IsVertical) {
                // vertical means that the slope is undefinied
                if (Math.Abs(this.Start.X - pos.X) <= Range) {
                    return (((pos.Y >= this.Start.Y) && (pos.Y <= this.End.Y)) || ((pos.Y <= this.Start.Y) && (pos.Y >= this.End.Y)));
                } else 
                    return false;
            } else {
                if (Math.Round(Math.Abs(pos.Y - (pos.X * (float)this.Slope + (float)this.YMovement)), 2) <= Range) {
                    return (((pos.X >= this.Start.X) && (pos.X <= this.End.X)) || ((pos.X <= this.Start.X) && (pos.X >= this.End.X)));
                } else
                    return false;
            }
        }
    }
}
