using System;
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

        [Value("${test.simpleDate}")]
        public DateTime simpleDate;
    }
}
