using Archimedes.Framework;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ContainerTest.Circular
{
    public class ContainerCircularProtectionTest
    {
        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtection()
        {
            var conf = new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
            var container = new ElderBox(conf);

            container.Resolve<ServiceY1>();
        }

        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtection2()
        {
            var conf = new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
            var container = new ElderBox(conf);

            container.Resolve<ServiceY2>();
        }


        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtectionAutowiring()
        {
            var conf = new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
            var container = new ElderBox(conf);

            var service = container.Resolve<ServiceA1>();

            var s2 = service;
        }
    }
}
