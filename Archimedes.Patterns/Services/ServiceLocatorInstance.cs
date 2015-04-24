using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Archimedes.Patterns.Services
{
    [Obsolete("Consider using Unity or MEF for Dependency Injection")]
    public class ServiceLocatorInstance : IServiceLocator
    {
        readonly Dictionary<Type, ServiceInfo> _services = new Dictionary<Type, ServiceInfo>();

        #region Public methods

        /// <summary>
        /// Registers a service.
        /// </summary>
        public void Register<TInterface, TImplemention>() 
            where TImplemention : TInterface {
            Register<TInterface, TImplemention>(false);
        }


        /// <summary>
        /// Registers a service as a singleton.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplemention>() 
            where TImplemention : TInterface {
            Register<TInterface, TImplemention>(true);
        }

        /// <summary>
        /// Registers a service with specific instance (this will automatically be handled as a singleton)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="instance"></param>
        public void RegisterInstance<TInterface>(TInterface instance) {
            _services.Add(typeof(TInterface), new ServiceInfo(this, instance));
        }


        /// <summary>
        /// Resolves a service.
        /// </summary>
        /// <typeparam name="TInterface">Service Interface</typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TInterface Resolve<TInterface>() {
            if (!_services.ContainsKey(typeof(TInterface))) {
                throw new ServiceNotFoundException(typeof(TInterface));
            }
            return (TInterface)_services[typeof(TInterface)].ServiceImplementation;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <param name="isSingleton">true if service is Singleton; otherwise false.</param>
        private void Register<TInterface, TImplemention>(bool isSingleton) where TImplemention : TInterface
        {
            _services.Add(typeof(TInterface), new ServiceInfo(this, typeof(TImplemention), isSingleton));
        }


        /// <summary>
        /// Creates an instance of a specific type.
        /// </summary>
        /// <param name="type">The type of the instance to create.</param>
        private object CreateInstance(Type type)
        {
            if (this.Contains(type))
            {
                return this.GetServiceImplementation(type);
            }

            var constructor = type.GetConstructors().First();

            var parameters =
                from parameter in constructor.GetParameters()
                select CreateInstance(parameter.ParameterType);

            return Activator.CreateInstance(type, parameters.ToArray());
        }

        private bool Contains(Type t) {
            return _services.ContainsKey(t);
        }

        private object GetServiceImplementation(Type t) {
            return _services[t].ServiceImplementation;
        }

        #endregion

        #region ServiceInfo Class

        /// <summary>
        /// Class holding service information.
        /// </summary>
        class ServiceInfo
        {
            private readonly ServiceLocatorInstance _context;
            readonly Type _serviceImplementationType;
            readonly bool _isSingleton;
            object _serviceImplementation;
            

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceInfo"/> class.
            /// </summary>
            /// <param name="serviceImplementationType">Type of the service implementation.</param>
            /// <param name="isSingleton">Whether the service is a Singleton.</param>
            public ServiceInfo(ServiceLocatorInstance context, Type serviceImplementationType, bool isSingleton)
            {
                _context = context;
                _serviceImplementationType = serviceImplementationType;
                _isSingleton = isSingleton;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceInfo"/> class.
            /// </summary>
            /// <param name="serviceImplementation">Instance. of service</param>
            /// <param name="type"> </param>
            public ServiceInfo(ServiceLocatorInstance context, object serviceImplementation)
            {
                _context = context;
                _serviceImplementationType = serviceImplementation != null ? serviceImplementation.GetType() : null;
                _serviceImplementation = serviceImplementation;
                _isSingleton = true;
            }

            /// <summary>
            /// Gets the service implementation.
            /// </summary>
            public object ServiceImplementation {
                get {
                    if (_isSingleton) {
                        if (_serviceImplementation == null && _serviceImplementationType != null)
                        {
                            _serviceImplementation = _context.CreateInstance(_serviceImplementationType);
                        }
                        return _serviceImplementation;
                    } else {
                        return _context.CreateInstance(_serviceImplementationType);
                    }
                }
            }
        }

        #endregion

    }

    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type serviceInterface)
            : base(string.Format("Interface {0} was not found withhin this ServiceLocator!",serviceInterface.ToString())) {
        }
    }
}
