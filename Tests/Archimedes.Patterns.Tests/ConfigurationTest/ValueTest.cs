using System;
using System.Collections.Generic;
using System.Globalization;
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

            var date = DateTime.Now;
            configuration.SetParameterValue("test.simpleDate", date.ToString(CultureInfo.InvariantCulture));


            var valueService = context.Resolve<ServiceConfigTarget>();

            Assert.AreEqual("MyTest", valueService.simpleStringValue);
            Assert.AreEqual(12, valueService.simpleIntValue);
            Assert.AreEqual(33, valueService.nullableNumber);


            Assert.AreEqual(date.Year, valueService.simpleDate.Year);
            Assert.AreEqual(date.Month, valueService.simpleDate.Month);
            Assert.AreEqual(date.Day, valueService.simpleDate.Day);
            Assert.AreEqual(date.Hour, valueService.simpleDate.Hour);
            Assert.AreEqual(date.Minute, valueService.simpleDate.Minute);
            Assert.AreEqual(date.Second, valueService.simpleDate.Second);

        }


        
    }
}
