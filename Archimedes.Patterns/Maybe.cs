using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns
{
    /// <summary>
    /// Represents a possible value.
    /// 
    /// The value can either be present or not. 
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Maybe<T>
    {
        #region Static Builder

        public static Maybe<T> FromNullable(T nullable)
        {
            return nullable != null ? new Maybe<T>(nullable) : new Maybe<T>();
        }

        
        public static Maybe<T> Empty()
        {
            return new Maybe<T>();
        }

        #endregion

        private readonly T _value;
        private readonly bool _hasValue;

        /// <summary>
        /// Creates a Maybe
        /// </summary>
        private Maybe()
        {
            _hasValue = false;
        }

        public Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Gets the value if available
        /// </summary>
        /// <returns></returns>
        public T GetValue()
        {   // TODO Probably throw here exception if no value set
            return _value;
        }

        /// <summary>
        /// Returns the value if available, or else the provided value as fall-back.
        /// </summary>
        /// <param name="elseValue"></param>
        /// <returns></returns>
        public T OrElse(T elseValue)
        {
            if (HasValue) return GetValue();
            return elseValue;
        }

        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
            
        }

        public override string ToString()
        {
            return HasValue ? GetValue() + "" : "{Empty-Maybe}";
        }
    }
}
