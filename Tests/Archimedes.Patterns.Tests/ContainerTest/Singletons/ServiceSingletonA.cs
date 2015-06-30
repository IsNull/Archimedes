using System;

namespace Archimedes.Patterns.Tests.ContainerTest.Singletons
{
    class ServiceSingletonA
    {
        public ServiceSingletonA()
        {
            SingletonsTest.IncrementInstance();
        }
    }
}
