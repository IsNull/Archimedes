using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vertex = Archimedes.Geometry.Vector2;
using Archimedes.Geometry.Extensions;
using System.Drawing.Drawing2D;
using System.Drawing;
using Archimedes.Geometry.Builder;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Collection of connected Points
    /// </summary>
    public class Path2 : IGeometryBase
    {
        #region Fields

        System.Drawing.Pen _pen;
        readonly GraphicsPath _gpath = new GraphicsPath();

        #endregion

        #region Construcors

        public Path2() {
            _gpath.StartFigure();
        }

        public Path2(Path2 prototype) 
            : this() {
            this.Prototype(prototype);
        }
        public Path2(IGeometryBase geometry)
            : this() {
                AddGeometry(geometry);
        }
        public Path2(IEnumerable<IGeometryBase> geometries)
            : this() {
            foreach (var g in geometries)
                AddGeometry(g);
        }
        public Path2(IEnumerable<Vertex> vertices)
            : this() {
                AddVertices(vertices);
        }

        #endregion

        #region Data Access

        /// <summary>
        /// Adds  the <paramref name="geometry"/> to the path.
        /// </summary>
        /// <param name="geometry">Geomtry to add</param>
        public void AddGeometry(IGeometryBase geometry) {
            AddVertices(geometry.ToVertices());
        }
        /// <summary>
        /// Adds the <paramref name="vertices"/> to the path.
        /// </summary>
        /// <param name="vertices">Vertixes to add</param>
        public void AddVertices(IEnumerable<Vertex> vertices) {
            if(vertices.Count() > 1)
                _gpath.AddLines(Vertices.ToPoints(vertices).ToArray());
        }

        /// <summary>
        /// Tries to dock the <paramref name="geometry"/> to this path. 
        /// </summary>
        /// <param name="geometry">Geometry to dock on this path</param>
        public void DockGeometry(IGeometryBase geometry) {
            DockVertices(geometry.ToVertices());
        }
        public void DockVertices(IEnumerable<Vertex> vertices) {
            // does vertex order matther?
            if (_gpath.PointCount == 0) {
                AddVertices(vertices);
                return;
            }

            // sort the vertices to make connection possible
            var connector = new PathConnector(this.ToVertices(), vertices);
            Clear();
            AddVertices(connector.ConnectPaths());
        }

        public void Clear() {
            _gpath.Reset();
        }

        #endregion

        public Vector2 LastPoint {
            get { return _gpath.PointCount != 0 ? _gpath.GetLastPoint() : new PointF(); }
        }

        public Vector2 FirstPoint {
            get {
                return _gpath.PointCount != 0 ? _gpath.PathPoints[0] : new PointF();
            }
        }

        public virtual void Dispose() {
            _gpath.Dispose();
        }

        #region IGeometryBase

        public Vector2 Location {
            get { return MiddlePoint; }
            set { MiddlePoint = value; }
        }

        public Vector2 MiddlePoint {
            get {
                return this.BoundingBox.MiddlePoint();
            }
            set {
                var move = new Vector2(this.MiddlePoint, value);
                this.Move(move);
            }
        }

        public IGeometryBase Clone() {
            return new Path2(this);
        }

        public void Prototype(IGeometryBase prototype) {
            _gpath.Reset();
            prototype.AddToPath(_gpath);
            this.Pen = prototype.Pen;
        }

        public void Move(Vector2 mov) {
            for (int i = 0; i < _gpath.PathPoints.Count(); i++)
                _gpath.PathPoints[i] +=  mov;
        }

        public void Scale(double fact) {
            for (int i = 0; i < _gpath.PathPoints.Count(); i++)
                _gpath.PathPoints[i] = _gpath.PathPoints[i].Scale(fact);
        }

        public Vertices ToVertices() {
            return new Vertices(_gpath.PathPoints);
        }

        public System.Drawing.RectangleF BoundingBox {
            get { return ToVertices().BoundingBox; }
        }

        public System.Drawing.Pen Pen {
            get {return _pen; }
            set { _pen = value; }
        }

        public Circle2 BoundingCircle {
            get { return BoundingBox.BoundingCircle(); }
        }

        public bool IntersectsWith(IGeometryBase GeometryObject) {
            throw new NotImplementedException();
        }

        public IEnumerable<Vector2> Intersect(IGeometryBase other) {
            throw new NotImplementedException();
        }

        public bool Contains(Vector2 pnt) {
            return this.ToVertices().Contains(pnt);
        }

        public void Draw(System.Drawing.Graphics g) {
            if (_gpath.PointCount > 0)
                g.DrawPath(Pen, _gpath);
        }

        public void AddToPath(GraphicsPath path) {
            if (_gpath.PointCount > 0)
                path.AddPath(_gpath, true);
        }

        #endregion
    }
}
