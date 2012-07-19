using System;
using Archimedes.Patterns.MVMV;
using System.ComponentModel;
namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// Represents a viewmodel in the workspace
    /// </summary>
    public interface IWorkspaceViewModel : IViewModelBase
    {

        #region Events

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        event EventHandler RequestClose;

        /// <summary>
        /// Raised when this workspace should gain the focus
        /// </summary>
        event EventHandler RequestFocus;

        /// <summary>
        /// Raised when the focus of this Workspace Element has changed
        /// </summary>
        event EventHandler HasFocusChanged;

        /// <summary>
        /// Raised when the IsOnWorkspace Property has changed
        /// </summary>
        event EventHandler IsOnWorkspaceChanged;


        /// <summary>
        /// Raised when this Element is about to close itself
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Raised when this Element was closed.
        /// </summary>
        event EventHandler Closed;

        #endregion

        void Close();

        /// <summary>
        /// Gets a Command who closes this viewmodel
        /// </summary>
        System.Windows.Input.ICommand CloseCommand { get; }

        /// <summary>
        /// Gets a Command who focus this viewmodel
        /// </summary>
        System.Windows.Input.ICommand FocusCommand { get; }

        /// <summary>
        /// Does this viewmodel has focus
        /// </summary>
        bool HasFocus { get; set; }
        
        /// <summary>
        /// Is this viewmodel displayed on the workspace
        /// </summary>
        bool IsOnWorkspace { get; set; }
        
        /// <summary>
        /// Set the Language used by this viewmodel
        /// </summary>
        System.Windows.Markup.XmlLanguage Language { get; }




        /// <summary>
        /// Occurs when this viewmodel was closed
        /// </summary>
        void OnClosed();

        /// <summary>
        /// Occurs when this viewmodel is about to close
        /// </summary>
        /// <param name="e"></param>
        void OnClosing(System.ComponentModel.CancelEventArgs e);

        /// <summary>
        /// Occurs when 
        /// </summary>
        void OnRequestClose();

    }
}
