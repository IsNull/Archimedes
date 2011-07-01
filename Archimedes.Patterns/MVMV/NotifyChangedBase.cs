using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Reflection;

namespace Archimedes.Patterns.MVMV
{

    /// <summary>
    /// Base Class for INotifyPropertyChanged types
    /// </summary>
    public class NotifyChangedBase : INotifyPropertyChanged
    {
        #region Fields

        static Dictionary<string, PropertyChangedEventArgs> _eventArgsMap = new Dictionary<string, PropertyChangedEventArgs>();

        Dictionary<string, List<Action<object>>> _typedInvokerMap = new Dictionary<string, List<Action<object>>>();
        PropertyChangedEventHandler _propChangedHandler;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged {
            add {
                _propChangedHandler = (PropertyChangedEventHandler)Delegate.Combine(_propChangedHandler, value);
            }
            remove {
                if (_propChangedHandler != null)
                    _propChangedHandler = (PropertyChangedEventHandler)Delegate.Remove(_propChangedHandler, value);
            }
        }

        #endregion

        /// <summary>
        /// Eventhandler which invokes the OnPropertyChanged Method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPropertyChangedInvoker(object sender, PropertyChangedEventArgs e) {
            if (_propChangedHandler != null) {
                _propChangedHandler(this, e);
            }
        }

        public void RegisterPropertyChangedEvent(Expression<Func<object>> property, Action<object> callback) {
            var name = Lambda.GetPropertyName(property);
            if (!_typedInvokerMap.ContainsKey(name)) {
                _typedInvokerMap.Add(name, new List<Action<object>>());
            }
            _typedInvokerMap[name].Add(callback);
        }

        #region Notify Helpers

        /// <summary>
        /// Notifies all listener that the given Property(s) have changed.
        /// </summary>
        /// <param name="expressions">Lambda Expression which references one ore more Property/ies</param>
        protected void OnPropertyChanged(params Expression<Func<object>>[] expressions) {
            if (expressions == null)
                throw new ArgumentNullException("expressions", "You need to provide at least one expression");
            if (expressions.Length <= 0)
                throw new ArgumentOutOfRangeException("expressions", "You need to provide at least one expression");

            string[] propertyNames = GetPropertyNames(expressions);

            OnPropertysChangedInternal(propertyNames);
        }

        string[] GetPropertyNames(Expression<Func<object>>[] expressions) {
            string[] propertyNames = new string[expressions.Length];
            for (int i = 0; i < expressions.Length; i++) {
                propertyNames[i] = Lambda.GetPropertyName(expressions[i]);
            }
            return propertyNames;
        }




        void OnPropertysChangedInternal(params string[] propertyNames) {
            if ((propertyNames == null) || (propertyNames.Length == 0))
                throw new ArgumentNullException("propertyNames");
            if (_propChangedHandler == null)
                return;

            foreach (string propertyName in propertyNames)
                OnPropertyChangedInternal(propertyName);
        }

        void OnPropertyChangedInternal(string propertyName) {
            if (_propChangedHandler != null)
                _propChangedHandler(this, GetEventArgs(propertyName));

            if (_typedInvokerMap.ContainsKey(propertyName)) {
                foreach (var a in _typedInvokerMap[propertyName])
                    a(this);
            }
        }

        static PropertyChangedEventArgs GetEventArgs(string propertyName) {
            PropertyChangedEventArgs pe = null;
            if (_eventArgsMap.TryGetValue(propertyName, out pe) == false) {
                pe = new PropertyChangedEventArgs(propertyName);
                _eventArgsMap[propertyName] = pe;
            }
            return pe;
        }

        #endregion
    }
}
