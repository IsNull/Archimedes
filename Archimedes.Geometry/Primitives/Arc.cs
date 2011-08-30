using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Extensions;


/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  2010
 * 
 * *****************************************
 * *****************************************/


namespace Archimedes.Geometry.Primitives
{

    public partial class Arc : IGeometryBase
    {
        #region Private Data

        float? _radius = null;
        float? _angle = null;
        float? _bowlen = null;

        float _anglediff = 0f;
        
        Direction _direction = Direction.LEFT;
        Vector2 _startPoint;
        Vector2 _base;

        Pen _pen = null;
        bool _verticesInvalidated = true;

        #endregion 

        #region Contructors

        public Arc() { }

        public Arc(Arc prototype) {
            ArcFromProtoType(prototype);
        }

        public Arc(Vector2 start, Vector2 inter, Vector2 end) {
            ArcFromProtoType(FromDescriptorPoints(start, inter, end));
        }

        private void ArcFromProtoType(Arc prototype){
            Prototype(prototype);
        }


        public Arc(float? radius, float? angle, float? bowLen, Vector2 baseVector) {
            _angle = angle;
            _radius = radius;
            _bowlen = bowLen;
            _base = baseVector;
        }

        public Arc(float? uRadius, float? uAngle, float? uBowLen, Vector2 BaseVector, float AngleDiff)
            : this(uRadius, uAngle, uBowLen, BaseVector) {
            _anglediff = AngleDiff;
        }

        #endregion

        #region Public Methods (Transformators)

