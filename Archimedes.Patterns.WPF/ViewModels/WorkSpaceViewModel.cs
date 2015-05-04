using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Archimedes.Patterns.MVMV;
using Archimedes.Patterns.WPF.Commands;
using System.Linq.Expressions;
using System.Windows.Markup;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// This ViewModelBase subclass has basic UI handlings.
    /// It requests to be removed  from the UI when its CloseCommand executes.
    /// 
    /// This class is abstract.
    /// </summary>
    public abstract class WorkspaceViewModel : ViewModelWPF, IWorkspaceViewModel
    {
        #region Fields

        ICommand _closeCommand;
        ICommand _focusCommand;
        bool _hasFocus = false;
        bool _isOnWorkspace = false;
        private bool _isVisible;

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

        /// <summary>
        /// Raised when the focus of this Workspace Element has changed
        /// </summary>
        public event EventHandler HasFocusChanged;

        /// <summary>
        /// Raised when the IsOnWorkspace Property has changed
        /// </summary>
        public event EventHandler IsOnWorkspaceChanged;


        /// <summary>
        /// Raised when this Element is about to close itself
        /// </summary>
        public event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Raised when this Element was closed.
        /// </summary>
        public event EventHandler Closed;

        #endregion

        #region Constructor

        protected WorkspaceViewModel()
        {
            IsVisible = true;
            CanClose = true;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// The id of the content view.
        /// This is primarly used to support persistence of a custom view layout and 
        /// later recovering the layout again.
        /// 
        /// This id must be unique and also be the same after an application restart.
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Determines if the view assigned to this viewmodel can be removed from the workspace by the user.
        /// </summary>
        public bool CanClose { get; set; }

        public static XmlLanguage DefaultLanguage = XmlLanguage.Empty;
        public virtual XmlLanguage Language {
            get { return DefaultLanguage; }
        }

        /// <summary>
        /// Gets/Sets if this Elemnt has currently focus
        /// </summary>
        public bool HasFocus {
            get { return _hasFocus; }
            set { 
                _hasFocus = value;
                OnHasFocusChanged();
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(() => IsVisible);
            }
        }

        /// <summary>
        /// Gets/Sets if this Element currently is on the Workspace
        /// </summary>
        public bool IsOnWorkspace {
            get { return _isOnWorkspace; }
            set { 
                _isOnWorkspace = value;
                OnIsOnWorkspaceChanged();
            }
        }

        #endregion

        #region Close Command

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

        /// <summary>
        /// Invokes the Close-Command of this ViewModel
        /// </summary>
        public void Close(){
            this.CloseCommand.Execute(null);
        }

        #endregion // CloseCommand


        public ICommand FloatCommand { get; set; }

        #region Focus Command

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to focus this workspace in the user interface.
        /// </summary>
        public ICommand FocusCommand {
            get {
                if (_focusCommand == null) {
                    _focusCommand = new RelayCommand(x => OnRequestFocus());
                }
                return _focusCommand;
            }
            set { _focusCommand = value; }
        }
        #endregion // FocusCommand

        #region Event Invokers

        /// <summary>
        /// Occurs when this Element is about to close
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClosing(CancelEventArgs e) {
            if(Closing != null && !e.Cancel)
                Closing(this, e);
        }

        public virtual void OnClosed() {
            if(Closed != null)
                Closed(this, EventArgs.Empty);
        }

        public virtual void OnRequestClose() {
            if (RequestClose != null)
                RequestClose(this, EventArgs.Empty);
        }


        protected virtual void OnIsOnWorkspaceChanged() {
            if(IsOnWorkspaceChanged != null)
                IsOnWorkspaceChanged(this, EventArgs.Empty);
        }

        protected virtual void OnHasFocusChanged() {
            if(HasFocusChanged != null)
                HasFocusChanged(this, EventArgs.Empty);
        }

        protected virtual void OnRequestFocus() {
            if (RequestFocus != null)
                RequestFocus(this, EventArgs.Empty);
        }

        #endregion 
    }
}
