using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Archimedes.Patterns.Data;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Holds all executed Commands 
    /// </summary>
    public class CustomCommandManager : ICommandManager
    {
        #region Fields

        ObservableLimitedStack<ICommandUndo> commandsExecuted = new ObservableLimitedStack<ICommandUndo>();
        StackLimited<ICommandUndo> commandsUndone = new StackLimited<ICommandUndo>();

        #endregion

        #region Constructor

        public CustomCommandManager() { }

        #endregion

        #region Events

        public event EventHandler CanUndoChanged;
        public event EventHandler CanRedoChanged;

        public event EventHandler CommandsExecutedChanged;
        public event EventHandler CommandsUndoneChanged;

        /// <summary>
        /// Raised when a Command is executed throug the CommandManager
        /// </summary>
        public event EventHandler<CommandEventArgs> CommandExecuted;


        public void OnCommandsExecutedChanged() {
            if (CommandsExecutedChanged != null)
                CommandsExecutedChanged(this, new EventArgs());
        }

        public void OnCommandsUndoneChanged() {
            if (CommandsUndoneChanged != null)
                CommandsUndoneChanged(this, new EventArgs());
        }

        public void OnCanUndoChanged(){
            if (CanUndoChanged != null)
                CanUndoChanged(this, new EventArgs());
        }
        public void OnCanRedoChanged(){
            if (CanRedoChanged != null)
                CanRedoChanged(this, new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the History of all Executed Commands.
        /// </summary>
        public ObservableLimitedStack<ICommandUndo> History {
            get {
                return commandsExecuted;
            }
        }

        /// <summary>
        /// How many commands are tracked in the History and therefore are undoable?
        /// </summary>
        public uint HistoryLimit {
            get { return commandsExecuted.Limit; }
            set {
                commandsExecuted.Limit = value;
            }
        }

        /// <summary>
        /// Can the CommandManager undo something?
        /// </summary>
        public virtual bool CanUndo {
            get { return commandsExecuted.Any(); }
        }

        /// <summary>
        /// Can the CommandManager redo smething?
        /// </summary>
        public virtual bool CanRedo {
            get { return commandsUndone.Any(); }
        }

        /// <summary>
        /// Get the next Command which will be undone if UnDo is executed.
        /// </summary>
        public ICommandUndo PreviewUndoCommand {
            get {
                if (commandsExecuted.Count != 0)
                    return commandsExecuted.Peek();
                else
                    return null;
            }
        }

        /// <summary>
        /// Get the next Command which will be redone if Redo is executed.
        /// </summary>
        public ICommandUndo PreviewRedoCommand {
            get {
                if (commandsUndone.Count != 0)
                    return commandsUndone.Peek();
                else
                    return null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the given command <paramref name="cmd"/> and adds it to the executed commands for allowing undo/redo support.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="commandArg"></param>
        public virtual void Execute(ICommandUndo cmd, object commandArg = null) {
            var undo = this.CanUndo;
            var redo = this.CanRedo;

            cmd.Execute(commandArg);
            if (CommandExecuted != null) {
                CommandExecuted(this, new CommandEventArgs(cmd));
            }

            if (!cmd.IsTransparentCommand) {
                commandsExecuted.Push(cmd.CloneCommand());
                OnCommandsExecutedChanged();
            }

            if (undo != this.CanUndo)
                OnCanUndoChanged();
            if (redo != this.CanRedo)
                OnCanRedoChanged();
        }

        /// <summary>
        /// Undo the last Command executed - if any.
        /// </summary>
        public virtual void Undo() {
#if !DEBUG
            try {
#endif
                if (this.CanUndo) {

                    var undo = this.CanUndo;
                    var redo = this.CanRedo;

                    var cmd = commandsExecuted.Pop();
                    cmd.UnExecute();
                    commandsUndone.Push(cmd);
                    OnCommandsUndoneChanged();

                    if (undo != this.CanUndo)
                        OnCanUndoChanged();
                    if (redo != this.CanRedo)
                        OnCanRedoChanged();
                }
#if !DEBUG
            } catch (Exception e) {
                MessageBox.Show("Fehler beim rückgängig machen der letzten Aktion!","RIEDOMaps Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        /// <summary>
        /// Redo the last exectued command - if any.
        /// </summary>
        public virtual void Redo() {
#if !DEBUG
            try {
#endif
                if (this.CanRedo) {

                    var undo = this.CanUndo;
                    var redo = this.CanRedo;

                    var cmd = commandsUndone.Pop();
                    cmd.Redo();
                    commandsExecuted.Push(cmd);
                    OnCommandsExecutedChanged();


                    if (undo != this.CanUndo)
                        OnCanUndoChanged();
                    if (redo != this.CanRedo)
                        OnCanRedoChanged();
                }
#if !DEBUG
            } catch (Exception e) {
                MessageBox.Show(string.Format("Fehler beim wiederherstellen der letzten, rückgängig gmachten Aktion!\n{0}",e),
                    "RIEDOMaps Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        /// <summary>
        /// Clears the whole Command History
        /// </summary>
        public virtual void ClearHistory() {
            commandsExecuted.ClearStack();
            commandsUndone.ClearStack();
            OnCanUndoChanged();
            OnCanRedoChanged();
            OnCommandsExecutedChanged();
            OnCommandsUndoneChanged();
        }

        #endregion
    }
}
