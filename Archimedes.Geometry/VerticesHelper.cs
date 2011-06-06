using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    public static class VerticesHelper
    {
        /// <summary>
        /// Rotates given vertices around a origin with given angle. New Vertices are returned.
        /// </summary>
        /// <param name="vertices">Array of Points</param>
        /// <param name="rotationOrigin">Origin location of rotation</param>
        /// <param name="angle">Rotation angle</param>
        /// <returns>New vertices array</returns>
        public static IEnumerable<PointF> RotateVertices(IEnumerable<PointF> vertices, PointF rotationOrigin, float angle) {
            Vector2 vrotate;
            var cnt = vertices.Count();
            var rotVertices = new List<PointF>();
            foreach (var vertex in vertices) {
                vrotate = new Vector2(rotationOrigin, vertex);
                var pnt = vrotate.GetRotated(angle).GetPoint(rotationOrigin);
                rotVertices.Add(pnt.ToPoint());
            }
            return rotVertices;
        }

        /// <summary>
        /// Finds the boundingbox (min/max vertex) of a collection from vertices.
        /// </summary>
        /// <param name="vertices">Collection of vertices</param>
        /// <returns></returns>
        public static RectangleF BoundingBox(IEnumerable<PointF> vertices) {

            if (vertices.Count() == 0)
                return new RectangleF();

            var minX = (from v in vertices
                        orderby v.X ascending
                        select v.X).First();

            var maxX = (from v in vertices
                        orderby v.X descending
                        select v.X).First();

            var minY = (from v in vertices
                        orderby v.Y ascending
                        select v.Y).First();

            var maxY = (from v in vertices
                        orderby v.Y descending
                        select v.Y).First();

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Builds Line-Paths from vertices - <example>3 <paramref name="vertices"/> give 2 Lines</example>
        /// </summary>
        /// <param name="vertices">Sorted vertices</param>
        /// <returns>Line(s)</returns>
        public static IEnumerable<Line2> VerticesPathToLines(IEnumerable<PointF> vertices) {
            var segments = new List<Line2>();
            var startPoint = new PointF();
            var endPoint = new PointF();
            int i = 0;
            foreach (var uP in vertices) {
                i++;
                if (i == 1) {
                    startPoint = uP;
                    continue;
                } else
                    endPoint = uP;

                if (startPoint != null && endPoint != null) {
                    segments.Add(new Line2(startPoint, endPoint));
                }
                startPoint = endPoint;
            }
            return segments;
        }

        /// <summary>
        /// Sorts the vertices X and y oriented
        /// </summary>
        /// <param name="vertices"></param>
        public static void SortVertices(List<PointF> vertices) {
            vertices.Sort(delegate(PointF p1, PointF p2)
            {
                int dx = p1.X.CompareTo(p2.X);
                if (dx != 0) {
                    return dx;
                } else {
                    return p1.Y.CompareTo(p2.Y);
                }
            });
        }



    }
}
