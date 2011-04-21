using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Archimedes.Patterns
{
    public static class Lambda
    {
        /// <summary>
        /// Returns the PropertyName from a Expression containing a property like: "() => MyProperty"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName(Expression<Func<object>> expression) {
            var lambda = expression as LambdaExpression;
            var memberExpression = lambda.Body as MemberExpression;
            if (memberExpression == null) {
                var unaryExpression = (lambda.Body as UnaryExpression);
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.Name;
            else
                return "";
        }
    }
}
