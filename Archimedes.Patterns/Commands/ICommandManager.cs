using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Data;

namespace Archimedes.Patterns.Commands
{
    public interface ICommandManager
    {
        /// <summary>
        /// Executes the command and saves a clone in the command history
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="arg"></param>
        void Execute(ICommandUndo cmd, object arg = null);

        /// <summary>
        /// Undos the last executed Command
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last executed Command
        /// </summary>
        void Redo();

        /// <summary>
        /// Clear the Command History
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// Is there any Command to undo?
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Is there any undone command to restore?
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Raised when CanUndo has changed
        /// </summary>
        event EventHandler CanUndoChanged;

        /// <summary>
        /// Raised when CanRedo has changed
        /// </summary>
        event EventHandler CanRedoChanged;

        /// <summary>
        /// Raised when Executed List has changed
        /// </summary>
        event EventHandler CommandsExecutedChanged;

        /// <summary>
        /// Raised when Undone List has changed
        /// </summary>
        event EventHandler CommandsUndoneChanged;

        /// <summary>
        /// Raised when a Command is executed throug the CommandManager
        /// </summary>
        event EventHandler<CommandEventArgs> CommandExecuted;

        /// <summary>
        /// Get the whole Command history
        /// </summary>
        ObservableLimitedStack<ICommandUndo> History { get; }

        ICommandUndo PreviewRedoCommand { get; }
        ICommandUndo PreviewUndoCommand { get; }
    }
}
