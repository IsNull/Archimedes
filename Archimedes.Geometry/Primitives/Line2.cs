/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/
using System;
using System.Drawing;
using System.Collections.Generic;
using Archimedes.Geometry.Extensions;
using System.Drawing.Drawing2D;

namespace Archimedes.Geometry.Primitives
{

    public partial class Line2 : IGeometryBase
    {
        
        #region Fields

        const float roundingDiff = 0.5f;

        PointF _start;
        PointF _end;

        Pen _pen = null;
        bool _penSharedResource = false;

        #endregion

        #region Constructors

        public Line2() { }

        public Line2(float Lenght) {
            this.P1 = new PointF(0, 0);
            this.P2 = new PointF(Lenght,0);
        }

        public Line2(PointF uP1, PointF uP2) {
            this._start = uP1;
            this._end = uP2;
        }

        public Line2(PointF uP1, PointF uP2, Pen uPen) {
            this._start = uP1;
            this._end = uP2;
            this.Pen = uPen;
        }

        public Line2(float uP1x, float uP1y, float uP2x, float uP2y) {
            _start.X = uP1x;
            _start.Y = uP1y;
            _end.X = uP2x;
            _end.Y = uP2y;
        }

        public Line2(Line2 prototype) {
            Prototype(prototype);
        }

        public virtual void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as Line2;
            if (prototype == null)
                throw new NotImplementedException();

            this.P1 = prototype.P1;
            this.P2 = prototype.P2;
            try {
                if (prototype.Pen != null)
                    this.Pen = prototype.Pen.Clone() as Pen;
            } catch {

            }
        }

        #endregion

        #region Public Propertys

        public PointF P1 {
            get { return _start; }
            set { _start = value; }
        }

