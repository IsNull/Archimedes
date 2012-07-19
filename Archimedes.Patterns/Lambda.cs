using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Archimedes.Patterns
{
    /// <summary>
    /// Provides some common used static Lambda Helper Methods
    /// </summary>
    public static class Lambda
    {
        /// <summary>
        /// Returns the PropertyInfo from a Expression containing a property like: "() => MyProperty"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Expression<Func<object>> expression) {
            var lambda = expression as LambdaExpression;
            var memberExpression = lambda.Body as MemberExpression;
            if (memberExpression == null) {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            return memberExpression.Member as PropertyInfo;
        }

  
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> expression) {
            var lambda = expression as LambdaExpression;
            var memberExpression = lambda.Body as MemberExpression;
            if (memberExpression == null) {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            return memberExpression.Member as PropertyInfo;
        }


        /// <summary>
        /// Returns the PropertyName from a Expression containing a property like: "() => MyProperty"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName(Expression<Func<object>> expression) {
            var propertyInfo = GetPropertyInfo(expression);
            if (propertyInfo != null)
                return propertyInfo.Name;
            else
                return "";
        }
    }
}
