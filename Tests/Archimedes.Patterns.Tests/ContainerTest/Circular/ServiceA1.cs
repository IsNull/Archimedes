using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    [Service]
    public class ServiceA1
    {
        [Inject]
        private ServiceA2 serviceA2;

        public ServiceA1()
        {
            
        }
    }


}
