using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Extensions
{
    public static class RectangleExtensions
    {
        #region Area

        public static double Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }
        public static double Area(this RectangleF rect) {
            return rect.Width * rect.Height;
        }

        #endregion

        #region MiddlePoint

        public static Point MiddlePoint(this Rectangle oRect){
            return new Point(oRect.X + oRect.Width / 2, oRect.Y + oRect.Height / 2);
        }

        public static Vector2 MiddlePoint(this RectangleF oRect) {
            return new Vector2(oRect.X + oRect.Width / 2, oRect.Y + oRect.Height / 2);
        }
        #endregion

        public static Rectangle ToRectangle(this RectangleF rect) {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        #region ToVertices

        public static Vertices ToVertices(this Rectangle rect) {
            return ToVerticesConv(rect);
        }

        public static Vertices ToVertices(this RectangleF rect) {
            return ToVerticesConv(rect);
        }

        private static Vertices ToVerticesConv(RectangleF rect) {
            return new Vertices()
            {
                rect.Location,
                new Vector2(rect.Location.X + rect.Width, rect.Location.Y),
                new Vector2(rect.Location.X + rect.Width, rect.Location.Y + rect.Height),
                new Vector2(rect.Location.X, rect.Location.Y + rect.Height)
            };
        }

        #endregion

        #region BoundingCircle

        public static Circle2 BoundingCircle(this RectangleF rect) {
           var mp = rect.MiddlePoint();
           return new Circle2(mp, (float)Line2.CalcLenght(mp, rect.Location));
        }

        #endregion

        public static Rectangle2 ToGeometry(this Rectangle rect) {
            return new Rectangle2(rect);
        }
        public static Rectangle2 ToGeometry(this RectangleF rect) {
            return new Rectangle2(rect);
        }



    }
}
