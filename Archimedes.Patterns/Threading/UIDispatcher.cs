using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Threading
{
    /// <summary>
    /// Global Dispatcher running on the default GUI Thread
    /// </summary>
    public static class UIDispatcher
    {
        static IDispatcher _UIDispatcher;


        public static void SetUIDispatcher(IDispatcher disp) {
            _UIDispatcher = disp;
        }

        /// <summary>
        /// Gets the underlying UI Dispatcher
        /// </summary>
        public static IDispatcher Dispatcher {
            get {
                return _UIDispatcher;
            }
        }


        public static void Invoke(Action method) {
            Dispatcher.Invoke(method);
        }

        public static void InvokeBegin(Action method) {
            Dispatcher.InvokeBegin(method);
        }

        public static T Invoke<T>(Func<T> method) {
            return Dispatcher.Invoke<T>(method);
        }
    }
}
