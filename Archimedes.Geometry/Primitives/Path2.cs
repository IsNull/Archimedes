using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vertex = System.Drawing.PointF;
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
        #region Private Data

        private System.Drawing.Pen pen;
        GraphicsPath gpath = new GraphicsPath();

        #endregion

        #region Construcors

        public Path2() {
            gpath.StartFigure();
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
                gpath.AddLines(vertices.ToArray());
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
            if (gpath.PointCount == 0) {
                AddVertices(vertices);
                return;
            }

            // sort the vertices to make connection possible
            var connector = new PathConnector(this.ToVertices(), vertices);
            Clear();
            AddVertices(connector.ConnectPaths());
        }

        public void Clear() {
            gpath.Reset();
        }

        #endregion

        public PointF LastPoint {
            get { return gpath.PointCount != 0 ? gpath.GetLastPoint() : new PointF(); }
        }
        public PointF FirstPoint {
            get {
                return gpath.PointCount != 0 ? gpath.PathPoints[0] : new PointF();
            }
        }

        public virtual void Dispose() {
            Pen.Dispose();
            gpath.Dispose();
        }

        #region IGeometryBase

        public System.Drawing.PointF Location {
            get { return MiddlePoint; }
            set { MiddlePoint = value; }
        }

        public System.Drawing.PointF MiddlePoint {
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
            gpath.Reset();
            prototype.AddToPath(gpath);
            this.Pen = prototype.Pen;
        }

        public void Move(Vector2 mov) {
            for (int i = 0; i < gpath.PathPoints.Count(); i++)
                gpath.PathPoints[i] = mov.GetPoint(gpath.PathPoints[i]);
        }

        public void Scale(float fact) {
            for (int i = 0; i < gpath.PathPoints.Count(); i++)
                gpath.PathPoints[i] = gpath.PathPoints[i].Scale(fact);
        }

        public IEnumerable<Vertex> ToVertices() {
            return gpath.PathPoints;
        }

        public System.Drawing.RectangleF BoundingBox {
            get { return VerticesHelper.BoundingBox(ToVertices()); }
        }

        public System.Drawing.Pen Pen {
            get {return pen; }
            set { pen = value; }
        }

        public Circle2 BoundingCircle {
            get { return BoundingBox.BoundingCircle(); }
        }

        public bool IntersectsWith(IGeometryBase GeometryObject) {
            throw new NotImplementedException();
        }

        public IEnumerable<System.Drawing.PointF> Intersect(IGeometryBase other) {
            throw new NotImplementedException();
        }

        public bool Contains(System.Drawing.PointF Point) {
            throw new NotImplementedException();
        }

        public void Draw(System.Drawing.Graphics G) {
            G.DrawPath(Pen, gpath);
        }

        public void AddToPath(GraphicsPath path) {
            path.AddPath(gpath, true);
        }

        #endregion
    }
}
