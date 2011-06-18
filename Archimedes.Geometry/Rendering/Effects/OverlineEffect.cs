using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Archimedes.Geometry.Rendering.Effects
{
    //Draws the Given IGeometryBase with specific Pen/Brush
    public class OverlineEffect : IEffect
    {
        private IGeometryBase geometry;

        public OverlineEffect(IGeometryBase ugeometry, Pen effectPen = null, Brush effectBrush = null) {
            geometry = ugeometry.Clone();

            EffectPen = effectPen;
            if (geometry is IClosedGeometry)
                EffectBrush = effectBrush;         
        }

        #region Public Properties

        public Pen EffectPen {
            get { return geometry.Pen; }
            set { geometry.Pen = value; }
        }

        public Brush EffectBrush {
            get { 
                return (geometry is IClosedGeometry) ? (geometry as IClosedGeometry).FillBrush : null;
            }
            set {
                if (geometry is IClosedGeometry) {
                    (geometry as IClosedGeometry).FillBrush = value;
                } else
                    throw new NotSupportedException("This Geometry doesn't support FillBrush Property!");

            }
        }

        #endregion

        public void Draw(Graphics G) {
            geometry.Draw(G);
        }
    }
}
