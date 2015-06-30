using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Archimedes.Framework;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ContainerTest.Singletons
{
    public class SingletonsTest
    {
        public static long _instanceCount;

        public static void IncrementInstance()
        {
            Interlocked.Increment(ref _instanceCount);
        }


        [TestCase]
        public void TestSingletonBehaviour1()
        {
            var context = new ElderBox(GetConfiguration());

            var instance1 = context.Resolve<ServiceSingletonA>();
            var instance2 = context.Resolve<ServiceSingletonB>();

            long actualInstances = Interlocked.Read(ref _instanceCount);
            Assert.AreEqual(1, actualInstances, "Must only create a single instance!");
        }

        /*
        [TestCase]
        public void TestSingletonBehaviour()
        {
            var context = new ElderBox(GetConfiguration());

            var instance1 = context.Resolve<ServiceSingletonB>();
            var instance2 = context.Resolve<ServiceSingletonC>();

            long actualInstances = Interlocked.Read(ref _instanceCount);
            Assert.AreEqual(1, actualInstances, "Must only create a single instance!");
        }*/


        private AutoModuleConfiguration GetConfiguration()
        {
            return new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
        }
    }
}
