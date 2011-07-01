using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Reflection;

namespace Archimedes.Patterns.Conditions
{
    public class ConditionalEmiter<T>
    {
        List<DynamicConditionExpression> _conditions = new List<DynamicConditionExpression>();

        public void AddCondition(DynamicConditionExpression condition) {
            _conditions.Add(condition);
        }

        public void RemoveCondition(DynamicConditionExpression condition) {
            _conditions.Remove(condition);
        }

        public IQueryable<T> Emit(IQueryable<T> items) {
            IQueryable<object> itemsObjs = items.Cast<object>();
            _conditions.ForEach((x) =>
            {
                itemsObjs = x.Filter(itemsObjs);
            });
            return itemsObjs.Cast<T>();
        }
    }
}
