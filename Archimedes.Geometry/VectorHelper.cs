using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    public static class VectorHelper
    {
        public static Vector2 GetEndVector(IGeometryBase g) {
            Vector2 v;
            if (g is Line2) {
                v = (g as Line2).ToVector();
            } else if (g is Arc) {
                v = (g as Arc).EndVector;
            } else
                throw new NotSupportedException("Can't get End-Vector from IGeometryBase: " + g.GetType().ToString());
            return v;
        }
    }
}
