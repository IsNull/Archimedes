using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.Patterns.Tests.ContainerTest.Primary
{
    [Service]
    public class ServiceCandidate1 : ObjectDummy, IServiceCandidate
    {

    }
}
