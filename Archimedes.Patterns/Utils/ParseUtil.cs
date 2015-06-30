using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Utils
{
    /// <summary>
    /// Provides simple parse methods for primitives.
    /// 
    /// The difference to the build in .NET parse methods is, that in case of an erorr more detailed exceptions are thrown which 
    /// help to identify the problem.
    /// </summary>
    public static class ParseUtil
    {
        /// <summary>
        /// Parses the given string to an integer
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid integer number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseInt(string str)
        {
            int value;
            if (int.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an integer!", str));
        }

        /// <summary>
        /// Parses the given string to an long
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid long number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ParseLong(string str)
        {
            long value;
            if (long.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an long!", str));
        }

        /// <summary>
        /// Parses the given string to an double
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid double number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseDouble(string str)
        {
            double value;
            if (double.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an double!", str));
        }


        /// <summary>
        /// Parses the given string to an float
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid float number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ParseFloat(string str)
        {
            float value;
            if (float.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an float!", str));
        }


        /// <summary>
        /// Parses the given string to an decimal
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid decimal number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(string str)
        {
            decimal value;
            if (decimal.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an decimal!", str));
        }


        /// <summary>
        /// Parses the given string to an boolean
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid boolean</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ParseBool(string str)
        {
            bool value;
            if (bool.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an bool!", str));
        }

        /// <summary>
        /// Parses the given string to an double
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid double number</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ParseGuid(string str)
        {
            Guid value;
            if (Guid.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an Guid!", str));
        }


        /// <summary>
        /// Parses the given string to an Enum Value
        /// </summary>
        /// <exception cref="FormatException">Thrown when the string is not a valid enum member of the specified Enum Type</exception>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string str)
            where T : struct, IConvertible
        {
            T value;

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }


            if (Enum.TryParse(str, out value))
            {
                return value;
            }
            throw new FormatException(string.Format("Failed to parse string '{0}' to an decimal!", str));
        }

    }
}