        public Vector2 GetPointOnArc(float DeltaAngle) {

            Vector2 pointOnArc;
            var fakeBaseVector = _base.GetRotated(this.AngleDiff);

            using (var helperArc = new Arc(Radius, DeltaAngle, null, _base, AngleDiff)) {
                helperArc.Direction = this.Direction;
                helperArc.Location = this.Location;

                var relAngle = CalcRelAngle(helperArc.Angle, (float)fakeBaseVector.GetAngle2X(), helperArc.Direction);
                pointOnArc = CalcEndpointDelta2M(helperArc.Radius, relAngle);

                var helperMP = helperArc.MiddlePoint;

                pointOnArc.X += helperMP.X;
                pointOnArc.Y += helperMP.Y;
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
        private Vector2 CalcEndpointDelta2M(float radius, float relAngle) {
            var delta2M = new Vector2(radius, radius);
            delta2M.X = (float)(delta2M.X * Math.Cos(MathHelper.ToRadians(relAngle)));
            delta2M.Y = (float)(delta2M.Y * Math.Sin(MathHelper.ToRadians(relAngle)));
            return delta2M;
        }

        /// <summary> 
        /// Calc Relative angle
        /// </summary>
        /// <param name="BowAngle">Bogenwinkel</param>
        /// <param name="BaseAngle">Winkel des Base Vector</param>
        /// <param name="Direction">Links oder Rechts</param>
        /// <returns>Relativer Winkel</returns>
        private float CalcRelAngle(float BowAngle, float BaseAngle, Direction Direction) {
            float RelAngle;
            if (BowAngle > 360) { BowAngle -= 360; }

            RelAngle = BowAngle + BaseAngle;

            if (Direction == Direction.LEFT) {
                RelAngle += 270;
            } else {
                RelAngle += 90 - 2 * (BowAngle - 180);
            }

            if (RelAngle > 360) { RelAngle -= 360; }

            return RelAngle;
        }


        public void Scale(float factor) {
            this.Location = this.Location.Scale(factor);

            if(_radius.HasValue)
                _radius *= factor;

            if (_bowlen.HasValue)
                _bowlen *= factor;
        }

        #endregion


        #region Public Propertys

        /// <summary>Effective Start Angle to draw
        /// 
        /// </summary>
        public float Angle2X {
            get {
                float angle = _base.GetAngle2X() + AngleDiff;

                // correct angle if we have opposite direction
                if (Direction == Direction.RIGHT) {
                    angle -= (this.Angle - 180);
                }
                return angle;
            }
        }

        /// <summary> 
        /// Get the Arc's Endpoint
        /// </summary>
        public Vector2 EndPoint {
            get {
                return GetPointOnArc(this.Angle);
            }
        }

        /// <summary>Angle break
        /// NULL = The Bow is tangent like and has no break.
        /// </summary>
        /// 
        public float AngleDiff {
            get { return _anglediff; }
            set { 
                _anglediff = value;
                Invalidate();
            }
        }

        public Direction Direction {
            get { return _direction; }
            set {
                _direction = value;
                Invalidate();
            }
        }

        public float Radius {
            set {
                _radius = value;
                if (_angle.HasValue && _bowlen.HasValue)
                    _bowlen = null;

                Invalidate();
            }
            get {
                if (_radius == null) { // radius not set!
                    // possible to calc radius?
                    if (_bowlen != null && _angle != null && _angle != 0) {
                        return (float)((180 * _bowlen) / (Math.PI * _angle));
                    } else {
                        return 0f;
                    }
                } else {
                    return (float)_radius;
                }

            }
        }

        public float Angle {
            set {
                _angle = value;
                if (_radius.HasValue && _bowlen.HasValue)
                    _bowlen = null;

                Invalidate();
            }
            get {
                if (!_angle.HasValue) {  // angle not set!
                    // possible to calc angle?
                    if (_bowlen.HasValue && _radius.HasValue && _radius != 0) {
                        return (float)((180 * _bowlen) / (Math.PI * _radius));
                    } else {
                        return 0f;
                    }
                } else {
                    return (float)_angle;
                }
            }
        }

        public float BowLen {
            set {
                _bowlen = value;
                if(_radius.HasValue && _angle.HasValue)
                    _radius = null;
                Invalidate();
            }
            get {
                if (_bowlen == null) {

                    // possible to calc bowlen?
                    if (_radius != null && _angle != null) {
                        return (float)(_radius * Math.PI * (_angle / 180));
                    } else {
                        return 0f;
                    }
                } else {
                    return (float)_bowlen;
                }
            }
        }

        public RectangleF DrawingRect {
            get {
                var middlePoint = this.MiddlePoint;
                return new RectangleF(middlePoint.X - this.Radius, middlePoint.Y - this.Radius, 2 * this.Radius, 2 * this.Radius);
            }
        }

        #endregion

        #region Specail Transformation

        /// <summary>
        /// Split the Arc at the given Point
        /// </summary>
        /// <param name="splitPoint">Point to split</param>
        /// <returns></returns>
        public IEnumerable<Arc> Split(Vector2 splitPoint) {
            var vstart = new Vector2(this.MiddlePoint, this.Location);
            var vsplit = new Vector2(this.MiddlePoint, splitPoint);
            return Split(vstart.GetAngle2V(vsplit));
        }

        /// <summary>
        /// Split a given Angle-Amount from this Arc
        /// </summary>
        /// <param name="splitAngle"></param>
        /// <returns></returns>
        public IEnumerable<Arc> Split(float splitAngle) {
            var arcs = new List<Arc>();

            splitAngle = Math.Abs(splitAngle);
            if ((splitAngle > this.Angle)) {
                arcs.Add(this); // can't split...
            } else { /* split the arc */
                
                //segment uno
                var split = new Arc(this.Radius, splitAngle, null, _base);
                split.Location = this.Location;
                split.Direction = this.Direction;
                split.AngleDiff = this.AngleDiff;
                arcs.Add(split);

                //segment due
                Vector2 newBase;
                if (this.Direction == Direction.LEFT) {
                    newBase = _base.GetRotated(splitAngle);
                } else {
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

        public GraphicsPath ToPath() {
            var path = new GraphicsPath();
            try {
                path.AddArc(this.DrawingRect, this.Angle2X - 90, this.Angle);
            } catch (ArgumentException) {
                //igonore - return void path
            }
            return path;
        }

        public Vertices ToVertices() {
            Vertices vertices = new Vertices();
            try {
                var path = ToPath();
                path.Flatten();
                vertices.AddRange(path.PathPoints);
                Invalidate();
            } catch (ArgumentException) {
                // igonroe yield nothing
            }
            return vertices;
        }

        public Circle2 ToCircle() {
            var c = new Circle2(this.MiddlePoint, this.Radius);
            if(c.Pen != null)
                c.Pen = this.Pen.Clone() as Pen;
            return c;
        }

        #endregion

        #region Geomerty Base

        public Vector2 MiddlePoint {
            get {
                // calc vector which points to the arc middlepoint
                var orthBaseVector = _base.GetOrthogonalVector(this.Direction);
                orthBaseVector = orthBaseVector.GetRotated(this.AngleDiff);
                orthBaseVector.Lenght = this.Radius;                              

                return new Vector2((float)(this.Location.X + orthBaseVector.X), (float)(this.Location.Y + orthBaseVector.Y));
            }
            set {
                var vMove = new Vector2(this.MiddlePoint, value);
                this.Location += vMove;
            }
        }

        public void Move(Vector2 mov) {
            this.Location += mov;
        }

        /// <summary> Arc StartPoint
        /// 
        /// </summary>
        public Vector2 Location {
            get { return _startPoint; }
            set {
                _startPoint = value;
                Invalidate();
            }
        }

        public IGeometryBase Clone() {
            return new Arc(this);
        }

        public void Prototype(IGeometryBase iprototype) {

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
            try {
                if (prototype.Pen != null)
                    this.Pen = prototype.Pen.Clone() as Pen;
            } catch (Exception) {
                /// ignore
            }
        }



        public Circle2 BoundingCircle {
            get { return this.ToCircle(); }
        }

        public RectangleF BoundingBox {
            get {
                RectangleF box;
                try {
                    box = new Polygon2(ToVertices()).BoundingBox;
                } catch(Exception) {
                    box = new RectangleF();
                    //igonre - return void boundingbox
                }
                return box;
            }
        }

        public Rectangle2 BoundingBoxSmallest {
            get {
                Rectangle2 box;
                try {
                    box = new Polygon2(ToVertices()).FindSmallestBoundingBox();
                } catch (Exception) {
                    box = new Rectangle2();
                }
                return box;
            }
        }

        #endregion

        #region Geomerty Base Drawing

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public void Draw(Graphics G) {
            if (this.Pen != null)
                G.DrawArc(this.Pen, this.DrawingRect, this.Angle2X - 90, this.Angle);
        }
        public void AddToPath(GraphicsPath path) {
            path.AddArc(this.DrawingRect, this.Angle2X - 90, this.Angle);
        }

        #endregion

        public Vector2 EndVector {
            get {
                return Vector2.VectorFromAngle(this.AngleDiff + this.Angle, 1);   
            }
        }

        public Vector2 BaseVector {
            get { return _base; }
            set { _base = value; }
        }

        public virtual void Dispose() {
            //if(Pen != null)
            //    Pen.Dispose();
        }
        
        private void Invalidate() {
            _verticesInvalidated = true;
        }

        public override string ToString() {
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
