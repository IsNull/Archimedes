using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;
using Archimedes.Framework.AOP;

namespace Archimedes.Patterns.Tests.ConfigurationTest
{
    [Service]
    internal class ServiceConfigTarget
    {
        [Value("${test.simpleString}")]
        public string simpleStringValue;

        [Value("${test.simpleNumber}")]
        public int simpleIntValue;

        [Value("${test.nullableNumber}")]
        public int? nullableNumber;

    }
}
