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

        public Point StartPoint {
            get;
            set;
        }

        //public Pen Pen {
        //    get { return _pen; }
        //    set { _pen = value; }
        //}

        public VectorDrawer(Vector2 v, Point startPoint) {
            Vector = v;
            StartPoint = startPoint;
        }

        public VectorDrawer(Vector2 v, PointF startPoint) {
            Vector = v;
            StartPoint = new Point((int)startPoint.X, (int)startPoint.Y);
        }

        public void Draw(System.Drawing.Graphics G) {
            var line = new Line2(StartPoint, Vector.GetPoint(StartPoint));
            line.Pen = _thisPen;
            line.Draw(G);
        }
    }
}
