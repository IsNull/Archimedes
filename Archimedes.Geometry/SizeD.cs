using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry
{
    public struct SizeD
    {
        public static readonly SizeD Empty = new SizeD(0, 0);

        public readonly double Width;
        public readonly double Height;

        public SizeD(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool IsEmpty { get { return Width == 0 && Height == 0; } }

        public override bool Equals(Object obj)
        {
            return obj is SizeD && this == (SizeD)obj;
        }
        public override int GetHashCode()
        {
            return Width.GetHashCode() * 17 ^ Height.GetHashCode();
        }
        public static bool operator ==(SizeD x, SizeD y)
        {
            return x.Width == y.Width && x.Height == y.Height;
        }
        public static bool operator !=(SizeD x, SizeD y)
        {
            return !(x == y);
        }
    }
}
