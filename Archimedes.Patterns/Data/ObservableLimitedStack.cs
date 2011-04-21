using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Archimedes.Patterns.Data
{
    public class ObservableLimitedStack<T> : StackLimited<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableLimitedStack(uint limit) : base(limit) { }
        public ObservableLimitedStack() : base() { }

        #region Overrides to catch Stack Changes

        protected override void Insert(int index, T item) {
            base.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            InvokeChangedEvents();
        }

        protected override void RemoveAt(int index) {
            var item = stack[index];
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            base.RemoveAt(index);
            InvokeChangedEvents();
        }

        protected override void RemoveRange(int index, int count) {
            var items = stack.GetRange(index, count);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
            base.RemoveRange(index, count);
            InvokeChangedEvents();
        }

        protected override void Clear() {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            InvokeChangedEvents();
        }

        #endregion

        public override uint Limit {
            get {
                return base.Limit;
            }
            set {
                base.Limit = value;
                OnPropertyChanged("Limit");
                OnPropertyChanged("IsLimited");
            }
        }

        protected void InvokeChangedEvents() {
            OnPropertyChanged("Count");
        }

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
#if !DEBUG
            try {
#endif
            try {
                if (CollectionChanged != null)
                    CollectionChanged(this, e);
            } catch {

            }
#if !DEBUG
            } catch {
            // we dont need such exceptions in Release build...
            }
#endif
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members
    }
}
