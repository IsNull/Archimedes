using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Rendering.Primitives
{
    /// <summary>
    /// This factory helps to construct visuals from geometry objects.
    /// </summary>
    public static class VisualFactory
    {

        public static VisualPoint Create(Vector2 point, Brush brush)
        {
            return new VisualPoint(point)
            {
                FillBrush = brush
            };
        }

        public static VisualLine Create(Line2 line, Pen pen = null)
        {
            return new VisualLine(line)
            {
                Pen = pen
            };
        }

        public static VisualArc Create(Arc arc, Pen pen = null)
        {
            return new VisualArc(arc)
            {
                Pen = pen
            };
        }

        public static VisualCircle Create(Circle2 circle, Pen pen = null, Brush brush = null)
        {
            return new VisualCircle(circle)
            {
                Pen = pen,
                FillBrush = brush
            };
        }

        public static VisualPolygon Create(Polygon2 poly, Pen pen = null, Brush brush = null)
        {
            return new VisualPolygon(poly)
            {
                Pen = pen,
                FillBrush = brush
            };
        }

        public static VisualRectangle Create(Rectangle2 rect, Pen pen = null, Brush brush = null)
        {
            return new VisualRectangle(rect)
            {
                Pen = pen,
                FillBrush = brush
            };
        }

        public static VisualLineString Create(LineString lineString, Pen pen = null)
        {
            return new VisualLineString(lineString)
            {
                Pen = pen
            };
        }
    }
}
