using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    public static class Common
    {

        public static Matrix FlipY {
            get { return new Matrix(1, 0, 0,
                                   -1, 0, 0); }
        }
        public static Matrix FlipX {
            get { return new Matrix(-1, 0, 0,
                                     1, 0, 0); }
        }


        /// <summary>
        /// To convert a degrees value to radians, multiply it by pi/180 (approximately 0.01745329252).
        /// 
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static float Degree2RAD(float degree) {
            return (float)(degree * Math.PI / 180);
        }

        /// <summary>
        ///  To convert a radians value to degrees, multiply it by 180/pi (approximately 57.29578).
        /// </summary>
        /// <param name="RAD"></param>
        /// <returns></returns>
        public static float RAD2Degree(float RAD) {
            return (float)(RAD * 180 / Math.PI);
        }

        /// <summary>Switches a direction
        /// 
        /// </summary>
        /// <param name="uDirection"></param>
        /// <returns></returns>
        public static Direction Switch(Direction uDirection) {
            return (uDirection == Direction.LEFT) ? Direction.RIGHT : Direction.LEFT;
        }

        /// <summary>
        /// Get -1 or 1, depending on the radius Direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static int Mul(Direction direction) {
            return (direction == Direction.LEFT) ? 1 : -1;
        }

        /// <summary>
        /// Get the direction depending on positive/negative number
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction UnMul(double mul) {
            return (mul > 0) ? Direction.LEFT : Direction.RIGHT;
        }


        public static void GetStartEndPoint(IGeometryBase geometry, out Point? start, out Point? end) {

            start = null;
            end = null;

            if (geometry is Line2) {
                var line = geometry as Line2;
                start = line.P1.ToPoint(); ;
                end = line.P2.ToPoint();
            } else if (geometry is Arc) {
                var arc = geometry as Arc;
                start = arc.Location.ToPoint();
                end = arc.EndPoint.ToPoint();
            }
        }



    }
}
