using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Services
{
    public interface IServiceLocator
    {
        bool Contains(Type t);
        object GetServiceImplementation(Type t);
        void RegisterInstance<TInterface, TImplemention>(TImplemention instance) where TImplemention : TInterface;
        void Register<TInterface, TImplemention>() where TImplemention : TInterface;
        void RegisterSingleton<TInterface, TImplemention>() where TImplemention : TInterface;
        TInterface Resolve<TInterface>();
    }
}
