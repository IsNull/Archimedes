using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ContainerTest.Primary
{
    class PrimaryHandlingTest
    {
        [TestCase]
        public void TestPrimary()
        {
            var context = new ElderBox(GetConfiguration());
            var serviceA = context.Resolve<ServiceA>();
        }

        [TestCase]
        public void TestPrimaryScoped()
        {
            var context = new ElderBox(GetConfiguration());
            var serviceA = context.Resolve<ServiceB>();
        }

        private AutoModuleConfiguration GetConfiguration()
        {
            return new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
        }
    }
}
