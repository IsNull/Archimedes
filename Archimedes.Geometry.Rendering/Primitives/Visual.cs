using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry.Rendering.Primitives
{
    /// <summary>
    /// Represents any visual geometry
    /// </summary>
    public abstract class Visual : IDrawable
    {
        /// <summary>
        /// The pen used to draw the outline of this visual
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// The brush used to fill the content of this visual
        /// Note that not all visuals support filling.
        /// </summary>
        public Brush FillBrush { get; set; }

        /// <summary>
        /// Gets the geometry of this visual
        /// </summary>
        public abstract IGeometryBase Geometry { get; }

        /// <summary>
        /// Draws this visual to the given graphics context
        /// </summary>
        /// <param name="g"></param>
        public abstract void Draw(System.Drawing.Graphics g);

    }
}
