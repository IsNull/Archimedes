using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// Marks the component as a controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : ComponentAttribute
    {
        public ControllerAttribute() { }

        public ControllerAttribute(string name)
            : base(name)
        {
        }
    }
}
