using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Primitives.Bounds
{
    /// <summary>
    /// Base Class for all polygon-based BoundingBox finder Algorythms
    /// This Class is abstract
    /// </summary>
    public abstract class PolygonBoundingBoxAlgorythm
    {
        protected Polygon2 _polygon;
        protected Vector2[] _vertices;

        /// <summary>
        /// Set the Poligon to process.
        /// </summary>
        /// <param name="poly"></param>
        public virtual void SetPolygon(Polygon2 poly) {
            _polygon = poly.Clone() as Polygon2;
            _vertices = _polygon.ToVertices().Distinct().ToArray();
        }

        /// <summary>
        /// Process the set Polygon with the underlying algorythm and find the Boundingbox.
        /// </summary>
        /// <returns>Returns the Boundingbox as Array of 4 Points</returns>
        public abstract Vector2[] FindBounds();
    }
}
