using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Primitives
{
    public partial class Arc : IGeometryBase
    {
        /// <summary> 
        /// Create Arc from 3 given Points
        /// </summary>
        /// <param name="startPoint">Start-Point of the Arc</param>
        /// <param name="interPoint">A point anywhere the Arc</param>
        /// <param name="endPoint">End-Point of the Arc</param>
        /// <returns></returns>
        private static Arc FromDescriptorPoints(Vector2 startPoint, Vector2 interPoint, Vector2 endPoint) {

            var calcdirection = Direction.RIGHT;

            // Calculate Rays from the 3 given Points
            var rays = RaysFromDescriptorPoints(startPoint, interPoint, endPoint, DirectionUtil.Switch(calcdirection));
            // The two Rays intercept in the Arc's Middlepoint:
            var arcCenter = rays[0].Intersect(rays[1]);
            var arcRadius = new Vector2(startPoint, arcCenter).Length;

            // Take Vectors from these Points
            var vMPToStartPoint = new Vector2(arcCenter, startPoint);
            var vMPToEndPoint = new Vector2(arcCenter, endPoint);

            // Calculate base vector
            var vbase = vMPToStartPoint.GetOrthogonalVector(Direction.RIGHT) * -1;
            var newArc = new Arc(arcRadius, vMPToEndPoint.GetAngleBetweenClockWise(vMPToStartPoint, calcdirection), null, vbase)
            {
             Location = startPoint,
             Direction = DirectionUtil.Switch(calcdirection)
            };

            return newArc;
        }


        /// <summary> 
        /// Get 2 rays from 3 points. The Rays interception Point is the Middlepoint of the Arc
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="interPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private static Ray[] RaysFromDescriptorPoints(Vector2 startPoint, Vector2 interPoint, Vector2 endPoint, Direction direction) {

            Ray[] rays = new Ray[2];

            Vector2 vRay1 = new Vector2(startPoint, interPoint).GetOrthogonalVector(direction);    //Direction doesn't matter !?
            Vector2 vRay2 = new Vector2(interPoint, endPoint).GetOrthogonalVector(direction);      //Direction doesn't matter !?

            var ray1StartPoint = new Line2(startPoint, interPoint).MiddlePoint;
            var ray2StartPoint = new Line2(interPoint, endPoint).MiddlePoint;

            rays[0] = new Ray(vRay1, ray1StartPoint);
            rays[1] = new Ray(vRay2, ray2StartPoint);
            return rays;
        }



    }
}
