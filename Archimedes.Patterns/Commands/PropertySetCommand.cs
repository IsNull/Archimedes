using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Represents a Property Setter which can be undone
    /// </summary>
    /// <typeparam name="T">Type of the Property</typeparam>
    public class PropertySetCommand<T> : CommandUndo
    {
        private readonly Object ctx;
        private readonly MethodInfo propertySetter;
        private readonly MethodInfo propertyGetter;

        private T previousValue;
        private T newValue;

        /// <summary>
        /// Create a new Property UnDoSupport
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyExpression"></param>
        public PropertySetCommand(Object context, Expression<Func<T>> propertyExpression) {
            this.ctx = context;
            var property = Lambda.GetPropertyInfo(propertyExpression);
            this.propertySetter = property.GetSetMethod();
            this.propertyGetter = property.GetGetMethod();
        }

        /// <summary>
        /// Clone Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertySetter"></param>
        /// <param name="previousValue"></param>
        /// <param name="newValue"></param>
        protected PropertySetCommand(Object context, MethodInfo propertySetter, MethodInfo propertyGetter, T previousValue, T newValue) {
            this.ctx = context;
            this.propertySetter = propertySetter;
            this.propertyGetter = propertyGetter;
            this.previousValue = previousValue;
            this.newValue = newValue;
        }

        public override void Execute(object args) {
            if (args is T) {
                newValue = (T)args;
                previousValue = PropertyValue;
                PropertyValue = newValue;
                base.Execute(args);
            } else
                throw new ArgumentException("args must be of type " + typeof(T).Name);
        }

        public override void UnExecute() {
            PropertyValue = previousValue;
            base.UnExecute();
        }

        public override void Redo() {
            PropertyValue = newValue;
            base.Redo();
        }

        /// <summary>
        /// Setter/getter of the remote property
        /// </summary>
        private T PropertyValue {
            set {
                propertySetter.Invoke(ctx, new Object[] { value });
            }
            get {
                return (T)propertyGetter.Invoke(ctx, null);
            }
        }

        public override CommandUndo CloneCommand() {
            return new PropertySetCommand<T>(ctx, propertySetter, propertyGetter, previousValue, newValue);
        }
    }
}
