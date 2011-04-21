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
    public class ServiceLocator : ServiceLocatorInstance
    {
        private readonly static ServiceLocator _instance = new ServiceLocator();

        ServiceLocator() { }
        static ServiceLocator() { }

        public static ServiceLocator Instance {
            get { return _instance; }
        }
    }
}
