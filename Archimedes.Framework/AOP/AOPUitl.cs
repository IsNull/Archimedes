using System;
using System.Linq;
using Archimedes.DI.AOP;

namespace Archimedes.Framework.AOP
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


        public static PrimaryAttribute GetPrimaryAttribute(Type impl)
        {
            return (PrimaryAttribute) impl.GetCustomAttributes(PrimaryAttribute, false).FirstOrDefault();
        }
    }
}
