using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Geometry
{
    /// <summary>
    /// Represents a size information. (Immutable)
    /// </summary>
    public struct SizeD
    {

        public static readonly SizeD Empty = new SizeD(0,0);

        public readonly double Width;
        public readonly double Height;

        public SizeD(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool IsEmpty { get { return Width == 0 && Height == 0; } }
    }
}
