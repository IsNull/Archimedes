using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    [Service]
    public class ServiceX
    {
        [Inject]
        private ServiceA _serviceA;

        [Inject]
        public IServiceB ServiceB;

        public ServiceX()
        {
            if (_serviceA == null) throw new ArgumentNullException("serviceA");
            if (ServiceB == null) throw new ArgumentNullException("ServiceB");

        }


    }
}
