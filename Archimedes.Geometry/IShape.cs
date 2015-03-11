using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    /// <summary>
    /// Represents a shape (a closed geometry)
    /// </summary>
    public interface IShape : IGeometry
    {
        /// <summary>
        /// Gets the area of this geometry shape
        /// </summary>
        double Area { get; }

        /// <summary>
        /// Turns this shape into a polygon
        /// </summary>
        /// <returns></returns>
        Polygon2 ToPolygon2();


        /// <summary>
        /// The fill brush
        /// </summary>
        Brush FillBrush { get; set; } // TODO refactor drawing stuff away...
    }
}
