using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    [Service]
    public class ServiceD
    {
        [Inject]
        [DebuggerStepThrough]
        public ServiceD(ServiceA serviceA, IServiceB serviceB, ServiceY serviceY)
        {
            if(serviceA == null) throw new ArgumentNullException("serviceA");
            if(serviceB == null) throw new ArgumentNullException("serviceB");

            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.ServiceY = serviceY;
        }

        public ServiceA serviceA;
        public IServiceB serviceB;
        public ServiceY ServiceY;
    }
}
