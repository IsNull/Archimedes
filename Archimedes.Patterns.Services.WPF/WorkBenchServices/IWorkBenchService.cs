using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Generic;
using Archimedes.Services.WPF.WorkBenchServices.MessageBox;
using Archimedes.Patterns.WPF.ViewModels;

namespace Archimedes.Services.WPF.WorkBenchServices
{
    /// <summary>
    /// Basic Interface for WorkbenchService
    /// </summary>
    public interface IWorkBenchService
    {
        #region Events

        /// <summary>
        /// Raised when the StatusText has changed
        /// </summary>
        event EventHandler StatusTextChanged;

        #endregion

        void ActivateMainWindow();
        
        void LoaderView();
        void LoaderView(bool display);
        

        #region Show Content Methods

        /// <summary>
        /// Show the ViewModel as modal Dialog window (blocks current thread)
        /// </summary>
        /// <param name="viewModel">VM to show</param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        bool? ShowDialog(WorkspaceViewModel viewModel, System.Windows.SizeToContent sizeToContent = SizeToContent.WidthAndHeight, System.Windows.Size? windowSize = null);
        
        /// <summary>
        /// Show the ViewModel as docked Content
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="title"></param>
        void ShowDockedContent(WorkspaceViewModel viewModel, string title);

        /// <summary>
        /// Show the ViewModel as docked Document
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="title"></param>
        void ShowDockedDocument(WorkspaceViewModel viewModel, string title);

        /// <summary>
        /// Show the ViewModel as floating window which can be redocked to the DockingManager.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        void ShowDockableFloating(WorkspaceViewModel viewModel, System.Windows.SizeToContent sizeToContent = SizeToContent.WidthAndHeight, System.Windows.Size? windowSize = null);
        
        /// <summary>
        /// Show the ViewModel as standard floating Window. (Can't be redocked)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        void ShowFloating(WorkspaceViewModel viewModel, SizeToContent sizeToContent = SizeToContent.WidthAndHeight, Size? windowSize = null);

        /// <summary>
        /// Show a common MessageBox Dialoge
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        //MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);

        /// <summary>
        /// Show a common MessageBox Dialoge
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        DialogWPFResult MessageBox(string message, string title, MessageBoxType type = MessageBoxType.None, MessageBoxWPFButton button = MessageBoxWPFButton.OK);

        #endregion

        #region Properties

        /// <summary>
        /// Returns the standard Dispatcher of the UI
        /// </summary>
        Dispatcher STADispatcher { get; }

        /// <summary>
        /// Get/Set the current Statustext. This is often visualized by Statusbars etc.
        /// </summary>
        string StatusText { get; set; }


        //!! Dont expose AvalonDock primitives in this generic interface!

        ///// <summary>
        ///// Returns all hidden Contents
        ///// </summary>
        //IEnumerable<AvalonDock.DockableContent> HiddenContents { get; }

        #endregion


    }
}
