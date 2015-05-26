using System;

namespace Archimedes.Framework.DI
{
    public interface IModuleConfiguration
    {
        void Configure();

        Type GetImplementaionTypeFor(Type type);
    }
}