        public PointF P2 {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary> 
        /// Return the q (movement from x axis). If slope isn't defined, this property is Zero.
        /// </summary>
        public float YMovement {
            get {   // q = y1 - m * x1 
                if (this.IsVertical) {
                    return 0;
                }
                else {
                    return this.P1.Y - ((float)this.Slope * this.P1.X);
                }
            }
        }

        /// <summary>
        /// Returns the solpe of the line. Returns Zero if the slope isn't defined.
        /// </summary>
        public float Slope {
            get {
                if ((this.P2.X - this.P1.X) == 0) { // prevent divison by zero
                    return 0;
                }
                return (this.P2.Y - this.P1.Y) / (this.P2.X - this.P1.X);
            }
        }


        public void Stretch(float len, Direction direction){
            Vector2 vThis = new Vector2(this.P1,this.P2);

            if (direction == Direction.RIGHT) {
                vThis.Lenght += len;
                this.P2 = vThis.GetPoint(this.P1);
            } else {
                vThis.Lenght = len;
                vThis *= -1;
                this.P1 = vThis.GetPoint(this.P1);
            }
        }

        public double Lenght {
            get { return CalcLenght(this.P1, this.P2); }
        }

        /// <summary>
        /// Calculates the distance between two Points
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double CalcLenght(PointF start, PointF end) {
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
        }
      

        /// <summary>
        /// Compare the slope of two lines  
        /// </summary>
        /// <param name="Line2"></param>
        /// <returns>true, if the solpes are equal</returns>
        public bool EqualSlope(Line2 Line2, short Round = 3) {

            if (Line2 == null) {
                return false;
            }
            if (this.IsVertical && Line2.IsVertical) {
                return true;
            }
            else if (this.IsVertical || Line2.IsVertical) {
                return false;
            }
            else if (Math.Round(this.Slope,Round) == Math.Round(Line2.Slope,Round)) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Is this Line vertical?
        /// </summary>
        public bool IsVertical {
            get { return (Math.Abs(this.P2.X - this.P1.X) <= roundingDiff); }
        }

        /// <summary>
        /// Is this Line horizontal?
        /// </summary>
        public bool IsHorizontal {
            get { return (Math.Abs(this.P2.Y - this.P1.Y) <= roundingDiff); }
        }
        /// <summary>
        /// Is this Line horizontal or vertical?
        /// </summary>
        public bool IsHorOrVert {
            get {
                return this.IsHorizontal || this.IsVertical;
            }
        }


        #endregion

        #region Specail Transformators
        /// <summary>
        /// Scale the line by given scale factor
        /// </summary>
        /// <param name="factor">Scale Factor</param>
        public void Scale(float factor) {
            this.P1 = P1.Scale(factor);
            this.P2 = P2.Scale(factor);
        }

        /// <summary>
        /// Explodes a Rectangle into the 4 border lines and returns an array of lines
        /// </summary>
        /// <param name="Rect"></param>
        /// <returns>Line2d List<></returns>
        public static Line2[] RectExplode(RectangleF Rect) {
            var Sides = new Line2[4];
            Sides[0] = new Line2(Rect.X, Rect.Y, Rect.X + Rect.Width, Rect.Y);                                 // upper horz line
            Sides[1] = new Line2(Rect.X, Rect.Y + Rect.Height, Rect.X + Rect.Width, Rect.Y + Rect.Height);     // lower horz line
            Sides[2] = new Line2(Rect.X, Rect.Y, Rect.X, Rect.Y + Rect.Height);                                // left  vert line
            Sides[3] = new Line2(Rect.X + Rect.Width, Rect.Y, Rect.X + Rect.Width, Rect.Y + Rect.Height);      // right  vert line
            return Sides;
        }

        public Vector2 ToVector() {
            return new Vector2(this.P1, this.P2);
        }

        public IEnumerable<PointF> ToVertices() {
            return new PointF[] { this.P1, this.P2 };
        }

        #endregion

        #region Geomerty Base

        public void AddToPath(GraphicsPath path) {
            path.AddLine(this.P1, this.P2);
        }

        public PointF Location {
            get {
                return this.P1;
            }
            set {
                Move(new Vector2(this.P1, value));
            }
        }

        public PointF MiddlePoint {
            get {
                Vector2 vLine = (new Vector2(P1, P2)) / 2;
                return vLine.GetPoint(P1);
            }
            set {
                Move(new Vector2(this.MiddlePoint, value));
            }
        }

        public void Move(Vector2 mov) {
            this.P1 = mov.GetPoint(this.P1);
            this.P2 = mov.GetPoint(this.P2);
        }

        /// <summary>
        /// Creates a new Line, clone of the origin but moved by given vector
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public static Line2 CreateMoved(Line2 origin, Vector2 move) {
            var clone = origin.Clone() as Line2;
            clone.Move(move);
            return clone;
        }


        public IGeometryBase Clone() {
            return new Line2(this);
        }

        public RectangleF BoundingBox {
	        get { 
                PointF SP;
                if (P1.X > P2.X) {
                    SP = P1;
                }else if (P1.X == P2.X){
                    if(P1.Y < P2.Y)
                        SP = P1;
                    else
                        SP = P2;
                }else
                    SP = P2;

                return new RectangleF(SP, new SizeF(Math.Abs(P1.X - P2.X), Math.Abs(P1.Y - P2.Y)));
            }
        }

        public Circle2 BoundingCircle {
            get { 
                var m = this.MiddlePoint;
                return new Circle2(m, (float)Line2.CalcLenght(m, this.P1));
            }
        }


        #endregion

        #region Geomerty Base Drawing

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public void Draw(Graphics G) {
            if (this.Pen != null && !P1.Equals(P2))
                G.DrawLine(this.Pen, this.P1, this.P2);
        }

        #endregion

        #region Geomtry Base Collision


        public IEnumerable<PointF> Intersect(IGeometryBase other){
            var pnts = new List<PointF>();

            if (other is Line2) {
                var pnt = this.InterceptLine(other as Line2);
                if(pnt.HasValue)
                    pnts.Add(pnt.Value);
            } else if (other is GdiText2) {
                pnts.AddRange(this.InterceptRect(other.BoundingBox));
            } else
                other.Intersect(this);
            return pnts;
        }


        public bool IntersectsWith(IGeometryBase GeometryObject) {

            if (GeometryObject is Line2) {
                return this.InterceptLineWith(GeometryObject as Line2);
            } else if (GeometryObject is GdiText2 || GeometryObject is ImageDrawable) {
                return this.InterceptRectWith(GeometryObject.BoundingBox);
            } else { //delegate Collision Detection to other Geometry Object
                return GeometryObject.IntersectsWith(this);
            }
        }


        /// <summary>Checks if a Point is on the 2dLine
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <returns>true/false</returns>
        public bool Contains(PointF Point) {
            return Contains(Point, 0.0);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate smalles distance to a Target Point
        /// </summary>
        /// <param name="pt">Target Point</param>
        /// <param name="closest">Closest Point on Line to Targetpoint</param>
        /// <returns>Distance to target point</returns>
        public double FindDistanceToPoint(PointF pt, out PointF closest) {
            return Line2.FindDistanceToPoint(pt, this, out closest);
        }
        public double FindDistanceToPoint(PointF pt) {
            PointF dummy;
            return Line2.FindDistanceToPoint(pt, this, out dummy);
        }

        /// <summary>
        /// Calculate the distance between point pt and the line.
        /// </summary>
        /// <param name="pt">Target Point to wich distance is calculated</param>
        /// <param name="line">line</param>
        /// <param name="closest">closes Point on Line to target Point</param>
        /// <returns>Smallest distance to the targetpoint</returns>
        public static double FindDistanceToPoint(PointF pt, Line2 line, out PointF closest) {
            var p1 = line.P1; var p2 = line.P2;
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0)) {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0) {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            } else if (t > 1) {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            } else {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        #endregion

        #region IDisposable

        public virtual void Dispose() {
            //if (_pen != null && !PenSharedResource)
            //    _pen.Dispose();
        }

        #endregion

        public override string ToString() {
            string dump = "";
            dump += "P1: " + this.P1.ToString() + "\n";
            dump += "P2: " + this.P2.ToString() + "\n";
            dump += "slope: " + this.Slope + "\n";
            dump += "q: " + this.YMovement + "\n";
            dump += "vert: " + this.IsVertical + "\n";
            dump += "horz: " + this.IsHorizontal + "\n";

            return dump;
        }
    }
}
