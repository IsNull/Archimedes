using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Command which supports undo/redo
    /// </summary>
    public interface ICommandUndo : ICommandSimple, ICloneable
    {
        /// <summary>
        /// Raised when the Command was Executed
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> Executed;

        /// <summary>
        /// Dont add this Command to the History?
        /// This is generally used when a Command self calls other Commands and exectues them on the CommandContext.
        /// </summary>
        bool IsTransparentCommand { get; }

        /// <summary>
        /// Was this Command already Executed?
        /// </summary>
        bool IsExecuted { get; }

        void UnExecute();

        void Redo();

        string Name { get; }

        string Description { get; }

        /// <summary>
        /// Deep Clone this command. 
        /// This is used to store the current command state in the history
        /// </summary>
        /// <returns></returns>
        CommandUndo CloneCommand();
    }

}
