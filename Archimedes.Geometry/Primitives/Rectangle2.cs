﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using System.Drawing.Drawing2D;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Represents an (rotated) 2D Rectangle
    /// </summary>
    public class Rectangle2 : IGeometryBase, IClosedGeometry
    {
        #region Fields
        //Internal Data which defines a rotatable rect

        Vector2 _middlePoint;
        float _width;
        float _height;
        float _rotateAngle = 0; //rotation is considered centric

        Pen _pen = null;
        Brush _brush = null;

        #endregion

        #region Public Propertys

        public Brush FillBrush {
            get { return _brush; }
            set { _brush = value;}
        }

        public float Angle {
            get { return _rotateAngle; }
            set { _rotateAngle = value; }
        }

        public bool IsRotated {
            get { return (this.Angle != 0); }
        }

        public float Height {
            get {
                return _height;
            }
            set {
                _height = value;
            }
        }

        public float Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }


        public SizeF Size {
            get {
                return new SizeF(_width, _height);
            }
            set {
                _width = value.Width;
                _height = value.Height;
            }
        }

        /// <summary>
        /// Area of this Rectangle
        /// </summary>
        public float Area {
            get { return Height * Width; }
        }

        #endregion

        #region Constructor's

        public Rectangle2() { }

        public Rectangle2(Vector2[] vertices) {

            if (vertices.Count() != 4)
                throw new ArgumentException("You must submit 4 vertices!");

            //Calc width and height Vectors
            var vW = new Vector2(vertices[0], vertices[1]);
            var vH = new Vector2(vertices[1], vertices[2]);

            //Use Vector Geometry to walk to the middlepoint
            _middlePoint = ((vW / 2) + (vH / 2)).GetPoint(vertices[0]);

            _rotateAngle = vW.GetAngle2X();
            _width = vW.Lenght;
            _height = vH.Lenght;
        }

        public Rectangle2(float x, float y, float uwidth, float uheight, float angle = 0 ) {
            _width = uwidth;
            _height = uheight;
            this.Location = new Vector2(x, y);
            this.Angle = angle;
        }
        public Rectangle2(Vector2 uLocation, SizeF uSize, float angle = 0) {
            this.Size = uSize;
            this.Location = uLocation;
            this.Angle = angle;
        }

        public Rectangle2(RectangleF uRectF, float angle = 0) {
            this.Size = uRectF.Size;
            this.Location = uRectF.Location;
            this.Angle = angle;
        }
        public Rectangle2(Rectangle2 prototype, float angle = 0) {
            Prototype(prototype, angle); 
        }

        
        public void Prototype(Rectangle2 prototype, float angle) {
            Prototype(prototype);
            this.Angle += angle;
        }

        public void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as Rectangle2;
            if (prototype == null)
                throw new NotSupportedException();

            this.Size = prototype.Size;
            this.MiddlePoint = prototype.MiddlePoint;
            this.Angle = prototype.Angle;
            this._pen = prototype.Pen;
            this.FillBrush = prototype.FillBrush;
        }

        #endregion

        #region Public Misc Methods

        /// <summary>
        /// Gets the Vertices (Points) of this Rectangle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vector2> ToVertices() {
            var rect = new RectangleF(new Vector2(MiddlePoint.X - Width / 2, MiddlePoint.Y - Height / 2), this.Size);
            var vertices = rect.ToVertices();

            if (_rotateAngle != 0)
                vertices = VerticesHelper.RotateVertices(vertices, this.MiddlePoint, Angle);

            return vertices;
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(this.ToVertices())
            {
                Pen = this.Pen,
                FillBrush = this.FillBrush
            };
        }

        public RectangleF ToRectangleF(bool forceConversation = false) {
            if (this.Angle != 0 || forceConversation)
                throw new NotSupportedException("Can not transform rotated Rectangle2 to RectangleF!");
            return new RectangleF(this.Location, this.Size);
        }

        /// <summary>
        /// Creates the 4 Lines which encloses this Rectangle
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Line2> ToLines() {

            var vertices = this.ToVertices().ToArray();

            if (vertices.Count() != 4)
                return new Line2[0]; 

            var lines = new Line2[] 
            {
                new Line2(vertices[0], vertices[1]),
                new Line2(vertices[1], vertices[2]),
                new Line2(vertices[2], vertices[3]),
                new Line2(vertices[3], vertices[0])
            };

            return lines;
        }

        #endregion

        #region IGeometryBase

        public void AddToPath(GraphicsPath path) {
            this.ToPolygon2().AddToPath(path);
        }

        /// <summary>
        /// Gets or sets the Location of the upper Left Corner of this Rectangle
        /// </summary>
        public Vector2 Location {
            get {
                var upperleftCorner = new Vector2(MiddlePoint.X - Width / 2, MiddlePoint.Y - Height / 2);
                if (this.IsRotated) { //optimisation - do only if we have a rotated rect
                    var vToUpperLeft = new Vector2(MiddlePoint, upperleftCorner);
                    upperleftCorner = vToUpperLeft.GetRotated(Angle).GetPoint(MiddlePoint);
                }
                return upperleftCorner;
            }
            set {
                Move(new Vector2(Location, value));
            }
        }

        /// <summary>
        /// Move this Rectangle along the given Vector
        /// </summary>
        /// <param name="mov"></param>
        public void Move(Vector2 mov) {
            MiddlePoint = mov.GetPoint(MiddlePoint);
        }

        public void Scale(float fact) {
            throw new NotImplementedException();
        }

        public Vector2 MiddlePoint {
            get { return this._middlePoint; }
            set { this._middlePoint = value; }
        }

        public IGeometryBase Clone() {
            return new Rectangle2(this);
        }

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public virtual void Draw(Graphics G) {
            this.ToPolygon2().Draw(G);
        } 

        #endregion

        public virtual void Dispose() {
            Pen.Dispose();
            FillBrush.Dispose();
        }

        public override string ToString() {
            return string.Format(@"[{0}/{1}] {2}°", _width, _height, _rotateAngle); //rotation is considered centric
        }

        #region IGeometryBase Collision Detection

        public bool Contains(Vector2 point) {
            if (this.IsRotated)
                return this.ToPolygon2().Contains(point);
            else //optimisation - use simple rect if no rotation is given
                return this.ToRectangleF().Contains(point);
        }

        public Circle2 BoundingCircle {
            get {
                return this.ToPolygon2().BoundingCircle;
            }
        }

        public RectangleF BoundingBox {
            get { return this.ToPolygon2().BoundingBox; }
        }

        public bool IntersectsWith(IGeometryBase other) {
            foreach (var line in this.ToLines()) {
                if (other.IntersectsWith(line))
                    return true;
            }
            return false;
        }
        public IEnumerable<Vector2> Intersect(IGeometryBase other) {
            var intersections = new List<Vector2>();

            foreach (var line in this.ToLines())
                intersections.AddRange(other.Intersect(line));

            return intersections;
        }


        #endregion
    }
}