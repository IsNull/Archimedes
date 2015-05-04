using System;
using System.Collections.Generic;


namespace Archimedes.Patterns.Container
{
    public class AutoModuleConfiguration : ElderModuleConfiguration
    {
        private readonly IEnumerable<Type> _componentTypes;

        public AutoModuleConfiguration(IEnumerable<Type> componentTypes)
        {
            _componentTypes = componentTypes;
        }

        public override void Configure()
        {
            foreach (var componentType in _componentTypes)
            {
                if (componentType.IsDefined(typeof(ServiceAttribute), false))
                {
                    RegisterSingleton(componentType, componentType);

                    RegisterInterfaceImpl(componentType);

                }
            }
        }

        private void RegisterInterfaceImpl(Type componentType)
        {
            foreach (var @interface in componentType.GetInterfaces())
            {
                RegisterSingleton(@interface, componentType);
            }
        }

    }
}
