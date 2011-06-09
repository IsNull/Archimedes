using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    public class VectorDrawer : IDrawable
    {
        static readonly Pen _thisPen = new Pen(Brushes.Black)
        {
            EndCap = LineCap.ArrowAnchor
        };


        public Vector2 Vector {
            get;
            set;
        }

        public Vector2 StartPoint {
            get;
            set;
        }

        public VectorDrawer(Vector2 v, Vector2 startPoint) {
            Vector = v;
            StartPoint = new Vector2(startPoint.X, startPoint.Y);
        }

        public void Draw(System.Drawing.Graphics G) {
            var line = new Line2(StartPoint, StartPoint + Vector);
            line.Pen = _thisPen;
            line.Draw(G);
        }
    }
}
