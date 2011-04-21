using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace Archimedes.Patterns.Data
{
    /// <summary>
    /// Stack, limited to a item count. If the stack is full, first added elements are droped to make space for the new one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackLimited<T> : IEnumerable<T>, ICloneable
    {
        #region Fields

        protected uint maxSize;
        protected List<T> stack;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Stack limited to the given item <param name="limit">Limit</param>
        /// </summary>
        /// <param name="limit">The Limit of the Stack.</param>
        public StackLimited(uint limit) {
            maxSize = limit;
            stack = new List<T>();
        }

        /// <summary>
        /// Creates an infinite Stack. 
        /// (Unlike the default stack, this stack can later be limted in setting the Limit Property.)
        /// </summary>
        public StackLimited() : this(0) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Count of items in this Stack.
        /// </summary>
        public int Count {
            get {
                return stack.Count;
            }
        }

        /// <summary>
        /// Get or Set the Limit of items in this stack.
        /// </summary>
        public virtual uint Limit {
            get { return maxSize; }

            set {
                maxSize = value;
                if (IsLimited) {
                    if (maxSize < stack.Count) {
                        RemoveRange(stack.Count - 1, stack.Count - (int)maxSize);
                    }
                }
            }
        }

        /// <summary>
        /// Is this stack limited to specific item count?
        /// </summary>
        public bool IsLimited {
            get {
                return (maxSize > 0);
            }
        }

        #endregion

        #region Public Stack Methods

        /// <summary>
        /// Inserts an object at the top of the Stack.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Push(T item) {

            if (IsLimited) {
                if (stack.Count == maxSize)
                    RemoveAt(stack.Count - 1);
            }
            Insert(0, item);
        }


        /// <summary>
        /// Removes and returns the object at the top of the Stack.
        /// </summary>
        /// <returns></returns>
        public virtual T Pop() {
            if (stack.Count > 0) {
                var data = stack[0];
                RemoveAt(0);
                return data;
            } else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the object at the top of the Stack without removing it.
        /// </summary>
        /// <returns></returns>
        public virtual T Peek() {
            if (stack.Count > 0) {
                var data = stack[0];
                return data;
            } else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Clears the whole stack
        /// </summary>
        public virtual void ClearStack() {
            Clear();
        }

        #endregion

        protected virtual void Clear() {
            stack.Clear();
        }

        protected virtual void Insert(int index, T item) {
            stack.Insert(index, item);
        }

        protected virtual void RemoveAt(int index) {
            stack.RemoveAt(index);
        }

        protected virtual void RemoveRange(int index, int count) {
            stack.RemoveRange(index, count);
        }

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)stack).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)stack).GetEnumerator();
        }

        #endregion

        public object Clone() {
            return new StackLimited<T>();
        }
    }
}
