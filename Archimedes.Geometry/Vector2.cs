/*******************************************
 * 
 *  Written by P. Büttiker (c) April 2010
 *  
 * 
 * *****************************************
 * *****************************************/
using System;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    /// <summary> 
    /// 2D Vector Type
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Constructors

        /// <summary>
        /// Create a new 2D Vector with given x/y Parts
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
            : this() {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Create a new 2D Vector from 2 given Points
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        public Vector2(Vector2 P1, Vector2 P2)
            : this(P2.X - P1.X, P2.Y - P1.Y) { }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns a Null-Vector
        /// </summary>
        public static readonly Vector2 Zero = new Vector2(0, 0); 

        /// <summary>
        /// Returns a X-Axis aligned Vector
        /// </summary>
        public static readonly Vector2 UnitX = new Vector2(1, 0);

        /// <summary>
        /// Returns a Y-Axis aligned Vector
        /// </summary>
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        #endregion

        #region Public Propertys

        /// <summary>
        /// The X Component of the Vector
        /// </summary>
        public float X;

        /// <summary>
        /// The Y Component of the Vector
        /// </summary>
        public float Y;

        /// <summary>
        /// Gets or Sets the Lenght of this Vector
        /// </summary>
        public float Lenght {
            get { return (float)(Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2))); }
            set {
                if (this.Lenght != 0) {
                    //v * newsize / |v|
                    this = this * (value / this.Lenght);
                } else
                    this = Vector2.Zero;
            }
        }

        /// <summary>
        /// Returns the slope of this Vector
        /// </summary>
        public float Slope {
            get { return (this.X != 0) ? (this.Y / this.X) : 0; }
        }

        #endregion

        #region Operator Overloads

        public static Vector2 operator *(float f1, Vector2 v1) {
            v1.X = v1.X * f1;
            v1.Y = v1.Y * f1;
            return v1;
        }

        public static Vector2 operator *(Vector2 v1, float f1) {
            v1.X = v1.X * f1;
            v1.Y = v1.Y * f1;
            return v1;
        }

        public static Vector2 operator /(Vector2 v1, float f1) {
            v1.X = v1.X / f1;
            v1.Y = v1.Y / f1;
            return v1;
        }

        /// <summary>
        /// Calc Scalarproduct (Dot-Product) of two Vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float operator *(Vector2 v1, Vector2 v2) {
            return v1.DotP(v2);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2) {
            v1.X += v2.X;
            v1.Y += v2.Y;
            return v1;
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2) {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            return v1;
        }



        public static implicit operator PointF(Vector2 other) {
            return new PointF(other.X, other.Y);
        }

        public static implicit operator Vector2(PointF other) {
            return new Vector2(other.X, other.Y);
        }

        #endregion    

        #region Static Methods

        public static Vector2 VectorFromAngle(float angle, float length) {
            var gk = length * (float)Math.Sin(MathHelper.ToRadians(angle));
            var ak = length * (float)Math.Cos(MathHelper.ToRadians(angle));
            return new Vector2(ak, gk);
        }

        public static Vector2 GetEndVector(IGeometryBase g) {
            Vector2 v;
            if (g is Line2) {
                v = (g as Line2).ToVector();
            } else if (g is Arc) {
                v = (g as Arc).EndVector;
            } else
                throw new NotSupportedException("Can't get End-Vector from IGeometryBase: " + g.GetType().ToString());
            return v;
        }

        /// <summary>
        /// Multiplicate a Vector with a Scalar (Equal to v1[Vector] * Operator[Number])
        /// </summary>
        /// <param name="scalar"></param>
        public static Vector2 Multiply(Vector2 v, float scalar) {
            return new Vector2(v.X * scalar, v.Y * scalar);
        }

        /// <summary>
        /// Divides this Vector with a scalar
        /// </summary>
        /// <param name="scalar"></param>
        public Vector2 Divide(float scalar) {
            if (scalar != 0) {
                return new Vector2(this.X / scalar, this.Y / scalar);
            } else
                return Vector2.Zero;
        }


        #endregion

        #region Transformation Methods

        /// <summary> 
        /// Calculates the Dot-Product of two Vectors
        /// </summary>
        /// <param name="v2">Other Vector</param>
        /// <returns></returns>
        public float DotP(Vector2 v2) {
            return (this.X * v2.X) + (this.Y * v2.Y);
        }

        /// <summary>
        /// Gets an new Vector based on this rotated by the specified amount
        /// </summary>
        /// <param name="angle">Rotation Angle</param>
        public Vector2 GetRotated(float angle) {
            var v2 = this;
            v2.Rotate(angle);
            return v2;
        }

        /// <summary>
        /// Rotates this Vector by the given amount
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(float angle) {
            float r = this.Lenght;
            float thisAngle = MathHelper.ToRadians(angle + this.GetAngle2X());
            this = new Vector2(r * (float)Math.Cos(thisAngle), r * (float)Math.Sin(thisAngle));
        }

        /// <summary>
        /// Turns this Vector into an Unit-Vector - a Vector with the same orientation but with 1 unit lenght
        /// </summary>
        /// <param name="newLenght"></param>
        /// <returns></returns>
        public void Normalize() {

            var len = this.Lenght;
            // Check for divide by zero errors
            if (len == 0) {
                this = Vector2.Zero;
            } else {
                // find the inverse of the vectors magnitude
                float inverse = 1 / len;
                this = new Vector2
                    (
                        // multiply each component by the inverse of the magnitude
                        this.X * inverse,
                        this.Y * inverse
                    );
            }
        }

        #endregion

        #region Specail Factorys

        /// <summary>Calculate a Vector which stands orthogonal on this Vector.
        /// 
        /// </summary>
        /// <param name="LEFT">The desired Direction of the new orthogonal Vector</param>
        /// <returns></returns>
        public Vector2 GetOrthogonalVector(Direction Direction) {
            // Crossproduct as unary operator (getting orthogonal)
            Vector2 orthVector = new Vector2();
            if (Direction == Direction.LEFT) {
                orthVector.X = this.Y * (-1);
                orthVector.Y = this.X;
            } else { // RIGHT
                orthVector.X = this.Y;
                orthVector.Y = this.X * (-1);
            }
            return orthVector;
        }



        #endregion

        #region Public Query Methods

        public Vector2 GetPoint(Vector2 startPoint) {
            return startPoint + this;
        }

        public Vector2 GetPoint(float XStart, float YStart) {
            return new Vector2(XStart,YStart) + this;
        }

        public bool IsParallel(Vector2 v2) {
            return (Math.Round((this.X / this.Y)) == Math.Round((v2.X / v2.Y)));
        }

        public bool IsVertical {
            get { return (this.X == 0); }
        }

        public bool IsHorizontal {
            get { return (this.Y == 0); }
        }

        public bool IsDirectionEqual(Vector2 v2) {
            bool bEqual = false;
            if (this.IsParallel(v2)) {
                bEqual = (this.X.IsSignEqual(v2.X) && this.Y.IsSignEqual(v2.Y));
            }
            return bEqual;
        }

        public float GetAngle2X() {

            float degree;
            Vector2 xVector = new Vector2(1, 0); // X-Vector

            degree = MathHelper.ToDegree((float)Math.Acos(this.DotP(xVector) / this.Lenght));
            if (this.Y < 0) {
                degree = 360 - degree;
            }
            return (float)Math.Round(degree, GeometrySettings.GEOMETRY_GLOBAL_ROUND);

        }

        /// <summary>
        /// Returns the Angle between two vectors 0° - 180° 
        /// (If you need 0° -360°, use GetAngleBetweenClockWise() instead.)
        /// </summary>
        /// <param name="vbase"></param>
        /// <returns></returns>
        public float GetAngle2V(Vector2 vbase) {
            float gamma;
            float tmp = this.DotP(vbase) / (this.Lenght * vbase.Lenght);
            gamma = MathHelper.ToDegree((float)Math.Acos(tmp));
            if (gamma > 180) {          //from mathematic definition, it's always the shorter angle to return.
                gamma = 360 - gamma;
            }
            return (float)Math.Round(gamma, GeometrySettings.GEOMETRY_GLOBAL_ROUND);
        }

        /// <summary>
        /// Returns Angle between two vectors. 
        /// The Angle is calculated from this vector until to the Destination Vector.
        /// </summary>
        /// <param name="B"></param>
        /// <param name="Direction">RIGHT = Clockwise, LEFT = other direction</param>
        /// <returns>0° - 360° Angle in degree</returns>
        public float GetAngleBetweenClockWise(Vector2 B, Direction Direction) {
            float theta;

            theta = GetAngle2V(B);

            if (((this.Y * B.X - this.X * B.Y) > 0) == (Direction == Direction.RIGHT)) {
                return theta;
            } else {
                return 360 - theta;
            }
        }

        #endregion

        #region Common Methods

        public override string ToString() {
            string vecDump = "";
            vecDump += "X: " + this.X.ToString() + "\n";
            vecDump += "Y: " + this.Y.ToString() + "\n";
            vecDump += "norm: " + this.Lenght.ToString() + "\n";
            return vecDump;
        }

        #endregion

        #region IEquatable

        public bool Equals(Vector2 other) {
            return this.Y == other.Y && this.X == other.X;
        }

        public override bool Equals(object obj) {
            if (obj is Vector2) {
                return Equals((Vector2)obj);
            } else
                return false;
        }

        public override int GetHashCode() {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        #endregion
    }
}

