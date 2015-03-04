using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Units;


/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  2010
 * 
 * *****************************************
 * *****************************************/


namespace Archimedes.Geometry.Primitives
{

    /// <summary>
    /// Represents an Arc in 2D Space
    /// </summary>
    public partial class Arc : IGeometryBase
    {
        #region Private Data

        double? _radius = null;
        Angle? _angle = null;
        double? _bowlen = null;

        Angle _anglediff = Units.Angle.Zero;

        Direction _direction = Direction.LEFT;
        Vector2 _startPoint;
        Vector2 _base;

        Pen _pen = null;

        #endregion

        #region Contructors

        public Arc() { }

        public Arc(Arc prototype)
        {
            ArcFromProtoType(prototype);
        }

        public Arc(Vector2 start, Vector2 inter, Vector2 end)
        {
            ArcFromProtoType(FromDescriptorPoints(start, inter, end));
        }

        private void ArcFromProtoType(Arc prototype)
        {
            Prototype(prototype);
        }


        public Arc(double? radius, Angle? angle, double? bowLen, Vector2 baseVector)
        {
            _angle = angle;
            _radius = radius;
            _bowlen = bowLen;
            _base = baseVector;
        }

        public Arc(double? uRadius, Angle? uAngle, double? uBowLen, Vector2 baseVector, Angle angleDiff)
            : this(uRadius, uAngle, uBowLen, baseVector)
        {
            _anglediff = angleDiff;
        }

        #endregion

        #region Public Methods (Transformators)

        public Vector2 GetPointOnArc(Angle deltaAngle)
        {

            Vector2 pointOnArc;
            var fakeBaseVector = _base.GetRotated(this.AngleDiff);

            using (var helperArc = new Arc(Radius, deltaAngle, null, _base, AngleDiff))
            {
                helperArc.Direction = this.Direction;
                helperArc.Location = this.Location;

                var relAngle = CalcRelAngle(helperArc.Angle, fakeBaseVector.GetAngle2X(), helperArc.Direction);
                pointOnArc = CalcEndpointDelta2M(helperArc.Radius, relAngle);

                var helperMP = helperArc.MiddlePoint;
                pointOnArc = new Vector2(pointOnArc.X + helperMP.X, pointOnArc.Y + helperMP.Y);
            }
            return pointOnArc;
        }

        /// <summary> 
        /// Calc the delta from the endpoint to M
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="relAngle"></param>
        /// <returns></returns>
        /// 
        private Vector2 CalcEndpointDelta2M(double radius, Angle rel)
        {
            return new Vector2(
                radius * Math.Cos(rel.Radians),
                radius * Math.Sin(rel.Radians));
        }

        /// <summary> 
        /// Calc Relative angle
        /// </summary>
        /// <param name="bowAngle">Bogenwinkel</param>
        /// <param name="baseAngle">Winkel des Base Vector</param>
        /// <param name="direction">Links oder Rechts</param>
        /// <returns>Relativer Winkel</returns>
        private Angle CalcRelAngle(Angle bowAngle, Angle baseAngle, Direction direction)
        {
            if (bowAngle > Units.Angle.FromDegrees(360)) { bowAngle -= Units.Angle.FromDegrees(360); }

            var relAngle = bowAngle + baseAngle;

            if (direction == Direction.LEFT)
            {
                relAngle += Units.Angle.FromDegrees(270);
            }
            else
            {
                relAngle += Units.Angle.FromDegrees(90) - 2 * (bowAngle - Units.Angle.FromDegrees(180));
            }

            // TODO Normalize as Angle method?
            if (relAngle > Units.Angle.FromDegrees(360)) { relAngle -= Units.Angle.FromDegrees(360); }

            return relAngle;
        }


        public void Scale(double factor)
        {
            this.Location = this.Location.Scale(factor);

            if (_radius.HasValue)
                _radius *= factor;

            if (_bowlen.HasValue)
                _bowlen *= factor;
        }

        #endregion


        #region Public Propertys

        /// <summary>Effective Start Angle to draw
        /// 
        /// </summary>
        public Angle Angle2X
        {
            get
            {
                var angle = _base.GetAngle2X() + AngleDiff;

                // correct angle if we have opposite direction
                if (Direction == Direction.RIGHT)
                {
                    angle -= (this.Angle - Angle.FromDegrees(180));
                }
                return angle;
            }
        }

