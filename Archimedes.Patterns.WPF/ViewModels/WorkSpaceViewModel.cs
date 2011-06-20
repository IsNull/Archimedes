using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Archimedes.Patterns.MVMV;
using Archimedes.Patterns.WPF.Commands;
using System.Linq.Expressions;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// This ViewModelBase subclass has basic UI handlings.
    /// It requests to be removed  from the UI when its CloseCommand executes.
    /// 
    /// This class is abstract.
    /// </summary>
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;
        RelayCommand _focusCommand;

        #endregion // Fields

        #region Events

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Raised when this workspace should gain the focus
        /// </summary>
        public event EventHandler RequestFocus;


        #endregion

        #region Constructor

        protected WorkspaceViewModel() { }

        #endregion // Constructor

        /// <summary>
        /// Updates the given Property identified by Expression
        /// </summary>
        /// <param name="expressions"></param>
        public virtual void UpdateProperty(params Expression<Func<object>>[] expressions) {
            OnPropertyChanged(expressions);
        }

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand {
            get {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(x => OnRequestClose());

                return _closeCommand;
            }
        }

        
        public ICommand FocusCommand {
            get {
                if (_focusCommand == null) {
                    _focusCommand = new RelayCommand(x => OnRequestFocus());
                }
                return _focusCommand;
            }
        }


        #endregion // CloseCommand

        #region Event Invokers

        public void OnRequestClose() {
            if (RequestClose != null)
                RequestClose(this, EventArgs.Empty);
        }

        void OnRequestFocus() {
            if (RequestFocus != null)
                RequestFocus(this, EventArgs.Empty);
        }

        #endregion 
    }
}
