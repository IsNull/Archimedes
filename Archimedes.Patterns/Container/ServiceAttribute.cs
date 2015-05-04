using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class ServiceAttribute : ComponentAttribute
    {
        public ServiceAttribute() { }

        public ServiceAttribute(string name) : base(name)
        {
        }

    }
}
