using System.Drawing;

namespace Archimedes.Geometry
{
    public static class RectangleFUtil
    {

        public static RectangleF ToRectangleF(AARectangle aarect)
        {
            return new RectangleF((float)aarect.X, (float)aarect.Y, (float)aarect.Width, (float)aarect.Height);
        }

    }
}
