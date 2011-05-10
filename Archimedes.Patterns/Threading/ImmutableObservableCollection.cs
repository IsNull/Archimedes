using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Specialized;

namespace Archimedes.Patterns.Threading
{
   
    public class ThreadMutableObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        #region Fields

        private IList<T> _collection;
        private ReaderWriterLock _syncLock = new ReaderWriterLock();

        #endregion

        /// <summary>
        /// Raised when the Collection has changed
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Constructor

        public ThreadMutableObservableCollection() {
            _collection = new List<T>();
        }

        public ThreadMutableObservableCollection(IEnumerable<T> items) {
            _collection = new List<T>(items);
        }

        #endregion

        public void Add(T item) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.Add(item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            _syncLock.ReleaseWriterLock();
        }

        public void Clear() {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.Clear();
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _syncLock.ReleaseWriterLock();
        }

        public bool Contains(T item) {
            _syncLock.AcquireReaderLock(Timeout.Infinite);
            var result = _collection.Contains(item);
            _syncLock.ReleaseReaderLock();
            return result;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.CopyTo(array, arrayIndex);
            _syncLock.ReleaseWriterLock();
        }

        public int Count {
            get {
                _syncLock.AcquireReaderLock(Timeout.Infinite);
                var result = _collection.Count;
                _syncLock.ReleaseReaderLock();
                return result;
            }
        }

        public bool IsReadOnly {
            get { return _collection.IsReadOnly; }
        }

        public bool Remove(T item) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            var index = _collection.IndexOf(item);
            if (index == -1) {
                _syncLock.ReleaseWriterLock();
                return false;
            }
            var result = _collection.Remove(item);
            if (result && CollectionChanged != null)
                CollectionChanged(this, new
                    NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _syncLock.ReleaseWriterLock();
            return result;
        }

        public IEnumerator<T> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _collection.GetEnumerator();
        }

        public int IndexOf(T item) {
            _syncLock.AcquireReaderLock(Timeout.Infinite);
            var result = _collection.IndexOf(item);
            _syncLock.ReleaseReaderLock();
            return result;
        }

        public void Insert(int index, T item) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            _syncLock.ReleaseWriterLock();
        }

        public void RemoveAt(int index) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            if (_collection.Count == 0 || _collection.Count <= index) {
                _syncLock.ReleaseWriterLock();
                return;
            }
            _collection.RemoveAt(index);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            _syncLock.ReleaseWriterLock();
        }

        public T this[int index] {
            get {
                _syncLock.AcquireReaderLock(Timeout.Infinite);
                var result = _collection[index];
                _syncLock.ReleaseReaderLock();
                return result;
            }
            set {
                _syncLock.AcquireWriterLock(Timeout.Infinite);
                if (_collection.Count == 0 || _collection.Count <= index) {
                    _syncLock.ReleaseWriterLock();
                    return;
                }
                _collection[index] = value;
                _syncLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Returns a immutable Snapshot from this immutable collection
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetSnapshot() {
            IEnumerable<T> snapshot;
            _syncLock.AcquireReaderLock(Timeout.Infinite);
            snapshot = new List<T>(_collection);
            _syncLock.ReleaseReaderLock();
            return snapshot;
        }

    }
}
