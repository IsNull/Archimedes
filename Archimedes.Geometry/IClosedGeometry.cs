using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    /// <summary>
    /// Represents a closed geometry
    /// </summary>
    public interface IClosedGeometry
    {
        /// <summary>
        /// Gets the area of this geometry shape
        /// </summary>
        double Area { get; }

        Brush FillBrush { get; set; }

        Polygon2 ToPolygon2();
    }
}
