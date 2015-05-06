using System;

namespace Archimedes.DI
{
    public interface IModuleConfiguration
    {
        void Configure();

        Type GetImplementaionTypeFor(Type type);
    }
}
