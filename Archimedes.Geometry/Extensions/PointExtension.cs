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
            newPnt.X = pntF.X + (float)(size * Math.Cos(MathHelper.ToRadians(angle)));
            newPnt.Y = pntF.Y + (float)(size * Math.Sin(MathHelper.ToRadians(angle)));
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
        public static Point ToPoint(this Vector2 pntF) {
            return new Point((int)Math.Round(pntF.X), (int)Math.Round(pntF.Y));
        }

        public static Vector2 Scale(this Vector2 pntF, float factor) {
            return new Vector2(pntF.X * factor, pntF.Y * factor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pntF"></param>
        /// <param name="size"></param>
        /// <param name="angle">Angle in degree</param>
        /// <returns></returns>
        public static Vector2 GetPoint(this Vector2 pntF, float size, float angle) {
            var newPnt = new Vector2();
            newPnt.X = pntF.X + (float)(size * Math.Cos(MathHelper.ToRadians(angle)));
            newPnt.Y = pntF.Y + (float)(size * Math.Sin(MathHelper.ToRadians(angle)));
            return newPnt;
        }

        public static Vector2 Round(this Vector2 pntF, int value) {
            var newPnt = new Vector2();
            newPnt.X = (float)Math.Round(pntF.X, value);
            newPnt.Y = (float)Math.Round(pntF.Y, value);
            return newPnt;
        }
    }
}
