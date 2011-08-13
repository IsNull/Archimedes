using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Archimedes.Patterns.Data
{
    public interface IPersistentList<T> : IList<T>, ICollection
        where T : class
    {
        Action<IList<T>> AfterAdd { get; set; }
        Action<IList<T>> AfterRemove { get; set; }
        Func<IList<T>, T, bool> BeforeAdd { get; set; }
        Func<IList<T>, T, bool> BeforeRemove { get; set; }
    }


    public class PersistentList<T> : IPersistentList<T>
    where T : class
    {
        readonly IList<T> _innerList;
        Action<IList<T>> _afterAdd;
        Action<IList<T>> _afterRemove;
        Func<IList<T>, T, bool> _beforeAdd;
        Func<IList<T>, T, bool> _beforeRemove;

        public PersistentList(IList<T> actual) {
            _innerList = actual;
        }

        public PersistentList() {
            _innerList = new List<T>();
        }

        /// <summary>
        ///     perform actions on one or more list items after an item is
        ///     added.
        /// </summary>
        public Action<IList<T>> AfterAdd {
            get { return _afterAdd ?? (_afterAdd = l => { }); }
            set { _afterAdd = value; }
        }

        /// <summary>
        ///     perform actions on one or more list items after an item is
        ///     removed.
        /// </summary>
        public Action<IList<T>> AfterRemove {
            get { return _afterRemove ?? (_afterRemove = l => { }); }
            set { _afterRemove = value; }
        }

        /// <summary>
        ///     perform a check on the item being added before adding it.
        ///     Return true if it should be added, false if it should not be
        ///     added.
        /// </summary>
        public Func<IList<T>, T, bool> BeforeAdd {
            get { return _beforeAdd ?? (_beforeAdd = (l, x) => true); }
            set { _beforeAdd = value; }
        }

        /// <summary>
        ///     perform a check on the item being removed before removing
        ///     it. Return true if it should be removed, false if it should not
        ///     be removed.
        /// </summary>
        public Func<IList<T>, T, bool> BeforeRemove {
            get { return _beforeRemove ?? (_beforeRemove = (l, x) => true); }
            set { _beforeRemove = value; }
        }

        public IEnumerator<T> GetEnumerator() {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T item) {
            if (BeforeAdd(this, item)) {
                _innerList.Add(item);
                AfterAdd(this);
            }
        }

        public void Clear() {
            while (_innerList.Any()) {
                RemoveAt(0);
            }
        }

        public bool Contains(T item) {
            return _innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _innerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            if (BeforeRemove(this, item)) {
                bool toReturn = _innerList.Remove(item);
                AfterRemove(this);
                return toReturn;
            }
            return true;
        }

        public int Count {
            get { return _innerList.Count; }
        }

        void ICollection.CopyTo(Array array, int index) {
            var copy = new T[_innerList.Count];
            _innerList.CopyTo(copy, 0);
            Array.Copy(copy, 0, array, index, _innerList.Count);
        }

        object ICollection.SyncRoot {
            get { throw new NotImplementedException(); }
        }

        bool ICollection.IsSynchronized {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly {
            get { return _innerList.IsReadOnly; }
        }

        public int IndexOf(T item) {
            return _innerList.IndexOf(item);
        }

        public void Insert(int index, T item) {
            if (BeforeAdd(this, item)) {
                _innerList.Insert(index, item);
                AfterAdd(this);
            }
        }

        public void RemoveAt(int index) {
            if (BeforeRemove(this, _innerList[index])) {
                _innerList.RemoveAt(index);
                AfterRemove(this);
            }
        }

        public T this[int index] {
            get { return _innerList[index]; }
            set {
                // this is problematic because BeforeAdd has to act on the
                // new item and the existing list MINUS the item at index.
                var copyWithoutItemAtIndex = new List<T>(_innerList);
                copyWithoutItemAtIndex.RemoveAt(index);

                if (BeforeAdd(copyWithoutItemAtIndex, value)) {
                    _innerList[index] = value;
                    AfterAdd(this);
                }
            }
        }
    }

}
