using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    interface IClosedGeometry
    {
        float Area { get; }
        Brush FillBrush { get; set; }
        Polygon2 ToPolygon2();
    }
}
