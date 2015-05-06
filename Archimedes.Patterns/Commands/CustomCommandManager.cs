using System;
using System.Linq;
using Archimedes.Patterns.Container;
using Archimedes.Patterns.Data;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Holds all executed Commands 
    /// </summary>
    [Service]
    public class CustomCommandManager : ICommandManager
    {
        #region Fields

        private readonly ObservableLimitedStack<ICommandUndo> _commandsExecuted = new ObservableLimitedStack<ICommandUndo>();
        private readonly StackLimited<ICommandUndo> _commandsUndone = new StackLimited<ICommandUndo>();

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
                return _commandsExecuted;
            }
        }

        /// <summary>
        /// How many commands are tracked in the History and therefore are undoable?
        /// </summary>
        public uint HistoryLimit {
            get { return _commandsExecuted.Limit; }
            set {
                _commandsExecuted.Limit = value;
            }
        }

        /// <summary>
        /// Can the CommandManager undo something?
        /// </summary>
        public virtual bool CanUndo {
            get { return _commandsExecuted.Any(); }
        }

        /// <summary>
        /// Can the CommandManager redo smething?
        /// </summary>
        public virtual bool CanRedo {
            get { return _commandsUndone.Any(); }
        }

        /// <summary>
        /// Get the next Command which will be undone if UnDo is executed.
        /// </summary>
        public ICommandUndo PreviewUndoCommand {
            get {
                if (_commandsExecuted.Count != 0)
                    return _commandsExecuted.Peek();
                else
                    return null;
            }
        }

        /// <summary>
        /// Get the next Command which will be redone if Redo is executed.
        /// </summary>
        public ICommandUndo PreviewRedoCommand {
            get {
                if (_commandsUndone.Count != 0)
                    return _commandsUndone.Peek();
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
                _commandsExecuted.Push(cmd.CloneCommand());
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

                    var cmd = _commandsExecuted.Pop();
                    cmd.UnExecute();
                    _commandsUndone.Push(cmd);
                    OnCommandsUndoneChanged();

                    if (undo != this.CanUndo)
                        OnCanUndoChanged();
                    if (redo != this.CanRedo)
                        OnCanRedoChanged();
                }
#if !DEBUG
            } catch (Exception e) {
                //MessageBox.Show(string.Format("Undo failed with \n{0}","Command Error", e.Message),MessageBoxButtons.OK, MessageBoxIcon.Error); 
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

                    var cmd = _commandsUndone.Pop();
                    cmd.Redo();
                    _commandsExecuted.Push(cmd);
                    OnCommandsExecutedChanged();


                    if (undo != this.CanUndo)
                        OnCanUndoChanged();
                    if (redo != this.CanRedo)
                        OnCanRedoChanged();
                }
#if !DEBUG
            } catch (Exception e) {
                //MessageBox.Show(string.Format("Redo failed with:\n{0}",e),
                //    "Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }

        /// <summary>
        /// Clears the whole Command History
        /// </summary>
        public virtual void ClearHistory() {
            _commandsExecuted.ClearStack();
            _commandsUndone.ClearStack();
            OnCanUndoChanged();
            OnCanRedoChanged();
            OnCommandsExecutedChanged();
            OnCommandsUndoneChanged();
        }

        #endregion
    }
}
