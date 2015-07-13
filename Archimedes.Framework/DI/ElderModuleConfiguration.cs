using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Base class of the ElderBox DI configuration
    /// </summary>
    public abstract class ElderModuleConfiguration : IModuleConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<Type, ImplementationRegistry> _componentRegistry = new Dictionary<Type, ImplementationRegistry>();


        /// <summary>
        /// Called before the dependency container is being constructed.
        /// 
        /// Sub classes are expected to overwrite this method and 
        /// call <see cref="RegisterSingleton"/> to configure the module.
        /// </summary>
        public void Configure()
        {
            ConfigureInternal();
            LogConfiguration();
        }

        public abstract void ConfigureInternal();

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
            if (!_componentRegistry.ContainsKey(iface))
            {
                _componentRegistry.Add(iface, new ImplementationRegistry(iface));
            }

            _componentRegistry[iface].Register(impl);
        }

        [DebuggerStepThrough]
        public Type GetImplementaionTypeFor(Type type)
        {
            if (_componentRegistry.ContainsKey(type))
            {
                return _componentRegistry[type].TryGetImplementation();
            }
            return null;
        }

        public void LogConfiguration()
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("DI Module Configuration:");
                foreach (var kv in _componentRegistry)
                {
                    Log.Debug(kv.Key + "  ===>  " + kv.Value);
                }
            }
        }
    }
}
