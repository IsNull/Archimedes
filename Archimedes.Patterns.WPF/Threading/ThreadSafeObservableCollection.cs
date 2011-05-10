using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Threading;
using System.Threading;

namespace Archimedes.Patterns.WPF.Threading
{
    public class ThreadSafeObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        #region Fields

        private IList<T> _collection;
        private Dispatcher _dispatcher;
        private ReaderWriterLock _syncLock = new ReaderWriterLock();

        #endregion

        /// <summary>
        /// Raised when the Collection has changed
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Constructor

        public ThreadSafeObservableCollection() {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _collection = new List<T>();
        }

        public ThreadSafeObservableCollection(IEnumerable<T> items) {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _collection = new List<T>(items);
        }

        #endregion

        public void Add(T item) {
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoAdd(item);
            else
                _dispatcher.BeginInvoke((Action)(() => { DoAdd(item); }));
        }

        private void DoAdd(T item) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.Add(item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            _syncLock.ReleaseWriterLock();
        }

        public void Clear() {
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoClear();
            else
                _dispatcher.BeginInvoke((Action)(() => { DoClear(); }));
        }

        private void DoClear() {
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
            if (Thread.CurrentThread == _dispatcher.Thread)
                return DoRemove(item);
            else {
                var op = _dispatcher.BeginInvoke(new Func<T, bool>(DoRemove), item);
                if (op == null || op.Result == null)
                    return false;
                return (bool)op.Result;
            }
        }

        private bool DoRemove(T item) {
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
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoInsert(index, item);
            else
                _dispatcher.BeginInvoke((Action)(() => { DoInsert(index, item); }));
        }

        private void DoInsert(int index, T item) {
            _syncLock.AcquireWriterLock(Timeout.Infinite);
            _collection.Insert(index, item);
            if (CollectionChanged != null)
                CollectionChanged(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            _syncLock.ReleaseWriterLock();
        }

        public void RemoveAt(int index) {
            if (Thread.CurrentThread == _dispatcher.Thread)
                DoRemoveAt(index);
            else
                _dispatcher.BeginInvoke((Action)(() => { DoRemoveAt(index); }));
        }

        private void DoRemoveAt(int index) {
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
        /// Returns a immutable Snapshot from this threadsave collection
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
