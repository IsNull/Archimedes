using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Archimedes.Patterns.Services
{
    public class ServiceLocatorInstance : IServiceLocator
    {
        Dictionary<Type, ServiceInfo> services = new Dictionary<Type, ServiceInfo>();

        /// <summary>
        /// Registers a service.
        /// </summary>
        public void Register<TInterface, TImplemention>() 
            where TImplemention : TInterface, new() {
            Register<TInterface, TImplemention>(false);
        }


        /// <summary>
        /// Registers a service as a singleton.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplemention>() 
            where TImplemention : TInterface, new() {
            Register<TInterface, TImplemention>(true);
        }

        /// <summary>
        /// Registers a service with specific instance (this will automatically be handled as a singleton)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplemention"></typeparam>
        /// <param name="instance"></param>
        public void RegisterInstance<TInterface, TImplemention>(TImplemention instance) where TImplemention : TInterface{
            services.Add(typeof(TInterface), new ServiceInfo(instance));
        }

        /// <summary>
        /// Resolves a service.
        /// </summary>
        [DebuggerStepThrough]
        public TInterface Resolve<TInterface>() {
            if (!services.ContainsKey(typeof(TInterface))) {
                throw new ServiceNotFoundException(typeof(TInterface));
            }
            return (TInterface)services[typeof(TInterface)].ServiceImplementation;
        }


        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <param name="isSingleton">true if service is Singleton; otherwise false.</param>
        void Register<TInterface, TImplemention>(bool isSingleton) where TImplemention : TInterface {
            services.Add(typeof(TInterface), new ServiceInfo(typeof(TImplemention), isSingleton));
        }

        public bool Contains(Type t) {
            return services.ContainsKey(t);
        }

        public object GetServiceImplementation(Type t) {
            return services[t].ServiceImplementation;
        }

        #region ServiceInfo Class

        /// <summary>
        /// Class holding service information.
        /// </summary>
        class ServiceInfo
        {
            Type _serviceImplementationType;
            object _serviceImplementation;
            bool _isSingleton;


            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceInfo"/> class.
            /// </summary>
            /// <param name="serviceImplementationType">Type of the service implementation.</param>
            /// <param name="isSingleton">Whether the service is a Singleton.</param>
            public ServiceInfo(Type serviceImplementationType, bool isSingleton) {
                _serviceImplementationType = serviceImplementationType;
                _isSingleton = isSingleton;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceInfo"/> class.
            /// </summary>
            /// <param name="serviceImplementation">Instance. of service</param>
            public ServiceInfo(object serviceImplementation) {
                _serviceImplementationType = serviceImplementation.GetType();
                _serviceImplementation = serviceImplementation;
                _isSingleton = true;
            }

            /// <summary>
            /// Gets the service implementation.
            /// </summary>
            public object ServiceImplementation {
                get {
                    if (_isSingleton) {
                        if (_serviceImplementation == null) {
                            _serviceImplementation = CreateInstance(_serviceImplementationType);
                        }
                        return _serviceImplementation;
                    } else {
                        return CreateInstance(_serviceImplementationType);
                    }
                }
            }


            /// <summary>
            /// Creates an instance of a specific type.
            /// </summary>
            /// <param name="type">The type of the instance to create.</param>
            private object CreateInstance(Type type) {
                if (ServiceLocator.Instance.Contains(type)) {
                    return ServiceLocator.Instance.GetServiceImplementation(type);
                }

                ConstructorInfo ctor = type.GetConstructors().First();

                var parameters =
                    from parameter in ctor.GetParameters()
                    select CreateInstance(parameter.ParameterType);

                return Activator.CreateInstance(type, parameters.ToArray());
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
