using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Archimedes.Patterns.Services
{
    /// <summary>
    /// Manages a set of implementations of the same interface.
    /// The implementations are identified by a string key.
    /// </summary>
    /// <typeparam name="T">The interface of the type</typeparam>
    public class ImplementationRegistry<T>
        where T : class
    {
        private readonly Dictionary<string, Implementation> _implementations = new Dictionary<string,Implementation>();


        public void RegisterSingletonImplementation<TImplementation>(string implementationKey)
            where TImplementation : T, new()
        {
            _implementations.Add(implementationKey, new Implementation(typeof(TImplementation), true));
        }

        [DebuggerStepThrough]
        public T ResolveInstance(string implementationKey)
        {
            if (implementationKey == null) throw new ArgumentNullException("implementationKey");

            if (_implementations.ContainsKey(implementationKey))
            {
                var implementation = _implementations[implementationKey];
                return implementation.GetImplementation();
            }
            else
            {
                throw new ImplementationKeyNotFound("Can not find a registered implementation with key '" + implementationKey + "'! Did you forget to register this type?");
            }
        }



        class Implementation
        {
            private readonly bool _isSingleton;
            private readonly Type _implementationType;
            private T _instance = null;

            public Implementation(Type implementationType, bool isSingleton)
            {
                if (implementationType == null) throw new ArgumentNullException("implementationType");

                _implementationType = implementationType;
                _isSingleton = isSingleton;
            }


            public T GetImplementation()
            {
                if (_instance == null)
                {
                    object createdInstance = Activator.CreateInstance(_implementationType);

                    var instance = createdInstance as T;
                    if (instance != null)
                    {
                        if (_isSingleton)
                        {
                            _instance = instance;
                        }
                        return instance;
                    }

                    throw new NotSupportedException("The registered implementation interface does not match the expected interface!");
                }
                return _instance;
            }
        }

    }

    /// <summary>
    /// Thrown when a implementation key could not be found in the implementation registry.
    /// </summary>
    [Serializable]
    public class ImplementationKeyNotFound : Exception
    {
        public ImplementationKeyNotFound()
        {
        }

        public ImplementationKeyNotFound(string message) : base(message)
        {
        }

        public ImplementationKeyNotFound(string message, Exception inner) : base(message, inner)
        {
        }

        protected ImplementationKeyNotFound(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
