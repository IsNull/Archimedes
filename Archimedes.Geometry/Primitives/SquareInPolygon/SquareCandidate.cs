using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Genetics;

namespace Archimedes.Geometry.Primitives.SquareInPolygon
{
    public class SquareCandidate : Candidate
    {
        private readonly Rectangle2 _rectangle;

        #region Allel Definitions

        /// <summary>
        /// Allel representing the X-Coordinate
        /// </summary>
        public static readonly Allel AllelX = new Allel("X-Coordinate");

        /// <summary>
        /// Allel representing the Y-Coordinate
        /// </summary>
        public static readonly Allel AllelY = new Allel("Y-Coordinate");

        /// <summary>
        /// Allel representing the Size
        /// </summary>
        public static readonly Allel AllelSize = new Allel("Size");

        /// <summary>
        /// Allel representing the Rotation
        /// </summary>
        public static readonly Allel AllelRotation = new Allel("Rotation", 0, 180);


        public static Allel[] AllAllels = { AllelX, AllelY, AllelSize, AllelRotation };

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Candidate from the given generation
        /// </summary>
        /// <param name="originGeneration"></param>
        public SquareCandidate(int originGeneration)
            : base(originGeneration)
        {
            _rectangle = new Rectangle2();
        }

        public SquareCandidate(int originGeneration, SquareCandidate prototype)
            : base(originGeneration)
        {
            _rectangle = new Rectangle2(prototype.Geometry);
        }

        public SquareCandidate(int originGeneration, Vector2 middlePoint, double size, double rotation)
            : base(originGeneration)
        {
            _rectangle = new Rectangle2()
                            {
                                MiddlePoint = middlePoint,
                                Width = size,
                                Height = size,
                                Angle = rotation
                            };
        }

        #endregion

        public bool IsOutside { get; set; }


        public override void Prototype(Candidate prototype)
        {
            var sq = prototype as SquareCandidate;
            if(sq != null)
                Prototype(sq);
        }

        public void Prototype(SquareCandidate prototype)
        {
            _rectangle.Prototype(prototype.Geometry);
        }


        public Rectangle2 Geometry
        {
            get {
                return _rectangle;
            }
        }

        #region Overrides of Candidate

        public override double GetAllelValue(Allel allel)
        {
            if (allel == AllelX)
            {
                return _rectangle.MiddlePoint.X;
            }

            if (allel == AllelY)
            {
                return _rectangle.MiddlePoint.Y;
            }

            if (allel == AllelSize)
            {
                return _rectangle.Width;
            }

            if (allel == AllelRotation)
            {
                return _rectangle.Angle;
            }

            throw new NotSupportedException("This Candidate does not contain an Allel '" + allel + "'");
        }

        public override void SetAllelValue(Allel allel, double value)
        {
            if (allel == AllelX)
            {
                _rectangle.MiddlePoint = new Vector2(value, _rectangle.MiddlePoint.Y);
                return;
            }

            if (allel == AllelY)
            {
                _rectangle.MiddlePoint = new Vector2(_rectangle.MiddlePoint.X, value);
                return;
            }

            if (allel == AllelSize)
            {
                _rectangle.Width = value;
                _rectangle.Height = value;
                return;
            }

            if (allel == AllelRotation)
            {
                _rectangle.Angle = value;
                return;
            }

            throw new NotSupportedException("This Candidate does not contain an Allel '" + allel + "'");

        }
        

        /// <summary>
        /// Gets all allels in this candidate
        /// </summary>
        public override Allel[] Allels
        {
            get { return AllAllels; }
        }

        #endregion
    }
}
