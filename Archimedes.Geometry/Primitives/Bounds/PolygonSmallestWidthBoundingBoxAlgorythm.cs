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
        Aerea? _bestRectDimension;

        protected override bool IsCurrentRectangleTheBest() {
            bool isBetter = false;

            float vx0 = _currentRectangle[0].X - _currentRectangle[1].X;
            float vy0 = _currentRectangle[0].Y - _currentRectangle[1].Y;
            float len0 = (float)Math.Sqrt(vx0 * vx0 + vy0 * vy0);

            float vx1 = _currentRectangle[1].X - _currentRectangle[2].X;
            float vy1 = _currentRectangle[1].Y - _currentRectangle[2].Y;
            float len1 = (float)Math.Sqrt(vx1 * vx1 + vy1 * vy1);
            
            Aerea thisRectDim = Aerea.Create(len0, len1);

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


        struct Aerea
        {
            public Aerea(float lenght, float width) {
                Lenght = lenght;
                Width = width;
            }
            public float Lenght;
            public float Width;

            /// <summary>
            /// Create the Aerea Struct from given 2 numbers
            /// </summary>
            /// <param name="n1"></param>
            /// <param name="n2"></param>
            /// <returns></returns>
            public static Aerea Create(float n1, float n2) {
                return (n1 >= n2) ? new Aerea(n1, n2) : new Aerea(n2, n1);
            }
        }

    }
}
