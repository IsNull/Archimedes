using System.Linq;
using Archimedes.Patterns.Container;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ContainerTest
{
    public class ContainerTest
    {
        [Inject]
        private ServiceA _serviceA;



        [TestCase]
        public void TestComponentScan()
        {
            var components = ApplicationContext.Instance.AutoFindComponents().ToList();

            Assert.True(components.Contains(typeof(ServiceA)),"");
            Assert.True(components.Contains(typeof(ServiceB)), "");
            Assert.True(components.Contains(typeof(ServiceC)), "");
            Assert.True(components.Contains(typeof(ServiceD)), "");
        }

        [TestCase]
        public void TestAutoConfiguration()
        {
            var components = ApplicationContext.Instance.AutoFindComponents().ToList();
            var conf = new AutoModuleConfiguration(components);
        }


        [TestCase]
        public void TestSimpleConstructorWiring()
        {

            var context = new ElderBox(new AutoModuleConfiguration(ApplicationContext.Instance.AutoFindComponents()));

            var instance = context.Resolve<ServiceC>();
        }


        [TestCase]
        public void TestSimpleInstanceWiring()
        {

            var context = new ElderBox(new AutoModuleConfiguration(ApplicationContext.Instance.AutoFindComponents()));


            context.Autowire(this);

            Assert.IsNotNull(_serviceA, "Failed to autowire instance!");
        }

        /**/
        [TestCase]
        public void TestInterfaceConstructorWiring()
        {
            var context = new ElderBox(new AutoModuleConfiguration(ApplicationContext.Instance.AutoFindComponents()));
            var instance = context.Resolve<ServiceD>();
        }


    }
}
