using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.DragNDrop
{
    public interface IDragSource
    {
        void StartDrag(DragInfo dragInfo);
    }
}
