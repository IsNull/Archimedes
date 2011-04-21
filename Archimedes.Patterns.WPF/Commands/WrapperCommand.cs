using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;
using Archimedes.Patterns.MVMV;
using Archimedes.Patterns.Commands;

namespace Archimedes.Patterns.WPF.Commands
{

    /// <summary>
    /// Wrapps a UndoCommand to ICommand Interface, Pushes the Command on the Undo/Redo Stack if they get executed
    /// (Bridge)
    /// </summary>
    public class WrapperCommand : NotifyChangedBase, ICommand
    {
        #region Fields

        ICommandSimple _command;
        ICommandManager _context;

        #endregion // Fields

        #region Events

        public event EventHandler Executed;

        #endregion

        #region Constructors


        /// <summary>
        /// Creates a new command. As CommandManger (Context) the global GCommandManger Singleton is used.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public WrapperCommand(ICommandSimple command) {
            if (command == null)
                throw new ArgumentNullException("command");
            _command = command;
        }


        /// <summary>
        /// Creates a new command. As CommandManger (Context) the global GCommandManger Singleton is used.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public WrapperCommand(ICommandUndo command, ICommandManager context) {
            if (command == null)
                throw new ArgumentNullException("command");
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
            _command = command;
        }
        
        #endregion // Constructors

        #region Public Properties

        static ICommand _empty = new EmptyCommand();
        /// <summary>
        /// Returns an Empty Command
        /// </summary>
        public static ICommand Empty {
            get {
                return _empty;
            }
        }

        public string Name {
            get {
                var undo = _command as ICommandUndo;
                if (undo != null)
                    return undo.Name;
                else
                    return "SimpleCommand";
            }
        }

        /// <summary>
        /// The delegate Command to execute
        /// </summary>
        public ICommandSimple Command {
            get { return _command; }
            private set { _command = value; }
        }

        #endregion

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return _command.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }


        public void Execute(object parameter) {

            if (_command is ICommandUndo && _context != null) {
                _context.Execute(_command as ICommandUndo, parameter);
            } else {
                _command.Execute(parameter);
            }
            if (Executed != null)
                Executed(this, null);
        }

        #endregion // ICommand Members

        #region Nested Classes

        /// <summary>
        /// An empty Command who cant execute
        /// </summary>
        internal class EmptyCommand : ICommand
        {
            public bool CanExecute(object parameter) {
                return false;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter) {
                // void
            }
        }

        #endregion
    }
}
