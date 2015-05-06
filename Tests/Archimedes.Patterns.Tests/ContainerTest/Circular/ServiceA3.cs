using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    [Service]
    public class ServiceA3
    {
        [Inject]
        private ServiceA1 service1;

        public ServiceA3()
        {

        }
    }
}
