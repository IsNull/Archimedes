using System;
using System.Collections.Generic;

namespace Archimedes.DI.AOP
{
    /// <summary>
    /// If there are ambiugities between possible implementations for a requested type,
    /// the implemnetaion marked with primary is automatically choosen.
    /// 
    /// Without it an AmbiugitieException may be thrown.
    /// 
    /// You can specify base types for which the primary rule applies. By default,
    /// it applies to all base types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryAttribute : Attribute
    {
        private readonly Type[] _primaryForTypes;
        
        /// <summary>
        /// Creates a new PrimaryAttribute
        /// </summary>
        public PrimaryAttribute()
        {
            _primaryForTypes = new Type[0];
        }

        /// <summary>
        /// Creates a new PrimaryAttribute
        /// </summary>
        /// <param name="primaryForTypes"></param>
        public PrimaryAttribute(params Type[] primaryForTypes)
        {
            _primaryForTypes = primaryForTypes;
        }

        public Type[] PrimaryForTypes
        {
            get { return _primaryForTypes; }
        }
    }
}
