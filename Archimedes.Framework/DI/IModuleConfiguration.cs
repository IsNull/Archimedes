using System;

namespace Archimedes.Framework.DI
{
    public interface IModuleConfiguration
    {
        void Configure();

        /// <summary>
        /// Resolve the implementation type for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Type GetImplementaionTypeFor(Type type);
    }
}
