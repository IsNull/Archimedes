using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// Represents the viewmodel for a dialog in the workspace.
    /// 
    /// The DialogCommands collection specifies which dialog buttons are visible.
    /// By default, there is only a OK command, which closes the dialog.
    /// To adjust the dialog actions, overwrite the <see cref="BuildCommands"/> Method.
    /// </summary>
    public abstract class DialogViewModel : WorkspaceViewModel, IDialogViewModel
    {
        private readonly List<DialogCommand> _dialogCommands = new List<DialogCommand>();
        private DialogResultType _dlgres = DialogResultType.None;
        private bool _commandsUpdated = false;

        protected DialogViewModel()
        {
            
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the Dialoge result
        /// </summary>
        public DialogResultType DialogeResult {
            get { return _dlgres;  }
            set { _dlgres = value;  }
        }


        /// <summary>
        /// Gets all dialog commands. I.e. this ususaly are the OK, YES, NO CANCEL etc. commands.
        /// These are used to build the dialog buttons dynamically
        /// </summary>
        public IEnumerable<DialogCommand> DialogCommands
        {
            get
            {
                if (!_commandsUpdated)
                {
                    _dialogCommands.Clear();
                    _dialogCommands.AddRange(BuildCommands());
                    _commandsUpdated = true;
                }
                return _dialogCommands;
            }
        }

        #endregion


        /// <summary>
        /// Builds the dialog commands.
        /// Sub classes can overwrite this method to yield their own commands.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<DialogCommand> BuildCommands()
        {
            yield return BuildDefaultCommand("OK", DialogResultType.OK, true);
        }


        protected DialogCommand BuildDefaultCommand(string name, DialogResultType cause, bool isAccept = false, bool isCancel = false)
        {
            var command = new DialogCommand(this, name, cause)
            {
                IsDefaultAccept = isAccept,
                IsDefaultCancel = isCancel
            };

            if (isAccept)
            {
                command.CommandType = CommandType.DefaultConfirm;
            }

            return command;
        }

    }
}
