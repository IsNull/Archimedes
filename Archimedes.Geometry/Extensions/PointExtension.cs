using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Units;

namespace Archimedes.Geometry.Extensions
{
    public static class PointExtension
    {
        public static Point ToPoint(this PointF pntF) {
            return new Point((int)Math.Round(pntF.X), (int)Math.Round(pntF.Y));
        }

        public static PointF Scale(this PointF pntF, double factor)
        {
            return new PointF((float)(pntF.X * factor), (float)(pntF.Y * factor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pntF"></param>
        /// <param name="size"></param>
        /// <param name="angle">Rotation in degree</param>
        /// <returns></returns>
        public static PointF GetPoint(this PointF pntF, double size, Angle angle)
        {
            var newPnt = new PointF();
            newPnt.X = (float)(pntF.X + size * Math.Cos(angle.Radians));
            newPnt.Y = (float)(pntF.Y + size * Math.Sin(angle.Radians));
            return newPnt;
        }

        public static PointF Round(this Vector2 pntF, int value) {
            var newPnt = new PointF();
            newPnt.X = (float)Math.Round(pntF.X, value);
            newPnt.Y = (float)Math.Round(pntF.Y, value);
            return newPnt;
        }
    }

    public static class VectorExtension
    {
        public static Point ToPoint(this Vector2 pntF)
        {
            return new Point((int) Math.Round(pntF.X), (int) Math.Round(pntF.Y));
        }
    }
}
