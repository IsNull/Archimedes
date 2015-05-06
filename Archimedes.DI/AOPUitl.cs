using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.DI.AOP;

namespace Archimedes.DI
{
    internal class AOPUitl
    {
        private static readonly Type[] ComponentAttributs = { typeof(ServiceAttribute), typeof(ControllerAttribute), typeof(ComponentAttribute) };
        private static readonly Type PrimaryAttribute = typeof(PrimaryAttribute);


        public static bool IsTypeComponent(Type type)
        {
           return GetComponentAttribute(type) != null;
        }

        public static ComponentAttribute GetComponentAttribute(Type impl)
        {
            foreach (var componentAttribut in ComponentAttributs)
            {
                var attr = impl.GetCustomAttributes(componentAttribut, false);
                if (attr.Length > 0)
                {
                    return (ComponentAttribute)attr[0];
                }
            }
            return null;
        }


        public static bool IsPrimaryImplementation(Type impl)
        {
            return impl.IsDefined(PrimaryAttribute, false);
        }
    }
}
