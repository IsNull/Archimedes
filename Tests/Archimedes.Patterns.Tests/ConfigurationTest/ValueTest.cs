using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Configuration;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Patterns.Tests.ConfigurationTest
{
    public class ValueTest
    {
        [TestCase]
        public void TestValue()
        {
            // "Archimedes.*"
            ApplicationContext.Instance.EnableAutoConfiguration();

            var context = ApplicationContext.Instance.GetContext();
            var configuration = context.Resolve<IConfigurationService>();
            configuration.SetParameterValue("test.simpleString", "MyTest");
            configuration.SetParameterValue("test.simpleNumber", 12.ToString());
            configuration.SetParameterValue("test.nullableNumber", 33.ToString());


            var valueService = context.Resolve<ServiceConfigTarget>();

            Assert.AreEqual("MyTest", valueService.simpleStringValue);
            Assert.AreEqual(12, valueService.simpleIntValue);
            Assert.AreEqual(33, valueService.nullableNumber);

        }


        
    }
}
