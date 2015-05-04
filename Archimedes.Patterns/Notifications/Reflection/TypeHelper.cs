using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Reflection
{
    public static class TypeHelper
    {
        static Type[] NUMERIC_TYPES = new Type[]
            {
                typeof(System.Byte),
                typeof(System.SByte),
                typeof(System.Int16),
                typeof(System.UInt16),
                typeof(System.Int32),
                typeof(System.UInt32),
                typeof(System.Int64),
                typeof(System.UInt64),
                typeof(System.Single),
                typeof(System.Double),
                typeof(System.Decimal)
            };

        public static Type[] GetAllNumericTypes() { return NUMERIC_TYPES; }

        public static bool IsNumeric(object obj) {
            return TypeHelper.NUMERIC_TYPES.Contains(obj.GetType());
        }

        [DebuggerStepThrough]
        public static bool IsNumeric(Type type) {
            if (type == null) throw new ArgumentNullException("type");

            return TypeHelper.NUMERIC_TYPES.Contains(type);
        }

        [DebuggerStepThrough]
        public static bool IsNullableType(Type type)
        {
            if(type == null) throw new ArgumentNullException("type");

            return (type.IsGenericType && type.
                GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        [DebuggerStepThrough]
        public static bool IsTypeOrUnderlingType(Type type, Type needle) {
            if (type == null) throw new ArgumentNullException("type");
            if (type == null) throw new ArgumentNullException("needle");

            if (IsNullableType(type))
                type = Nullable.GetUnderlyingType(type);
            return type == needle;
        }

        [DebuggerStepThrough]
        public static object ConvertOrDefault(object source, Type destination, bool instantiateNullables = true) {

            if (destination == null) throw new ArgumentNullException("destination");


            object newvalue;

            Type destinationType = destination;
            if (TypeHelper.IsNullableType(destinationType))
                destinationType = Nullable.GetUnderlyingType(destinationType);
            try {
                newvalue = Convert.ChangeType(source, destinationType);
            } catch {
                if (instantiateNullables)
                    newvalue = destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;
                else
                    newvalue = destination.IsValueType ? Activator.CreateInstance(destination) : null;
            }
            return newvalue;
        }


    }
}
