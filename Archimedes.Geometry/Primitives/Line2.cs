/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/
using System;
using System.Drawing;
using System.Collections.Generic;
using Archimedes.Geometry.Extensions;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Rendering;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Represents a Line in a 2D coord space defined by two points.
    /// </summary>
    public partial class Line2 : IGeometryBase
    {
        
        #region Fields

        Vector2 _start;
        Vector2 _end;

        Pen _pen = null;
        bool _penSharedResource = false;

        #endregion

        #region Static Builder

        public static Line2 Parse(string value)
        {
            Line2 line;
            var vertices = Vector2.ParseAll(value);
            if (vertices.Length == 2)
            {
                line = new Line2(vertices[0], vertices[1]);
            }
            else
            {
                throw new ArgumentOutOfRangeException("value", 
                    "Expected where 2 vertices to build a line, but got: " +vertices.Length + " - Parsed from '" + value+"'");
            }
            return line;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty line
        /// </summary>
        public Line2() { }

        /// <summary>
        /// Creates a new horizontal Line, starting from 0,0 with the given Length
        /// </summary>
        /// <param name="lenght">Lenght of the new Line</param>
        public Line2(double lenght) {
            _start = Vector2.Zero;
            _end = new Vector2(lenght, 0);
        }

        /// <summary>
        /// Creates a Line between two given Points represented by Vectors
        /// </summary>
        /// <param name="start">Startpoint of the Line</param>
        /// <param name="end">Endpoint of the Line</param>
        public Line2(Vector2 start, Vector2 end) {
            _start = start;
            _end = end;
        }

        public Line2(Vector2 uP1, Vector2 uP2, Pen uPen) {
            _start = uP1;
            _end = uP2;
            this.Pen = uPen;
        }

        public Line2(double uP1x, double uP1y, double uP2x, double uP2y)
        {
            _start = new Vector2(uP1x, uP1y);
            _end = new Vector2(uP2x, uP2y);
        }

        public Line2(Line2 prototype) {
            Prototype(prototype);
        }

        public virtual void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as Line2;
            if (prototype == null)
                throw new NotImplementedException();

            this.Start = prototype.Start;
            this.End = prototype.End;
            try {
                if (prototype.Pen != null)
                    this.Pen = prototype.Pen.Clone() as Pen;
            } catch {

            }
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// Get/Set the Startpoint of this Line
        /// </summary>
        public Vector2 Start {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Get/Set the Endpoint of this Line
        /// </summary>
        public Vector2 End {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary> 
        /// Return the q (movement from x axis). If slope isn't defined, this property is Zero.
        /// </summary>
        public double YMovement {
            get {   // q = y1 - m * x1 
                if (this.IsVertical) {
                    return 0;
                }
                else {
                    return this.Start.Y - (this.Slope * this.Start.X);
                }
            }
        }

        /// <summary>
        /// Returns the solpe of the line. Returns Zero if the slope isn't defined.
        /// </summary>
        public double Slope {
            get
            {
                return ToVector().Slope;
            }
        }

        /// <summary>
        /// Extends this line in the given direction
        /// </summary>
        /// <param name="len"></param>
        /// <param name="direction"></param>
        public void Stretch(double len, Direction direction){
            var vThis = ToVector();

            if (direction == Direction.RIGHT) {
                vThis.Length += len;
                this.End = this.Start + vThis;
            } else {
                vThis.Length = len;
                vThis *= -1;
                this.Start += vThis;
            }
        }

        /// <summary>
        /// Returns the lenght of this line
        /// </summary>
        public double Lenght {
            get { return CalcLenght(this.Start, this.End); }
        }

        /// <summary>
        /// Determites if the given Point/Vertex lies on the Left side of this Line
        /// </summary>
        /// <param name="point">The point to test</param>
        /// <returns></returns>
        public bool IsLeft(Vector2 point) {
            return IsLeft(this.Start, this.End, point);
        }

        /// <summary>
        /// Calculates if the given point is on the left side of the given line
        /// </summary>
        /// <param name="start">Start point of the line</param>
        /// <param name="end">End point of the line</param>
        /// <param name="point">The point to test</param>
        /// <returns></returns>
        static bool IsLeft(Vector2 start, Vector2 end, Vector2 c)
        {
            return ((end.X - start.X) * (c.Y - start.Y) - (end.Y - start.Y) * (c.X - start.X)) > 0;
        }

        /// <summary>
        /// Calculates the distance between two Points
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double CalcLenght(Vector2 start, Vector2 end) { // TODO Rename to distance()
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
        }
      

        /// <summary>
        /// Compare the slope of two lines  
        /// </summary>
        /// <param name="line"></param>
        /// <param name="tolerance"></param>
        /// <returns>true, if the solpes are equal</returns>
        public bool IsParallelTo(Line2 line, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            return ToVector().IsParallelTo(line.ToVector(), tolerance);
        }

        /// <summary>
        /// Checks if this line has a congruent overlap with the given.
        /// TODO Probably change this method to return the intersectin line segment
        /// 
        /// </summary>
        /// <param name="line2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool IsCongruentTo(Line2 line2, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            if (IsParallelTo(line2, tolerance))
            {
                return (this.Contains(line2.Start, tolerance) || this.Contains(line2.End, tolerance));
            }
            return false;
        }

        /// <summary>
        /// Is this Line vertical?
        /// </summary>
        public bool IsVertical {
            get
            {
                return ToVector().IsVertical;
            }
        }

        /// <summary>
        /// Is this Line horizontal?
        /// </summary>
        public bool IsHorizontal {
            get
            {
                return ToVector().IsHorizontal;
            }
        }

        /// <summary>
        /// Is this Line horizontal or vertical?
        /// </summary>
        public bool IsHorOrVert {
            get {
                return this.IsHorizontal || this.IsVertical;
            }
        }


        #endregion

        #region Specail Transformators

        /// <summary>
        /// Scale the line by given scale factor
        /// </summary>
        /// <param name="factor">Scale Factor</param>
        public void Scale(double factor) {
            this.Start = Start.Scale(factor);
            this.End = End.Scale(factor);
        }

        /// <summary>
        /// Explodes a Rectangle into the 4 border lines and returns an array of lines
        /// </summary>
        /// <param name="rect"></param>
        /// <returns>Line2d List<></returns>
        public static Line2[] RectExplode(AARectangle rect)
        {
            var sides = new Line2[4];
            sides[0] = new Line2(rect.X, rect.Y, rect.X + rect.Width, rect.Y);                                 // upper horz line
            sides[1] = new Line2(rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);     // lower horz line
            sides[2] = new Line2(rect.X, rect.Y, rect.X, rect.Y + rect.Height);                                // left  vert line
            sides[3] = new Line2(rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);      // right  vert line
            return sides;
        }

        /// <summary>
        /// Buils lines from the vertices
        /// </summary>
        /// <param name="vertices">The vertices - they must be ordered as the lines shall be created</param>
        /// <returns></returns>
        public static IEnumerable<Line2> LinesFromVertices(Vertices vertices)
        {
            if (vertices.Count > 1)
            {
                for (int i = 0; i < vertices.Count - 1; i++)
                {
                    yield return new Line2(vertices[i], vertices[i + 1]);
                }
            }
        }


        public Vector2 ToVector() {
            return new Vector2(this.Start, this.End);
        }

        public Vertices ToVertices() {
            return new Vertices{ this.Start, this.End };
        }

        #endregion

        #region Geomerty Base

        public void AddToPath(GraphicsPath path) {
            path.AddLine(this.Start, this.End);
        }

        public Vector2 Location {
            get {
                return this.Start;
            }
            set {
                Translate(new Vector2(this.Start, value));
            }
        }

        public Vector2 MiddlePoint {
            get {
                Vector2 vLine = (new Vector2(Start, End)) / 2;
                return Start + vLine;
            }
            set {
                Translate(new Vector2(this.MiddlePoint, value));
            }
        }

        public void Translate(Vector2 mov) {
            this.Start += mov;
            this.End += mov;
        }

        /// <summary>
        /// Creates a new Line, clone of the origin but moved by given vector
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public static Line2 CreateMoved(Line2 origin, Vector2 move) {
            var clone = origin.Clone() as Line2;
            clone.Translate(move);
            return clone;
        }


        public IGeometryBase Clone() {
            return new Line2(this);
        }

        public AARectangle BoundingBox {
	        get { 
                Vector2 start;
                if (Start.X > End.X) {
                    start = Start;
                }else if (Start.X == End.X){
                    if(Start.Y < End.Y)
                        start = Start;
                    else
                        start = End;
                }else
                    start = End;

                return new AARectangle(start, new SizeD(Math.Abs(Start.X - End.X), Math.Abs(Start.Y - End.Y)));
            }
        }

        public Circle2 BoundingCircle {
            get { 
                var m = this.MiddlePoint;
                return new Circle2(m, Line2.CalcLenght(m, this.Start));
            }
        }


        #endregion

        #region Geomerty Base Drawing

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public void Draw(Graphics g) {
            if (this.Pen != null && !Start.Equals(End))
                g.DrawLine(this.Pen, this.Start, this.End);
        }

        #endregion

        #region Geomtry Base Collision


        public IEnumerable<Vector2> Intersect(IGeometryBase other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE) {
            var pnts = new List<Vector2>();

            if (other is Line2) {
                var pnt = this.InterceptLine(other as Line2, tolerance);
                if(pnt.HasValue)
                    pnts.Add(pnt.Value);
            } else if (other is GdiText2) {
                pnts.AddRange(this.InterceptRect(other.BoundingBox, tolerance));
            } else
                other.Intersect(this);
            return pnts;
        }


        public bool IntersectsWith(IGeometryBase geometry, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {

            if (geometry is Line2)
            {
                return this.InterceptLineWith(geometry as Line2, tolerance);
            }
            else if (geometry is GdiText2 || geometry is ImageDrawable)
            {
                return this.InterceptRectWith(geometry.BoundingBox, tolerance);
            } else { //delegate Collision Detection to other Geometry Object
                return geometry.IntersectsWith(this, tolerance);
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate smalles distance to a Target Point
        /// </summary>
        /// <param name="pt">Target Point</param>
        /// <param name="closest">Closest Point on Line to Targetpoint</param>
        /// <returns>Distance to target point</returns>
        public double FindDistanceToPoint(Vector2 pt, out Vector2 closest) {
            return Line2.FindDistanceToPoint(pt, this, out closest);
        }
        public double FindDistanceToPoint(Vector2 pt) {
            Vector2 dummy;
            return Line2.FindDistanceToPoint(pt, this, out dummy);
        }

        /// <summary>
        /// Calculate the distance between point pt and the line.
        /// </summary>
        /// <param name="pt">Target Point to wich distance is calculated</param>
        /// <param name="line">line</param>
        /// <param name="closest">closes Point on Line to target Point</param>
        /// <returns>Smallest distance to the targetpoint</returns>
        public static double FindDistanceToPoint(Vector2 pt, Line2 line, out Vector2 closest) {
            var p1 = line.Start; var p2 = line.End;
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0)) {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            double t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0) {
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            } else if (t > 1) {
                closest = p2;
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            } else {
                closest = new Vector2(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        #endregion

        #region IDisposable

        public virtual void Dispose() {
            //if (_pen != null && !PenSharedResource)
            //    _pen.Dispose();
        }

        #endregion

        public override string ToString() {
            string dump = "";
            dump += "P1: " + this.Start.ToString() + "\n";
            dump += "P2: " + this.End.ToString() + "\n";
            dump += "slope: " + this.Slope + "\n";
            dump += "q: " + this.YMovement + "\n";
            dump += "vert: " + this.IsVertical + "\n";
            dump += "horz: " + this.IsHorizontal + "\n";

            return dump;
        }
    }
}
