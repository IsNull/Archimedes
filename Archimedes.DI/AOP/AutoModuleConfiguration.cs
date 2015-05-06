using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace Archimedes.DI.AOP
{
    public class AutoModuleConfiguration : ElderModuleConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private readonly IEnumerable<Type> _componentTypes;

        public AutoModuleConfiguration(IEnumerable<Type> componentTypes)
        {
            _componentTypes = componentTypes;
        }

        public override void ConfigureInternal()
        {
            foreach (var componentType in _componentTypes)
            {
                if (componentType.IsDefined(typeof(ServiceAttribute), false) || 
                    componentType.IsDefined(typeof(ControllerAttribute), false) || 
                    componentType.IsDefined(typeof(ComponentAttribute), false))
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
