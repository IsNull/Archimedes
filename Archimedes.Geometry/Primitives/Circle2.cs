/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/


using Archimedes.Geometry.Units;

namespace Archimedes.Geometry.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using Archimedes.Geometry.Extensions;

    public class Circle2 : IGeometryBase, IClosedGeometry
    {
        #region Fields

        Vector2 _middlePoint;
        double _radius;
        Pen _pen = null;
        Brush _fillBrush = null;

        bool _verticesInValidated = true;
        Vertices _vertices = new Vertices();

        #endregion

        #region Constructors

        public Circle2() { }

        public Circle2(double x, double y, double uRadius, Pen pen)
        {
            _middlePoint = new Vector2(x, y);
            _radius = uRadius;
            Pen = pen;
        }

        public Circle2(double x, double y, double uRadius)
        {
            _middlePoint = new Vector2(x, y);
            _radius = uRadius;
        }
        public Circle2(Vector2 uMiddlePoint, double uRadius)
        {
            _middlePoint = uMiddlePoint;
            _radius = uRadius;
        }
        public Circle2(Vector2 uMiddlePoint, double uRadius, Pen pen)
        {
            _middlePoint = uMiddlePoint;
            _radius = uRadius;
            Pen = pen;
        }

        public Circle2(Circle2 prototype) {
            Prototype(prototype);
        }


        #endregion

        #region Exportet Properties

        public double Radius
        {
            get { return _radius; }
            set { 
                _radius = value;
                _verticesInValidated = true;
            }
        }

        public RectangleF DrawingRect {
            get {
                return new RectangleF(
                    (float)(this.Location.X - this.Radius), (float)(this.Location.Y - this.Radius),
                    (float)(2 * this.Radius), (float)(2 * this.Radius));
            }
        }

        #endregion

        #region Exp Methods

        public Vector2 GetPoint(Angle angle){
            var circlePoint = new Vector2(
                (float)Math.Cos(angle.Radians) * this.Radius,
                (float)Math.Sin(angle.Radians) * this.Radius);

            circlePoint += this.Location;
            return circlePoint;
        }

        #endregion

        #region Intersection

        public bool Contains(Vector2 point, double range) {
            return Circle2.Contains(this.MiddlePoint, this.Radius, point, range);
        }

        /// <summary>
        /// Checks if a Point lies in a circle
        /// (This is a strictly static method for performance reasons)
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="radius"></param>
        /// <param name="checkPoint"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool Contains(Vector2 middlePoint, double radius, Vector2 checkPoint, double range = 0)
        {
            var len = Line2.CalcLenght(middlePoint, checkPoint);
            var dist = radius - (len - range);
            return (dist >= 0);
        }


        #region Cricle - Line

        /// <summary>Circle-Line Interception
        /// Does the Line intercept with the Circle?
        /// </summary>
        /// <param name="uLine"></param>
        /// <returns></returns>
        private bool InterceptLineWith(Line2 uLine) {
            var List = InterceptLine(uLine);
            return (List.Count > 0);
        }

        /// <summary> 
        /// Circle-Line Interception 
        /// </summary>
        /// <param name="uLine"></param>
        /// <returns>Returns all intersection points</returns>
        private List<Vector2> InterceptLine(Line2 uLine) {
            var intersections = new List<Vector2>();
            Vector2 p1, p2;
            var location = this.Location;

            // we assume that the circle Middlepoint is NULL/NULL
            // So we move the Line with the delta to NULL
            var helperLine = new Line2(uLine.Start - location, uLine.End - location);

            // line
            var q = helperLine.YMovement;
            var m = helperLine.Slope;

            if (!helperLine.IsVertical) {
                // The slope is defined as the Line isn't vertical

                var discriminant = (Math.Pow(m, 2) + 1) * Math.Pow(this.Radius, 2) - Math.Pow(q, 2);
                if (discriminant > 0) {
                    // only positive discriminants for f() -> sqrt(discriminant) results are defined in |R



                    var p1X = (Math.Sqrt(discriminant) - m * (q)) / (Math.Pow(m, 2) + 1);
                    var p1Y = m * p1X + q;
                    var p2X = (-1 * (Math.Sqrt(discriminant) + m * q)) / (Math.Pow(m, 2) + 1);
                    var p2Y = m * p2X + q;

                    p1 = new Vector2(p1X, p1Y);
                    p2 = new Vector2(p2X, p2Y);

                    if (helperLine.Contains(p1)) {
                        intersections.Add(p1 + location);
                    }
                    if ((p1.X != p2.X) || (p1.Y != p2.Y)) {
                        if (helperLine.Contains(p2)) {
                            intersections.Add(p2 + location);
                        }
                    }
                }
            } else {
                // undefined slope, so we have to deal with it directly

                var p1X = this.Location.X + helperLine.Start.X;
                var p1Y = (float)Math.Sqrt(Math.Pow(this.Radius, 2) - Math.Pow(p1X, 2));
                p1 = new Vector2(p1X, p1Y);
                p2 = new Vector2(p1.X, -p1.Y);

                if (helperLine.Contains(p1)) {
                    intersections.Add(p1 + location);
                }
                if (helperLine.Contains(p2)) {
                    intersections.Add(p2 + location);
                }
            }

            return intersections;
        }
        #endregion

        #region Circle Rectangle

        private List<Vector2> InterceptRectangle2(Rectangle2 rect) {
            var intersections = new List<Vector2>();
            foreach (var border in rect.ToLines()) {
                intersections.AddRange(this.InterceptLine(border));
                border.Dispose();
            }
            return intersections;
        }

        #endregion

        #region Circle - Circle

        private bool InterceptWithCircle(Circle2 other){
            return !(Line2.CalcLenght(this.MiddlePoint, other.MiddlePoint) > (this.Radius + other.Radius));
        }

        private IEnumerable<Vector2> InterceptCircle(Circle2 other) {
            var interceptions = new List<Vector2>();
            if(InterceptWithCircle(other)){

                var d = Line2.CalcLenght(this.MiddlePoint, other.MiddlePoint);
                if (d < Math.Abs(this.Radius + other.Radius)) {
                    // circle is contained in other
                } else if (d == 0 && (this.Radius == other.Radius)) {
                    // circle are concident -> infinite numbers of intersections
                } else {
                    interceptions.AddRange(IntersectCircle(this, other));
                }
            }
            return interceptions;
        }

        private float Radius2 {
            get { return (float)Math.Pow(this.Radius, 2); }
        }

        /// <summary>
        /// Find Circle - Circle Intersectionpoints
        /// </summary>
        /// <param name="cA"></param>
        /// <param name="cB"></param>
        /// <returns></returns>
        private IEnumerable<Vector2> IntersectCircle(Circle2 cA, Circle2 cB) {

            var dv = cA.MiddlePoint - cB.MiddlePoint;
            var d2 = dv.X * dv.X + dv.Y * dv.Y;
            var d = Math.Sqrt(d2);

            if (d > cA.Radius + cB.Radius || d < Math.Abs(cA.Radius - cB.Radius))
                return new Vector2[] { }; // no solution

            var a = (cA.Radius2 - cB.Radius2 + d2) / (2 * d);
            var h = (float)Math.Sqrt(cA.Radius2 - a * a);
            var x2 = cA.MiddlePoint.X + a * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;
            var y2 = cA.MiddlePoint.Y + a * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;

            var paX = x2 + h * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;
            var paY = y2 - h * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;
            var pbX = x2 - h * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;
            var pbY = y2 + h * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;

            return new Vector2[] { new Vector2(paX, paY), new Vector2(pbX, pbY) };
        }

        #endregion

        #endregion

        #region Geometry Base

        public void AddToPath(GraphicsPath path) {
            path.AddEllipse(this.DrawingRect);
        }

        public void Translate(Vector2 mov) {
            this.Location += mov;
        }

        public Vector2 Location {
            get { return this.MiddlePoint; }
            set { this.MiddlePoint = value; }
        }

        public Vector2 MiddlePoint {
            get { return _middlePoint; }
            set { _middlePoint = value; }
        }

        public IGeometryBase Clone() {
            return new Circle2(this);
        }

        public void Prototype(IGeometryBase iprototype) {
            this.Location = iprototype.Location;
            this.Pen = iprototype.Pen.Clone() as Pen;
            this.Radius = (iprototype as Circle2).Radius;
        }


        #endregion

        #region GeomertryBase Collision

        public virtual bool Contains(Vector2 Point) {
            return Contains(Point, 0);
        }

        public virtual bool IntersectsWith(IGeometryBase other) {
            if (other is Line2) {
                return this.InterceptLineWith(other as Line2);
            } else if (other is Circle2) {
                return this.InterceptWithCircle(other as Circle2);
            } else if (other is Rectangle2) {
                return InterceptRectangle2(other as Rectangle2).Count != 0;
            } else if (other is Arc) { // inverse call:
                return other.IntersectsWith(this);
            } else
                return this.ToPolygon2().IntersectsWith(other);
        }

        public virtual IEnumerable<Vector2> Intersect(IGeometryBase other) {

            if (other is Line2) {
                return this.InterceptLine(other as Line2);
            } else if (other is Circle2) {
                return this.InterceptCircle(other as Circle2);
            } else if (other is Rectangle2) {
                return InterceptRectangle2(other as Rectangle2);
            }else if (other is Arc){ // inverse call:
                return other.Intersect(this);
            } else
                return this.ToPolygon2().Intersect(other);
        }

        public virtual Circle2 BoundingCircle {
            // bounding circle of a cricle... really abstract :)
            get { return this.Clone() as Circle2; }
        }

        public virtual RectangleF BoundingBox {
            get {
                return this.DrawingRect;
            }
        }

        #endregion

        #region Geometry Base Drawing

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public virtual void Draw(Graphics g) {
            try {
                if (this.FillBrush != null)
                    g.FillEllipse(this.FillBrush, this.DrawingRect);
                if (this.Pen != null)
                    g.DrawArc(this.Pen, this.DrawingRect, 0, 360);
            } catch (Exception) {
                //ignore
            }
        }

        #endregion

        public void Scale(double factor) {
            this.Location = Location.Scale(factor);
            Radius *= factor;
        }

        public virtual Vertices ToVertices() {
            if (_verticesInValidated) {
                var path = new GraphicsPath();
                _vertices.Clear();
                try {
                    path.AddArc(this.DrawingRect, 0, 360);
                    path.Flatten();
                    _vertices.AddRange(path.PathPoints);
                } catch (ArgumentException) {
                    // we ignore this -> void vertices
                }
            }
            return new Vertices(_vertices);
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(this.ToVertices())
            {
                FillBrush = this.FillBrush,
                Pen = this.Pen
            };
        }


        public void Dispose() {
            //try {
            //    if (Pen != null)
            //        this.Pen.Dispose();
            //    if (FillBrush != null)
            //        this.FillBrush.Dispose();
            //} catch (ArgumentException) {
            //    // ignore
            //}
        }

        #region IClosedGeometry

        public Brush FillBrush {
            get { return _fillBrush; }
            set { _fillBrush = value; }
        }

        public double Area {
            get { return Math.Pow(Radius, 2) * Math.PI; }
        }

        #endregion
    }
}
