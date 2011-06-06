using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Extensions
{
    public static class PointExtension
    {
        public static Point ToPoint(this PointF pntF) {
            return new Point((int)Math.Round(pntF.X), (int)Math.Round(pntF.Y));
        }

        public static PointF Scale(this PointF pntF, float factor) {
            return new PointF(pntF.X * factor, pntF.Y * factor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pntF"></param>
        /// <param name="size"></param>
        /// <param name="angle">Angle in degree</param>
        /// <returns></returns>
        public static PointF GetPoint(this PointF pntF, float size, float angle) {
            var newPnt = new PointF();
            newPnt.X = pntF.X + (float)(size * Math.Cos(Common.Degree2RAD(angle)));
            newPnt.Y = pntF.Y + (float)(size * Math.Sin(Common.Degree2RAD(angle)));
            return newPnt;
        }

        public static PointF Round(this PointF pntF, int value) {
            var newPnt = new PointF();
            newPnt.X = (float)Math.Round(pntF.X, value);
            newPnt.Y = (float)Math.Round(pntF.Y, value);
            return newPnt;
        }


    }
}
