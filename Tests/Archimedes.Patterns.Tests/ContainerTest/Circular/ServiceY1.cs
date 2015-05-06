using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    [Service]
    public class ServiceY1
    {
        public ServiceY1(ServiceY2 serviceY2)
        {
            
        }
    }


}