        /// <summary> 
        /// Get the Arc's Endpoint
        /// </summary>
        public Vector2 EndPoint
        {
            get
            {
                return GetPointOnArc(this.Angle);
            }
        }

        /// <summary>Angle break
        /// NULL = The Bow is tangent like and has no break.
        /// </summary>
        /// 
        public Angle AngleDiff
        {
            get { return _anglediff; }
            set
            {
                _anglediff = value;
            }
        }

        public Direction Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
            }
        }

        public double Radius
        {
            set
            {
                _radius = value;
                if (_angle.HasValue && _bowlen.HasValue)
                    _bowlen = null;
            }
            get
            {
                if (_radius == null)
                { // radius not set!
                    // possible to calc radius?
                    if (_bowlen != null && _angle != null && _angle != Angle.Zero)
                    {
                        return (float)((180 * _bowlen) / (Math.PI * _angle.Value.Degrees));
                    }
                    else
                    {
                        return 0f;
                    }
                }
                else
                {
                    return (float)_radius;
                }

            }
        }

        /// <summary>
        /// The Arc Angle
        /// </summary>
        public Angle Angle
        {
            set
            {
                _angle = value;
                if (_radius.HasValue && _bowlen.HasValue)
                    _bowlen = null;
            }
            get
            {
                if (_angle.HasValue)
                {
                    return _angle.Value;
                }
                
                // angle not set!
                // possible to calc angle?
                if (_bowlen.HasValue && _radius.HasValue && _radius != 0)
                {
                    return Angle.FromDegrees(180 * _bowlen.Value) / (Math.PI * _radius.Value);
                }

                return Angle.Zero;
            }
        }

        /// <summary>
        /// Lenght of the Arc-Bow-Line
        /// </summary>
        public float BowLen
        {
            set
            {
                _bowlen = value;
                if (_radius.HasValue && _angle.HasValue)
                    _radius = null;
            }
            get
            {
                if (_bowlen == null)
                {

                    // possible to calc bowlen?
                    if (_radius != null && _angle != null)
                    {
                        return (float)(_radius * Math.PI * (_angle.Value.Degrees / 180));
                    }
                    else
                    {
                        return 0f;
                    }
                }
                else
                {
                    return (float)_bowlen;
                }
            }
        }

        public RectangleF DrawingRect
        {
            get
            {
                var middlePoint = this.MiddlePoint;
                return new RectangleF((float)(middlePoint.X - this.Radius), (float)(middlePoint.Y - this.Radius), 2 * (float)this.Radius, 2 * (float)this.Radius);
            }
        }

        #endregion

        #region Specail Transformation

        /// <summary>
        /// Split the Arc at the given Point
        /// </summary>
        /// <param name="splitPoint">Point to split</param>
        /// <returns></returns>
        public IEnumerable<Arc> Split(Vector2 splitPoint)
        {
            var vstart = new Vector2(this.MiddlePoint, this.Location);
            var vsplit = new Vector2(this.MiddlePoint, splitPoint);
            return Split(vstart.GetAngle2V(vsplit));
        }

        /// <summary>
        /// Split a given Angle-Amount from this Arc
        /// </summary>
        /// <param name="splitAngle"></param>
        /// <returns></returns>
        public IEnumerable<Arc> Split(Angle splitAngle)
        {
            var arcs = new List<Arc>();

            splitAngle = Angle.Abs(splitAngle);
            if ((splitAngle > this.Angle))
            {
                arcs.Add(this); // can't split...
            }
            else
            { /* split the arc */

                //segment uno
                var split = new Arc(this.Radius, splitAngle, null, _base);
                split.Location = this.Location;
                split.Direction = this.Direction;
                split.AngleDiff = this.AngleDiff;
                arcs.Add(split);

                //segment due
                Vector2 newBase;
                if (this.Direction == Direction.LEFT)
                {
                    newBase = _base.GetRotated(splitAngle);
                }
                else
                {
                    newBase = _base.GetRotated(-splitAngle);
                }
                split = new Arc(this.Radius, this.Angle - splitAngle, null, newBase);
                split.Location = this.GetPointOnArc(splitAngle);
                split.Direction = this.Direction;
                split.AngleDiff = this.AngleDiff;
                arcs.Add(split);
            }
            return arcs;
        }
        #endregion

        #region To Methods

        public GraphicsPath ToPath()
        {
            var path = new GraphicsPath();
            try
            {
                path.AddArc(this.DrawingRect, (float)Angle2X.Degrees - 90, (float)Angle.Degrees);
            }
            catch (ArgumentException)
            {
                //igonore - return void path
            }
            return path;
        }

        public Vertices ToVertices()
        {
            Vertices vertices = new Vertices();
            try
            {
                var path = ToPath();
                path.Flatten();
                vertices.AddRange(path.PathPoints);
            }
            catch (ArgumentException)
            {
                // igonroe yield nothing
            }
            return vertices;
        }

        public Circle2 ToCircle()
        {
            var c = new Circle2(this.MiddlePoint, this.Radius);
            if (c.Pen != null)
                c.Pen = this.Pen.Clone() as Pen;
            return c;
        }

        #endregion

        #region Geomerty Base

        public Vector2 MiddlePoint
        {
            get
            {
                // calc vector which points to the arc middlepoint
                var orthBaseVector = _base.GetOrthogonalVector(this.Direction);
                orthBaseVector = orthBaseVector.GetRotated(this.AngleDiff);
                orthBaseVector.Length = this.Radius;

                return new Vector2((float)(this.Location.X + orthBaseVector.X), (float)(this.Location.Y + orthBaseVector.Y));
            }
            set
            {
                var vMove = new Vector2(this.MiddlePoint, value);
                this.Location += vMove;
            }
        }

        public void Move(Vector2 mov)
        {
            this.Location += mov;
        }

        /// <summary> Arc StartPoint
        /// 
        /// </summary>
        public Vector2 Location
        {
            get { return _startPoint; }
            set
            {
                _startPoint = value;
            }
        }

        public IGeometryBase Clone()
        {
            return new Arc(this);
        }

        public void Prototype(IGeometryBase iprototype)
        {

            var prototype = (iprototype as Arc);
            if (prototype == null)
                throw new InvalidOperationException();

            _angle = prototype.Angle;
            _radius = prototype.Radius;
            _bowlen = prototype.BowLen;
            _base = prototype.BaseVector;

            Location = prototype.Location;
            Direction = prototype.Direction;
            AngleDiff = prototype.AngleDiff;
            try
            {
                if (prototype.Pen != null)
                    this.Pen = prototype.Pen.Clone() as Pen;
            }
            catch (Exception)
            {
                /// ignore
            }
        }



        public Circle2 BoundingCircle
        {
            get { return this.ToCircle(); }
        }

        public RectangleF BoundingBox
        {
            get
            {
                RectangleF box;
                try
                {
                    box = new Polygon2(ToVertices()).BoundingBox;
                }
                catch (Exception)
                {
                    box = new RectangleF();
                    //igonre - return void boundingbox
                }
                return box;
            }
        }

        public Rectangle2 BoundingBoxSmallest
        {
            get
            {
                Rectangle2 box;
                try
                {
                    box = new Polygon2(ToVertices()).FindSmallestBoundingBox();
                }
                catch (Exception)
                {
                    box = new Rectangle2();
                }
                return box;
            }
        }

        #endregion

        #region Geomerty Base Drawing

        public Pen Pen
        {
            get { return _pen; }
            set { _pen = value; }
        }

        public void Draw(Graphics g)
        {
            if (this.Pen != null)
                g.DrawArc(this.Pen, this.DrawingRect, (float)(Angle2X.Degrees - 90), (float)Angle.Degrees);
        }
        public void AddToPath(GraphicsPath path)
        {
            path.AddArc(this.DrawingRect, (float)(this.Angle2X.Degrees - 90), (float)Angle.Degrees);
        }

        #endregion

        public Vector2 EndVector
        {
            get
            {
                return Vector2.FromAngleAndLenght(this.AngleDiff + this.Angle, 1);
            }
        }

        public Vector2 BaseVector
        {
            get { return _base; }
            set { _base = value; }
        }

        public virtual void Dispose()
        {
            //if(Pen != null)
            //    Pen.Dispose();
        }

        public override string ToString()
        {
            string str = "";

            str += "MiddlePoint: " + this.MiddlePoint.ToString() + "\n";
            str += "Angle: " + this.Angle + "\n";
            str += "BowLen: " + this.BowLen + "\n";
            str += "Radius: " + this.Radius + "\n";
            str += "Direction: " + this.Direction + "\n";

            return str;
        }

    }
}
