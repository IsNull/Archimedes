using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Very basic command
    /// </summary>
    public interface ICommandSimple
    {
        /// <summary>
        /// Raised when the CanExecute Property has been changed
        /// </summary>
        event EventHandler CanExecuteChanged;

        /// <summary>
        /// Can this command execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CanExecute(object parameter);

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="args"></param>
        void Execute(object args);
    }
}
