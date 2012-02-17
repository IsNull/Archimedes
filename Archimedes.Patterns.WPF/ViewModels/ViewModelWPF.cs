using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.MVMV;
using System.Windows;
using System.Windows.Threading;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public class ViewModelWPF : ViewModelBase
    {
        #region internal Sync Dispatcher Helper Methods

        /// <summary>
        /// Returns the Dispatcher of the default GUI Thread
        /// </summary>
        protected Dispatcher DefaultDispatcher {
            get { return Application.Current.Dispatcher; } //Application.Current.Dispatcher.Invoke
        }

        /// <summary>
        /// Executes a Method async in the default Dispatcher (running on the standard GUI Thread) 
        /// Returns imediatly after the Dispatcher-Task has been created (non-blocking)
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        protected void SyncInvokeBegin(Action method, DispatcherPriority priority = DispatcherPriority.Normal) {
            ThreadingSupport.SyncInvokeBegin(method, priority);
        }

        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread) and blocks until it has completed
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        protected void SyncInvoke(Action method, DispatcherPriority priority = DispatcherPriority.Normal) {
            ThreadingSupport.SyncInvoke(method, priority);
        }


        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread) and returns the Result
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        /// <returns></returns>
        protected T SyncInvoke<T>(Func<T> method, DispatcherPriority priority = DispatcherPriority.Normal) {
            return ThreadingSupport.SyncInvoke<T>(method, priority);
        }

        #endregion
    }
}
