using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Rendering.Primitives
{
    public class VisualRectangle : Visual
    {
        private readonly Rectangle2 _rectangle;
        public VisualRectangle(Rectangle2 geometry)
        {
            _rectangle = geometry;
        }

        public override IGeometryBase Geometry
        {
            get { return _rectangle; }
        }

        public override void Draw(Graphics g)
        {
            try
            {
                var path = new GraphicsPath();
                path.AddPolygon(_rectangle.ToVertices().ToPointArray());
                if (this.FillBrush != null)
                    g.FillPath(this.FillBrush, path);
                g.DrawPath(Pen, path);
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}
