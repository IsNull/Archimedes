using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Services
{
    /// <summary>
    /// Global service locator.
    /// Singleton
    /// </summary>
    [Obsolete("Consider using Unity or MEF for Dependency Injection", true)]
    public class ServiceLocator : ServiceLocatorInstance
    {
        private readonly static ServiceLocator _instance = new ServiceLocator();

        ServiceLocator() { }
        static ServiceLocator() { }

        public static ServiceLocator Instance {
            get { return _instance; }
        }

        /// <summary>
        /// Resolve the current Interface to an service instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveService<T>(){
            return Instance.Resolve<T>();
        }
    }
}
