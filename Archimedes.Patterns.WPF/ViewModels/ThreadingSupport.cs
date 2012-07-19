using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public static class ThreadingSupport
    {

        /// <summary>
        /// Executes a Method async in the default Dispatcher (running on the standard GUI Thread) 
        /// Returns imediatly after the Dispatcher-Task has been created (non-blocking)
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        public static void SyncInvokeBegin(Action method, DispatcherPriority priority = DispatcherPriority.Normal) {
            Application app = Application.Current;
            if (app != null) {
                if (!app.Dispatcher.CheckAccess()) {
                    app.Dispatcher.BeginInvoke(method, priority);
                } else
                    method();
            }
        }

        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread) and blocks until it has completed
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        public static void SyncInvoke(Action method, DispatcherPriority priority = DispatcherPriority.Normal) {
            Application app = Application.Current;
            if (app != null) {
                if (!app.Dispatcher.CheckAccess()) {
                    app.Dispatcher.Invoke(method, priority);
                } else
                    method();
            }
        }


        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread) and returns the Result
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        /// <returns></returns>
        public static T SyncInvoke<T>(Func<T> method, DispatcherPriority priority = DispatcherPriority.Normal) {
            Application app = Application.Current;
            if (app != null)
                if (!app.Dispatcher.CheckAccess()) {
                    return (T)app.Dispatcher.Invoke(method, priority);
                } else
                    return method();
            else
                return default(T);
        }

    }
}
