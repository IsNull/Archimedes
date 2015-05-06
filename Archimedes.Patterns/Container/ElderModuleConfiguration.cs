using System;
using System.Collections.Generic;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ElderModuleConfiguration : IModuleConfiguration
    {

        private readonly Dictionary<Type, Type> _componentRegistry = new Dictionary<Type, Type>();


        /// <summary>
        /// Called before the dependency container is being constructed.
        /// 
        /// Sub classes are expected to overwrite this method and 
        /// call <see cref="RegisterSingleton"/> to configure the module.
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Registers a service as a singleton.
        /// </summary>
        protected void RegisterSingleton<TInterface, TImplemention>()
            where TImplemention : TInterface
        {
            RegisterSingleton(typeof(TInterface), typeof(TImplemention));
        }

        protected void RegisterSingleton(Type iface, Type impl)
        {
            _componentRegistry.Add(iface, impl);
        }


        public Type GetImplementaionTypeFor(Type type)
        {
            if (_componentRegistry.ContainsKey(type))
            {
                return _componentRegistry[type];
            }
            return null;
        }
    }
}
