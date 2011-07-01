using System;
using System.Collections.Generic;
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
        public static bool IsNumeric(Type type) {
            return TypeHelper.NUMERIC_TYPES.Contains(type);
        }

        public static bool IsNullableType(Type type) {
            return (type.IsGenericType && type.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }

        public static bool IsTypeOrUnderlingType(Type type, Type needle) {
            if (IsNullableType(type))
                type = Nullable.GetUnderlyingType(type);
            return type.Equals(needle);
        }

    }
}
