using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry.Rendering
{
    public class ZoomControler
    {
        private float zoomMultiplier = 1.5f;
        private float currentZoom = 1.0f;

        public ZoomControler() { }

        public ZoomControler(float multiplier) {
            ZoomMultiplier = multiplier;
        }

        public void ZoomIn() {
            currentZoom *= (1 * zoomMultiplier);
        }
        public void ZoomOut() {
            currentZoom *= (1 / zoomMultiplier); 
        }

        public float CurrentZoom {
            get { return currentZoom; }
        }
        public float ZoomMultiplier {
            get { return zoomMultiplier; }
            set {
                if (value <= 0)
                    throw new ArgumentException("multiplier must be greater than Null");
                zoomMultiplier = value; 
            }
        }

    }
}
