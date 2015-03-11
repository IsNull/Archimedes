/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/


using System.Linq;
using Archimedes.Geometry.Units;

namespace Archimedes.Geometry.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using Archimedes.Geometry.Extensions;

    /// <summary>
    /// Represents a circle shape in 2d Space
    /// </summary>
    public partial class Circle2 : IShape
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

        /// <summary>
        /// Creates an empty circle
        /// </summary>
        public Circle2() { }

        /// <summary>
        /// Creates a new circle at the given location with the given radius
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="uRadius"></param>
        public Circle2(double x, double y, double uRadius)
        {
            _middlePoint = new Vector2(x, y);
            _radius = uRadius;
        }
        
        public Circle2(double x, double y, double uRadius, Pen pen)
        {
            _middlePoint = new Vector2(x, y);
            _radius = uRadius;
            Pen = pen;
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


        #endregion

        #region Exp Methods

        /// <summary>
        /// Returns a point on this circle at the given angle offset.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Vector2 GetPoint(Angle angle){
            var circlePoint = new Vector2(
                Math.Cos(angle.Radians) * this.Radius,
                Math.Sin(angle.Radians) * this.Radius);

            circlePoint += this.Location;
            return circlePoint;
        }

        #endregion

        #region Geometry Base

        public void Scale(double factor)
        {
            this.Location = Location.Scale(factor);
            Radius *= factor;
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

        public IGeometry Clone() {
            return new Circle2(this);
        }

        public void Prototype(IGeometry iprototype) {
            this.Location = iprototype.Location;
            this.Pen = iprototype.Pen.Clone() as Pen;
            this.Radius = (iprototype as Circle2).Radius;
        }


        #endregion

        #region GeomertryBase Collision

        public virtual bool IntersectsWith(IGeometry other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            if (other is Line2)
            {
                return this.InterceptLineWith(other as Line2, tolerance);
            }
            else if (other is Circle2)
            {
                return this.InterceptWithCircle(other as Circle2, tolerance);
            }
            else if (other is Polygon2)
            {
                return InterceptPolygon(((Polygon2) other), tolerance).Count != 0;
            }
            else if (other is Rectangle2)
            {
                return InterceptPolygon(((Rectangle2) other).ToPolygon2(), tolerance).Count != 0;
            }
            else if (other is Arc)
            {
                // inverse call:
                return other.IntersectsWith(this, tolerance);
            }
            else
                return this.ToPolygon2().IntersectsWith(other, tolerance);
        }

        public virtual IEnumerable<Vector2> Intersect(IGeometry other,
            double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            if (other is Line2)
            {
                return this.InterceptLine(other as Line2, tolerance);
            }
            else if (other is Circle2)
            {
                return this.InterceptCircle(other as Circle2, tolerance);
            }
            else if (other is Polygon2)
            {
                return InterceptPolygon(other as Polygon2, tolerance);
            }
            else if (other is Rectangle2)
            {
                return InterceptPolygon((other as Rectangle2).ToPolygon2(), tolerance);
            }
            else if (other is Arc)
            {
                // inverse call:
                return other.Intersect(this, tolerance);
            }
            else
                return this.ToPolygon2().Intersect(other, tolerance);
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
                    g.FillEllipse(this.FillBrush, RectangleFUtil.ToRectangleF(this.BoundingBox));
                if (this.Pen != null)
                    g.DrawArc(this.Pen, RectangleFUtil.ToRectangleF(this.BoundingBox), 0, 360);
            } catch (Exception) {
                //ignore
            }
        }

        #endregion

        #region To-Methods

        public virtual Vertices ToVertices() {
            if (_verticesInValidated) {
                var path = new GraphicsPath();
                _vertices.Clear();
                try {
                    path.AddArc(RectangleFUtil.ToRectangleF(this.BoundingBox), 0, 360);
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

        #endregion

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

        #region IShape

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
