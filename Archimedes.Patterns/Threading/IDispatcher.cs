using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Threading
{
    /// <summary>
    /// Interface for a generic dispatcher
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Executes the given Method on the desired Dispatcher Thread.
        /// Returns after the Method was executed.
        /// </summary>
        /// <param name="method"></param>
        void Invoke(Action method);

        /// <summary>
        /// Executes the given Method async on the desired Dispatcher Thread.
        /// Returns imideatly after the method has been invoked.
        /// </summary>
        /// <param name="method"></param>
        void InvokeBegin(Action method);


        /// <summary>
        /// Executes the given Method on the desired Dispatcher Thread.
        /// Returns the method result.
        /// </summary>
        /// <typeparam name="T">Returntype</typeparam>
        /// <param name="method"></param>
        T Invoke<T>(Func<T> method);
    }
}
