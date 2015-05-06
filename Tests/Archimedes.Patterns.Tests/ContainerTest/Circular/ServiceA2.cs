using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    [Service]
    public class ServiceA2
    {
        [Inject]
        private ServiceA3 serviceY2;

        public ServiceA2()
        {

        }
    }
}
