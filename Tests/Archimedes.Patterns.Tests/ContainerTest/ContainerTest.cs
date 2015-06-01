using System.Linq;
using Archimedes.DI.AOP;
using Archimedes.Framework;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    public class ContainerTest
    {
        [Inject] private ServiceA _serviceA;

        [Inject] private IServiceB _serviceB;

        [TestCase]
        public void TestComponentScan()
        {
            var components = ApplicationContext.Instance.ScanComponents("Archimedes.*").ToList();

            Assert.True(components.Contains(typeof (ServiceA)), "");
            Assert.True(components.Contains(typeof (ServiceB)), "");
            Assert.True(components.Contains(typeof (ServiceC)), "");
            Assert.True(components.Contains(typeof (ServiceD)), "");
        }


        [TestCase]
        public void TestAutoConfiguration()
        {
            var components = ApplicationContext.Instance.ScanComponents("Archimedes.*").ToList();
            var conf = new AutoModuleConfiguration(components);
        }


        [TestCase]
        public void TestSimpleConstructorWiring()
        {

            var context = new ElderBox(GetConfiguration());

            var instance = context.Resolve<ServiceC>();
        }


        [TestCase]
        public void TestSimpleInstanceWiring()
        {

            var context = new ElderBox(GetConfiguration());


            context.Autowire(this);

            Assert.IsNotNull(_serviceA, "Failed to autowire instance!");
            Assert.IsNotNull(_serviceB, "Failed to autowire instance!");
        }

        [TestCase]
        public void TestInterfaceConstructorWiring()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceD>();
        }


        [TestCase]
        [ExpectedException(typeof(AmbiguousMappingException))]
        public void TestAmbigous()
        {
            var context = new ElderBox(GetConfiguration());

            var imp = context.Resolve<IServiceAmbig>();
        }

        [TestCase]
        public void TestConstructorByPassing()
        {
            var context = new ElderBox(GetConfiguration());

            var imp = context.Resolve<ServiceX>();
        }

        private AutoModuleConfiguration GetConfiguration()
        {
            return new AutoModuleConfiguration(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
        }

    }
}
