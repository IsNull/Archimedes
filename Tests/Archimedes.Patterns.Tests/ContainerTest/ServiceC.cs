using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Patterns.Container;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    [Service]
    public class ServiceC
    {
        [Inject]
        public ServiceC(ServiceA serviceA, ServiceB serviceB)
        {
            if(serviceA == null) throw new ArgumentNullException("serviceA");
            if(serviceB == null) throw new ArgumentNullException("serviceB");

            this.serviceA = serviceA;
            this.serviceB = serviceB;
        }

        public ServiceA serviceA;
        public ServiceB serviceB;
    }
}
