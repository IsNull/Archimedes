using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Services
{
    /// <summary>
    /// Basic Service resolving service
    /// </summary>
    public interface IServiceLocator
    {
        void RegisterInstance<TInterface>(TInterface instance);
        void Register<TInterface, TImplemention>() where TImplemention : TInterface, new();
        void RegisterSingleton<TInterface, TImplemention>() where TImplemention : TInterface, new();

        TInterface Resolve<TInterface>();
    }
}
