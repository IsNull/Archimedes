using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry
{
    /// <summary>
    /// Represents a point where the components are of type double
    /// </summary>
    public struct PointD
    {
        public readonly double X;
        public readonly double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
