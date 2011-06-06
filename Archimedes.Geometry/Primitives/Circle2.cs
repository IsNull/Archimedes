/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/


namespace Archimedes.Geometry.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using Archimedes.Geometry.Extensions;

    public class Circle2 : IGeometryBase, IClosedGeometry
    {
        #region Internal Data

        PointF _middlePoint = new PointF();
        float _radius;
        Pen _pen = null;
        Brush _fillBrush = null;

        bool _verticesInValidated = true;
        List<PointF> _vertices = new List<PointF>();

        #endregion

        #region Constructors

        public Circle2() { }

        public Circle2(float x, float y, float uRadius, Pen pen) {
            _middlePoint = new PointF(x, y);
            _radius = uRadius;
            Pen = pen;
        }

        public Circle2(float x, float y, float uRadius) {
            _middlePoint = new PointF(x, y);
            _radius = uRadius;
        }
        public Circle2(PointF uMiddlePoint, float uRadius) {
            _middlePoint = uMiddlePoint;
            _radius = uRadius;
        }
        public Circle2(PointF uMiddlePoint, float uRadius, Pen pen) {
            _middlePoint = uMiddlePoint;
            _radius = uRadius;
            Pen = pen;
        }

        public Circle2(Circle2 prototype) {
            Prototype(prototype);
        }


        #endregion

        #region Exportet Properties

        public float Radius {
            get { return _radius; }
            set { 
                _radius = value;
                _verticesInValidated = true;
            }
        }

        public RectangleF DrawingRect {
            get {
                return new RectangleF(this.Location.X - this.Radius, this.Location.Y - this.Radius, 2 * this.Radius, 2 * this.Radius);
            }
        }

        #endregion

        #region Exp Methods

        private PointF AddPoints(PointF P1, PointF P2) {
            P1.X = P1.X + P2.X;
            P1.Y = P1.Y + P2.Y;
            return P1;
        }

        private PointF SubPoints(PointF P1, PointF P2) {
            P1.X = P1.X - P2.X;
            P1.Y = P1.Y - P2.Y;
            return P1;
        }



        public PointF GetPoint(float Angle){
            var CirclePoint = new PointF(0,0);

            CirclePoint.X = (float)Math.Cos(Degree2RAD(Angle)) * this.Radius;
            CirclePoint.Y = (float)Math.Sin(Degree2RAD(Angle)) * this.Radius;

            CirclePoint.X += this.Location.X;
            CirclePoint.Y += this.Location.Y;

            return CirclePoint;
        }

        #endregion

        #region Intersection

        public bool Contains(PointF point, float range) {
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
        public static bool Contains(PointF middlePoint, float radius, PointF checkPoint, float range = 0) {
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

        /// <summary> Circle-Line Interception 
        /// 
        /// </summary>
        /// <param name="uLine"></param>
        /// <returns></returns>
        private List<PointF> InterceptLine(Line2 uLine) {
            var InterSeptionPoints = new List<PointF>();
            var P1 = new PointF();
            var P2 = new PointF();

            var M = this.Location;

            // we assume that the circle Middlepoint is NULL/NULL
            // So we move the Line with the delta to NULL
            var Line = new Line2(uLine.P1.X - M.X, uLine.P1.Y - M.Y, uLine.P2.X - M.X, uLine.P2.Y - M.Y);

            ///line
            float q = Line.YMovement;
            float m = Line.Slope;
            //circle

            float r = this.Radius;

            float D;    //Discriminant

            if (!Line.IsVertical) {
                // The slope is defined as the Line isn't vertical

                D = (float)((Math.Pow(m, 2) + 1) * Math.Pow(r, 2) - Math.Pow(q, 2));
                if (D > 0) {
                    //only posivive sqrt() results in a rational number

                    P1.X = (float)((Math.Sqrt(D) - m * (q)) / (Math.Pow(m, 2) + 1));
                    P2.X = (float)((-1 * (Math.Sqrt(D) + m * q)) / (Math.Pow(m, 2) + 1));
                    P1.Y = m * P1.X + q;
                    P2.Y = m * P2.X + q;

                    if (Line.Contains(P1)) {
                        InterSeptionPoints.Add(AddPoints(P1, M));
                    }
                    if ((P1.X != P2.X) || (P1.Y != P2.Y)) {
                        if (Line.Contains(P2)) {
                            InterSeptionPoints.Add(AddPoints(P2, M));
                        }
                    }
                }
            } else {
                //undefined slope, so we have to deal with it directly
                float dx, dy; //delta x,y

                dx = this.Location.X + Line.P1.X;
                dy = (float)Math.Sqrt(Math.Pow(this.Radius, 2) - Math.Pow(dx, 2));
                P1.X = dx;
                P1.Y = dy;

                P2.X = dx;
                P2.Y = -dy;

                if (Line.Contains(P1)) {
                    InterSeptionPoints.Add(AddPoints(P1, M));
                }
                if (Line.Contains(P2)) {
                    InterSeptionPoints.Add(AddPoints(P2, M));
                }
            }

            return InterSeptionPoints;
        }
        #endregion

        #region Circle Rectangle

        private List<PointF> InterceptRectangle2(Rectangle2 rect) {
            var intersections = new List<PointF>();
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

        private IEnumerable<PointF> InterceptCircle(Circle2 other) {
            List<PointF> interceptions = new List<PointF>();
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
        private IEnumerable<PointF> IntersectCircle(Circle2 cA, Circle2 cB) {

            float dx = cA.MiddlePoint.X - cB.MiddlePoint.X;
            float dy = cA.MiddlePoint.Y - cB.MiddlePoint.Y;
            float d2 = dx * dx + dy * dy;
            float d = (float)Math.Sqrt(d2);

            if (d > cA.Radius + cB.Radius || d < Math.Abs(cA.Radius - cB.Radius))
                return new PointF[]{}; // no solution

            float a = (cA.Radius2 - cB.Radius2 + d2) / (2 * d);
            float h = (float)Math.Sqrt(cA.Radius2 - a * a);
            float x2 = cA.MiddlePoint.X + a * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;
            float y2 = cA.MiddlePoint.Y + a * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;

            float paX = x2 + h * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;
            float paY = y2 - h * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;
            float pbX = x2 - h * (cB.MiddlePoint.Y - cA.MiddlePoint.Y) / d;
            float pbY = y2 + h * (cB.MiddlePoint.X - cA.MiddlePoint.X) / d;

            return new PointF[] { new PointF(paX, paY), new PointF(pbX, pbY) };
        }

        


        #endregion

        #endregion

        #region Geometry Base

        public void AddToPath(GraphicsPath path) {
            path.AddEllipse(this.DrawingRect);
        }

        public void Move(Vector2 mov) {
            this.Location = mov.GetPoint(this.Location);
        }

        public PointF Location {
            get { return this.MiddlePoint; }
            set { this.MiddlePoint = value; }
        }

        public PointF MiddlePoint {
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

        public virtual bool Contains(PointF Point) {
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

        public virtual IEnumerable<PointF> Intersect(IGeometryBase other) {

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

        public virtual void Draw(Graphics G) {
            try {
                if (this.FillBrush != null)
                    G.FillEllipse(this.FillBrush, this.DrawingRect);
                if (this.Pen != null)
                    G.DrawArc(this.Pen, this.DrawingRect, 0, 360);
            } catch (Exception) {
                //ignore
            }
        }

        #endregion

        #region Private Helper Methods

        // To convert a degrees value to radians, multiply it by pi/180 (approximately 0.01745329252).
        private static float Degree2RAD(float degree) {
            return (float)(degree * Math.PI / 180);
        }

        // To convert a radians value to degrees, multiply it by 180/pi (approximately 57.29578).
        private static float RAD2Degree(float RAD) {
            return (float)(RAD * 180 / Math.PI);
        }

        #endregion

        public void Scale(float factor) {
            this.Location = Location.Scale(factor);
            Radius *= factor;
        }

        public virtual IEnumerable<PointF> ToVertices() {
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
            return _vertices;
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(this.ToVertices())
            {
                FillBrush = this.FillBrush,
                Pen = this.Pen
            };
        }


        public void Dispose() {
            try {
                if (Pen != null)
                    this.Pen.Dispose();
                if (FillBrush != null)
                    this.FillBrush.Dispose();
            } catch (ArgumentException) {
                // ignore
            }
        }

        #region IClosedGeometry

        public Brush FillBrush {
            get { return _fillBrush; }
            set { _fillBrush = value; }
        }

        public float Area {
            get { return (float)(Math.Pow(Radius, 2) * Math.PI); }
        }

        #endregion
    }
}
