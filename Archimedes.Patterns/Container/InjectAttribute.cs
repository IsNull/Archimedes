using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// Marks a Constructor as Injection target
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectAttribute : Attribute
    {

    }
}
