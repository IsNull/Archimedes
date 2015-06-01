using System;

namespace Archimedes.Framework.AOP
{
    /// <summary>
    /// This attribute provides the ability to inject configuration values into fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ValueAttribute : Attribute
    {
        public ValueAttribute(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; private set; }
    }
}
