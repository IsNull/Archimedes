using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Services
{
    /// <summary>
    /// Basic Service resolving service
    /// </summary>
    [Obsolete("Consider using Unity or MEF for Dependency Injection")]
    public interface IServiceLocator
    {
        void RegisterInstance<TInterface>(TInterface instance);
        void Register<TInterface, TImplemention>() where TImplemention : TInterface;
        void RegisterSingleton<TInterface, TImplemention>() where TImplemention : TInterface;

        TInterface Resolve<TInterface>();
    }
}
