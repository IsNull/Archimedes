using System;

namespace Archimedes.Framework.AOP
{
    /// <summary>
    /// This attribute provides the ability to inject configuration values into fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ValueAttribute : Attribute
    {
        private readonly string _expression;

        public ValueAttribute(string expression)
        {
            _expression = expression;
        }

        public string Expression
        {
            get { return _expression; }
        }
    }
}
