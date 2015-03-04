using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    public static class MathHelper
    {

        public static Matrix FlipY {
            get {
                return new Matrix(1, 0, 0,
                                 -1, 0, 0);
            }
        }
        public static Matrix FlipX {
            get {
                return new Matrix(-1, 0, 0,
                                   1, 0, 0);
            }
        }


        public static void GetStartEndPoint(IGeometryBase geometry, out Point? start, out Point? end) {

            start = null;
            end = null;

            if (geometry is Line2) {
                var line = geometry as Line2;
                start = line.Start.ToPoint(); ;
                end = line.End.ToPoint();
            } else if (geometry is Arc) {
                var arc = geometry as Arc;
                start = arc.Location.ToPoint();
                end = arc.EndPoint.ToPoint();
            }
        }



    }
}
