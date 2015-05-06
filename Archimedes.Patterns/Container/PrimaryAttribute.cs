using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// If there are ambiugities between possible implementations for a requested type,
    /// the implemnetaion marked with primary is automatically choosen.
    /// 
    /// Otherwise, an AmbiugitieException is thrown.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryAttribute : Attribute
    {
    }
}
