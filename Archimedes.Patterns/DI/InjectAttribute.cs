using System;

namespace Archimedes.DI.AOP
{
    /// <summary>
    /// Marks a Field or Constructor as Injection target
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {

    }
}
