using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Extensions
{
    public static class PointListExtensions
    {
        public static List<Point> ToPointList(this List<Vector2> vertices) {
            var pnts = new List<Point>();
            foreach (var fp in vertices)
                pnts.Add(fp.ToPoint());
            return pnts;
        }
    }
}
