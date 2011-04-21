using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Archimedes.Patterns.Commands
{
    public interface ICommandUndo : ICommandSimple, ICloneable
    {
        /// <summary>
        /// Raised when the Command was Executed
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> Executed;

        bool IsTransparentCommand { get; }

        bool IsExecuted { get; }

        void UnExecute();

        void Redo();

        string Name { get; }

        string Description { get; }

        CommandUndo CloneCommand();
    }

}
