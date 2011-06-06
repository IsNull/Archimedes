using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Archimedes.Geometry.Primitives.Bounds
{
    /// <summary>
    /// Algorythm which tries to find the smallest (Aera) BoundingBox Rectangle of the given Polygon
    /// </summary>
    public class PolygonSmallestBoundingBoxAlgorythm : PolygonBoundingBoxAlgorythm
    {
        #region Fields

        protected int _numPoints = 0;

        // The points that have been used in test edges.
        protected bool[] _edgeChecked;

        // The four caliper control points. They start:
        //       m_ControlPoints(0)      Left edge       xmin
        //       m_ControlPoints(1)      Bottom edge     ymax
        //       m_ControlPoints(2)      Right edge      xmax
        //       m_ControlPoints(3)      Top edge        ymin
        private int[] _controlPoints = new int[4];

        // The line from this point to the next one forms
        // one side of the next bounding rectangle.
        protected int currentControlPoint = -1;

        // The area of the current and best bounding rectangles.
        private float _currentArea = float.MaxValue;
        protected PointF[] _currentRectangle = null;
        private float _bestArea = float.MaxValue;
        protected PointF[] _bestRectangle = null;

        #endregion

        #region Constructor

        public PolygonSmallestBoundingBoxAlgorythm() { }
        
        public PolygonSmallestBoundingBoxAlgorythm(Polygon2 poly) {
            SetPolygon(poly);
        }

        #endregion

        #region Public Methods

        public override void SetPolygon(Polygon2 poly) {

            _polygon = poly.Clone() as Polygon2;

            // This algorithm assumes the polygon
            // is oriented counter-clockwise.
            _polygon.OrientCounterClockwise();
            //Its very important that we not have any points more than once - distinct
            _vertices = _polygon.ToVertices().Distinct().ToArray();
        }

        /// <summary>
        /// Find a smallest bounding rectangle.
        /// </summary>
        /// <returns></returns>
        public override PointF[] FindBounds() {

            if (_polygon == null)
                throw new ArgumentNullException();

            if (_vertices.Count() == 0)
                return new PointF[0];

            // This algorithm assumes the polygon
            // is oriented counter-clockwise.
            Debug.Assert(!_polygon.IsOrientedClockwise());

            // Get ready;
            ResetBoundingRect();

            // Check all possible bounding rectangles.
            for (int i = 0; i < _vertices.Count(); i++) {
                CheckNextBoundingRectangle();
            }

            // Return the best result.
            return _bestRectangle;
        }

        #endregion

        #region Private Methods

        // Get ready to start.
        private void ResetBoundingRect() {
            _numPoints = _vertices.Count();

            // Find the minimum and maximum points
            // in all four directions.
            float minx = _vertices[0].X;
            float maxx = minx;
            float miny = _vertices[0].Y;
            float maxy = miny;
            float minxi = 0;
            float maxxi = 0;
            float minyi = 0;
            float maxyi = 0;

            for (int i = 1; i < _numPoints; i++) {
                if (minx > _vertices[i].X) {
                    minx = _vertices[i].X;
                    minxi = i;
                }
                if (maxx < _vertices[i].X) {
                    maxx = _vertices[i].X;
                    maxxi = i;
                }
                if (miny > _vertices[i].Y) {
                    miny = _vertices[i].Y;
                    minyi = i;
                }
                if (maxy < _vertices[i].Y) {
                    maxy = _vertices[i].Y;
                    maxyi = i;
                }
            }

            _controlPoints[0] = (int)minxi;
            _controlPoints[1] = (int)maxyi;
            _controlPoints[2] = (int)maxxi;
            _controlPoints[3] = (int)minyi;
            currentControlPoint = -1;

            // Reset the current and best bounding rectangle.
            _currentRectangle = new PointF[] 
            { 
                new PointF(minx, miny),
                new PointF(maxx, miny),
                new PointF(maxx, maxy),
                new PointF(minx, maxy),
            };
            _currentArea = (maxx - minx) * (maxy - miny);
            _bestRectangle = _currentRectangle;
            _bestArea = _currentArea;

            // So far we have not checked any edges.
            _edgeChecked = new bool[_numPoints];
            for (int i = 0; i < _numPoints; i++) {
                _edgeChecked[i] = false;
            }
        }

        // Find the next bounding rectangle and check it.
        private void CheckNextBoundingRectangle() {
            // Increment the current control point.
            // This means we are done with using this edge.
            if (currentControlPoint >= 0) {
                _controlPoints[currentControlPoint] =
                    (_controlPoints[currentControlPoint] + 1) % _numPoints;
            }

            // Find the next point on an edge to use.
            float xmindx = 0, xmindy = 0, ymaxdx = 0, ymaxdy = 0;
            float xmaxdx = 0, xmaxdy = 0, ymindx = 0, ymindy = 0;
            FindDxDy(ref xmindx, ref xmindy, _controlPoints[0]);
            FindDxDy(ref ymaxdx, ref ymaxdy, _controlPoints[1]);
            FindDxDy(ref xmaxdx, ref xmaxdy, _controlPoints[2]);
            FindDxDy(ref ymindx, ref ymindy, _controlPoints[3]);

            // Switch so we can look for the smallest opposite/adjacent ratio.
            float xminopp = xmindx;
            float xminadj = xmindy;
            float ymaxopp = -ymaxdy;
            float ymaxadj = ymaxdx;
            float xmaxopp = -xmaxdx;
            float xmaxadj = -xmaxdy;
            float yminopp = ymindy;
            float yminadj = -ymindx;

            // Pick initial values that will make every point an improvement.
            float bestopp = 1;
            float bestadj = 0;
            int best_control_point = -1;

            // Pick the best control point to use next.
            if ((xminopp >= 0) && (xminadj >= 0)) {
                if (xminopp * bestadj < bestopp * xminadj) {
                    bestopp = xminopp;
                    bestadj = xminadj;
                    best_control_point = 0;
                }
            }
            if ((ymaxopp >= 0) && (ymaxadj >= 0)) {
                if (ymaxopp * bestadj < bestopp * ymaxadj) {
                    bestopp = ymaxopp;
                    bestadj = ymaxadj;
                    best_control_point = 1;
                }
            }
            if ((xmaxopp >= 0) && (xmaxadj >= 0)) {
                if (xmaxopp * bestadj < bestopp * xmaxadj) {
                    bestopp = xmaxopp;
                    bestadj = xmaxadj;
                    best_control_point = 2;
                }
            }
            if ((yminopp >= 0) && (yminadj >= 0)) {
                if (yminopp * bestadj < bestopp * yminadj) {
                    bestopp = yminopp;
                    bestadj = yminadj;
                    best_control_point = 3;
                }
            }


            // Make sure we found a usable edge.
            if (best_control_point < 0) {
                return;
            }

            // Use the new best control point.
            currentControlPoint = best_control_point;

            // Remember that we have checked this edge.
            if (currentControlPoint != -1)
                _edgeChecked[_controlPoints[currentControlPoint]] = true;

            // Find the current bounding rectangle
            // and see if it is an improvement.
            FindBoundingRectangle();
        }


        /// <summary>
        /// Find the current bounding rectangle and
        /// see if it is better than the previous best.
        /// </summary>
        private void FindBoundingRectangle() {
            // See which point has the current edge.
            int i1 = _controlPoints[currentControlPoint];
            int i2 = (i1 + 1) % _numPoints;
            float dx = _vertices[i2].X - _vertices[i1].X;
            float dy = _vertices[i2].Y - _vertices[i1].Y;

            // Make dx and dy work for the first line.
            switch (currentControlPoint) {
                case 0: // Nothing to do.
                    break;
                case 1: // dx = -dy, dy = dx
                    float temp1 = dx;
                    dx = -dy;
                    dy = temp1;
                    break;
                case 2: // dx = -dx, dy = -dy
                    dx = -dx;
                    dy = -dy;
                    break;
                case 3: // dx = dy, dy = -dx
                    float temp2 = dx;
                    dx = dy;
                    dy = -temp2;
                    break;
            }

            float px0 = _vertices[_controlPoints[0]].X;
            float py0 = _vertices[_controlPoints[0]].Y;
            float dx0 = dx;
            float dy0 = dy;
            float px1 = _vertices[_controlPoints[1]].X;
            float py1 = _vertices[_controlPoints[1]].Y;
            float dx1 = dy;
            float dy1 = -dx;
            float px2 = _vertices[_controlPoints[2]].X;
            float py2 = _vertices[_controlPoints[2]].Y;
            float dx2 = -dx;
            float dy2 = -dy;
            float px3 = _vertices[_controlPoints[3]].X;
            float py3 = _vertices[_controlPoints[3]].Y;
            float dx3 = -dy;
            float dy3 = dx;

            // Find the points of intersection.
            _currentRectangle = new PointF[4];
            FindIntersection(px0, py0, px0 + dx0, py0 + dy0, px1, py1, px1 + dx1, py1 + dy1, ref _currentRectangle[0]);
            FindIntersection(px1, py1, px1 + dx1, py1 + dy1, px2, py2, px2 + dx2, py2 + dy2, ref _currentRectangle[1]);
            FindIntersection(px2, py2, px2 + dx2, py2 + dy2, px3, py3, px3 + dx3, py3 + dy3, ref _currentRectangle[2]);
            FindIntersection(px3, py3, px3 + dx3, py3 + dy3, px0, py0, px0 + dx0, py0 + dy0, ref _currentRectangle[3]);

            if (IsCurrentRectangleTheBest()) {
                _bestRectangle = _currentRectangle;
            }
        }

        /// <summary>
        /// See if this is the best bounding rectangle so far.
        /// Get the area of the bounding rectangle.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsCurrentRectangleTheBest() {

            float vx0 = _currentRectangle[0].X - _currentRectangle[1].X;
            float vy0 = _currentRectangle[0].Y - _currentRectangle[1].Y;
            float len0 = (float)Math.Sqrt(vx0 * vx0 + vy0 * vy0);

            float vx1 = _currentRectangle[1].X - _currentRectangle[2].X;
            float vy1 = _currentRectangle[1].Y - _currentRectangle[2].Y;
            float len1 = (float)Math.Sqrt(vx1 * vx1 + vy1 * vy1);

            // See if this is an improvement.
            _currentArea = len0 * len1;
            if (_currentArea < _bestArea) {
                _bestArea = _currentArea;
                return true;
            } else
                return false;
        }



        /// <summary>
        /// Find the slope of the edge from point i to point i + 1.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="i"></param>
        private void FindDxDy(ref float dx, ref float dy, int i) {
            int i2 = (i + 1) % _numPoints;
            dx = _vertices[i2].X - _vertices[i].X;
            dy = _vertices[i2].Y - _vertices[i].Y;
        }

        /// <summary>
        /// Find the point of intersection between two lines.
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <param name="A1"></param>
        /// <param name="B1"></param>
        /// <param name="A2"></param>
        /// <param name="B2"></param>
        /// <param name="intersect"></param>
        /// <returns></returns>
        private bool FindIntersection(float X1, float Y1, float X2, float Y2, float A1, float B1, float A2, float B2, ref PointF intersect) {
            float dx = X2 - X1;
            float dy = Y2 - Y1;
            float da = A2 - A1;
            float db = B2 - B1;
            float s, t;

            // If the segments are parallel, return False.
            if (Math.Abs(da * dy - db * dx) < 0.001) return false;

            // Find the point of intersection.
            s = (dx * (B1 - Y1) + dy * (X1 - A1)) / (da * dy - db * dx);
            t = (da * (Y1 - B1) + db * (A1 - X1)) / (db * dx - da * dy);
            intersect = new PointF(X1 + t * dx, Y1 + t * dy);
            return true;
        }

        #endregion
    }
}
