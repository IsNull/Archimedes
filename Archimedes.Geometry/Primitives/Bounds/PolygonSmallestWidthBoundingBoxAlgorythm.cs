using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Archimedes.Geometry.Primitives.Bounds
{
    public class PolygonSmallestWidthBoundingBoxAlgorythm : PolygonSmallestBoundingBoxAlgorythm
    {
        Area? _bestRectDimension;

        protected override bool IsCurrentRectangleTheBest() {
            bool isBetter = false;

            var vx0 = _currentRectangle[0].X - _currentRectangle[1].X;
            var vy0 = _currentRectangle[0].Y - _currentRectangle[1].Y;
            var len0 = Math.Sqrt(vx0 * vx0 + vy0 * vy0);

            var vx1 = _currentRectangle[1].X - _currentRectangle[2].X;
            var vy1 = _currentRectangle[1].Y - _currentRectangle[2].Y;
            var len1 = Math.Sqrt(vx1 * vx1 + vy1 * vy1);
            
            var thisRectDim = Area.Create(len0, len1);

            if (_bestRectDimension.HasValue) {
                if (_bestRectDimension.Value.Width > thisRectDim.Width) {
                    _bestRectDimension = thisRectDim;
                    isBetter = true;
                }
            } else {
                _bestRectDimension = thisRectDim;
                isBetter = true;
            }
            return isBetter;
        }


        struct Area
        {
            private Area(double lenght, double width)
            {
                Lenght = lenght;
                Width = width;
            }

            private double Lenght;
            public double Width;

            /// <summary>
            /// Create the Area Struct from given 2 numbers
            /// </summary>
            /// <param name="n1"></param>
            /// <param name="n2"></param>
            /// <returns></returns>
            public static Area Create(double n1, double n2)
            {
                return (n1 >= n2) ? new Area(n1, n2) : new Area(n2, n1);
            }
        }

    }
}
