using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry
{
    /// <summary>
    /// Represents an axis aligned rectangle / box.
    /// </summary>
    public class AABox 
    {
        #region Fields

        private Rectangle2 _internalRect = new Rectangle2();

        #endregion

        #region Constructors



        #endregion

        #region Public Properties

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        #endregion



    }
}
