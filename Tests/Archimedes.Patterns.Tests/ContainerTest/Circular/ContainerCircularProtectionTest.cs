using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI;
using Archimedes.DI.AOP;
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

            container.Resolve<ServiceA1>();
        }
    }
}
