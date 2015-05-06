using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Patterns.Container;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    [Service]
    public class ServiceAmbigOne : IServiceAmbig
    {
        public void Test()
        {
        }
    }

    [Service]
    public class ServiceAmbigTwo : IServiceAmbig
    {
        public void Test()
        {
        }
    }


    public interface IServiceAmbig
    {
        void Test();
    }
}
