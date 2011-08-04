using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Archimedes.Patterns.MVMV;
using Archimedes.Patterns.WPF.Commands;
using System.Linq.Expressions;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows;

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

        #region Sync Dispatcher

        /// <summary>
        /// Returns the Dispatcher of the default GUI Thread
        /// </summary>
        public Dispatcher DefaultDispatcher {
            get { return Application.Current.Dispatcher; } //Application.Current.Dispatcher.Invoke
        }

        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread)
        /// </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        public void SyncInvoke(Action method, DispatcherPriority priority = DispatcherPriority.Normal) {
            Application.Current.Dispatcher.Invoke(method, priority);
        }

        /// <summary>
        /// Executes a Method in the default Dispatcher (running on the standard GUI Thread) and returns the Result
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="method">Method to execute</param>
        /// <param name="priority">Dispatcher Priority</param>
        /// <returns></returns>
        public T SyncInvoke<T>(Func<T> method, DispatcherPriority priority = DispatcherPriority.Normal) {
            return (T)Application.Current.Dispatcher.Invoke(method, priority);
        }

        #endregion

        /// <summary>
        /// Updates the given Property identified by Expression
        /// </summary>
        /// <param name="expressions"></param>
        public virtual void UpdateProperty(params Expression<Func<object>>[] expressions) {
            OnPropertyChanged(expressions);
        }

        public static XmlLanguage DefaultLanguage = XmlLanguage.Empty;
        public virtual XmlLanguage Language {
            get { return DefaultLanguage; }
        }

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
        #endregion // CloseCommand

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
        }
        #endregion // FocusCommand

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
