using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns
{
    /// <summary>
    /// A immutable container object which may or may not contain a non-null value. 
    /// If a value is present, IsPresent will return true and Get() will return the value. 
    /// 
    /// Use the static builder methods to create instances of this class.
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Optional<T>
    {
        #region Static Builder

        public static Optional<T> Of(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T nullable)
        {
            return nullable != null ? Of(nullable) : new Optional<T>();
        }

        /// <summary>
        /// Returns a Optional which does not contain a value.
        /// </summary>
        /// <returns></returns>
        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }

        #endregion

        #region Fields

        private readonly T _value;
        private readonly bool _isPresent;

        #endregion

        /// <summary>
        /// Creates a Maybe
        /// </summary>
        private Optional()
        {
            _isPresent = false;
        }

        private Optional(T value)
        {
            _value = value;
            _isPresent = true;
        }

        /// <summary>
        /// Gets the value if available, otherwise throws <see cref="InvalidOperationException "/>
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (!IsPresent) throw new InvalidOperationException("You invoked Get() on an Optional<T> which values is NOT present. Use IsPresent to check or any of the 'OrElse' methods!");
            return _value;
        }

        /// <summary>
        /// Returns the value if available, or else the provided value as fall-back.
        /// </summary>
        /// <param name="elseValue"></param>
        /// <returns></returns>
        public T OrElse(T elseValue)
        {
            if (IsPresent) return Get();
            return elseValue;
        }

        /// <summary>
        /// Returns the value if available, or else the provided value as fall-back.
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="elseProvider"></param>
        /// <returns></returns>
        public T OrElse(Func<T> elseProvider)
        {
            return elseProvider();
        }

        /// <summary>
        /// Returns the value if available, or else the default value of the type -
        /// Which is null for all classes, and empty/zero for all structures.
        /// </summary>
        /// <returns></returns>
        public T OrDefault()
        {
            if (IsPresent) return Get();
            return default(T);
        }

        /// <summary>
        /// Checks if the value is present.
        /// Note that this does not guarantee that the value is NOT null!
        /// </summary>
        public bool IsPresent
        {
            get
            {
                return _isPresent;
            }
        }

        public override string ToString()
        {
            return IsPresent ? Get() + "" : "{Empty-Maybe}";
        }

        /// <summary>
        /// Maps this value if present, using the given mapper function.
        /// Otherwise, an empty optional is returned.
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public Optional<TD> MapOptional<TD>(Func<T, TD> mapper)
        {
            if (IsPresent)
            {
                return Optional<TD>.OfNullable(mapper(Get()));
            }

            return Optional<TD>.Empty();
        }
    }
}
