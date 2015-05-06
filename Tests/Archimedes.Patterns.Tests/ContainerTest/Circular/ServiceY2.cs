using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    [Service]
    public class ServiceY2
    {
        public ServiceY2(ServiceY3 serviceY2)
        {

        }
    }
}
