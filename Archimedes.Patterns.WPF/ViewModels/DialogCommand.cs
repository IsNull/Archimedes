using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Archimedes.Patterns.WPF.Commands;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// Represents a dialog command - usually the YES, NO, ACCEPT, CANCEL etc. buttons are each such a command
    /// </summary>
    public class DialogCommand : ICommand
    {
        private readonly DialogViewModel _owner;
        private bool _isDefaultAccept;

        #region Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Constructor


        public DialogCommand(DialogViewModel owner, string name, DialogResultType cause)
        {
            _owner = owner;
            Name = name;
            Cause = cause;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Display text of the command (usualy the button name)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The dialog result which is set automatically if this command has been executed
        /// </summary>
        public DialogResultType Cause { get; set; }

        /// <summary>
        /// Gets/Sets what kind of a command this is. 
        /// Depending on the actual dialg system, this probably has influence on the apearance of the button.
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Set if this is the default accept command.
        /// (This button is usually colored specally, and can be triggered by hitting Enter)
        /// </summary>
        public bool IsDefaultAccept
        {
            get { return _isDefaultAccept; }
            set
            {
                _isDefaultAccept = value;
            }
        }


        /// <summary>
        /// Set if this is the default cancel command.
        /// (This button can usually be triggered by hitting Esc)
        /// </summary>
        public bool IsDefaultCancel { get; set; }

        #endregion

        /// <summary>
        /// If set, executes this action when the command executes.
        /// </summary>
        public Action<object> CustomAction { get; set; }

        /// <summary>
        /// If set, uses this predicate to determine if this command can execute
        /// </summary>
        public Predicate<object> CustomCanExecute { get; set; }


        public virtual void Execute(object parameter)
        {
            if (CustomAction != null) CustomAction(parameter);
            CloseOwner(Cause);
        }

        public virtual bool CanExecute(object parameter)
        {
            return CustomCanExecute == null || CustomCanExecute(parameter);
        }

        /// <summary>
        /// Closes the owning dialog and sets the dialog result to the given cause
        /// </summary>
        /// <param name="cause"></param>
        protected void CloseOwner(DialogResultType cause)
        {
            _owner.DialogeResult = cause;
            _owner.Close();
        }

        public override string ToString()
        {
            return this.GetType().Name + "-" + Name;
        }
    }
}
