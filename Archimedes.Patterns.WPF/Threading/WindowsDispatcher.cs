using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Threading;
using System.Windows.Threading;

namespace Archimedes.Patterns.WPF.Threading
{
    /// <summary>
    /// Wrapps a Dispatcher to the IDispatcher Interface
    /// </summary>
    public class WindowsDispatcher : IDispatcher
    {
        Dispatcher _windowsDispatcher;
        public WindowsDispatcher(Dispatcher windowsDispatcher) {
            _windowsDispatcher = windowsDispatcher;
        }

        public void Invoke(Action method) {
            if (_windowsDispatcher.CheckAccess())
                method();
            else
                _windowsDispatcher.Invoke(method);
        }

        public void InvokeBegin(Action method) {
            if (_windowsDispatcher.CheckAccess())
                method();
            else
                _windowsDispatcher.BeginInvoke(method);
        }

        public T Invoke<T>(Func<T> method) {
            if (_windowsDispatcher.CheckAccess())
                return method();
            else
            return (T)_windowsDispatcher.Invoke(method);
        }
    }
}
