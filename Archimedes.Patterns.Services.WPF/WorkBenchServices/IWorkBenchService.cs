using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Generic;
using Archimedes.Services.WPF.WorkBenchServices.MessageBox;
using Archimedes.Patterns.WPF.ViewModels;
using System.Windows.Documents;
using Archimedes.Services.WPF.WindowViewModelMapping;
using Archimedes.Services.WPF.FrameWorkDialogs;

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

        /// <summary>
        /// Raised when the BackgroundWorking State has changed
        /// </summary>
        event EventHandler IsBackgroundWorkingChanged;

        #endregion

        void ActivateMainWindow();
        
        void LoaderView();
        void LoaderView(bool display);


        void ShowRapport(WorkspaceViewModel viewModel, DependencyObject template);

        #region Show Content Methods

        IDDialogResult ShowDialog(IDialog fileDialog, WorkspaceViewModel ownerVM = null);

        #region Show WorkspaceViewModel

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
        void ShowDockedContent(WorkspaceViewModel viewModel);

        /// <summary>
        /// Show the ViewModel as docked Document
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="title"></param>
        void ShowDockedDocument(WorkspaceViewModel viewModel);

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

        #endregion

        #region Show MessageBox

        /// <summary>
        /// Show a common MessageBox Dialoge
        /// </summary>
        /// <param name="message">The message to show</param>
        /// <param name="title">The title of the MessageBox</param>
        /// <param name="type">Type of the Message, which will result in diffrent images displayed to the user</param>
        /// <param name="button"></param>
        /// <returns></returns>
        IDDialogResult MessageBox(string message, string title, MessageBoxType type = MessageBoxType.None, MessageBoxWPFButton button = MessageBoxWPFButton.OK);

        /// <summary>
        /// Show a common MessageBox Dialoge
        /// </summary>
        /// <param name="message">The message to show</param>
        /// <param name="detail">The Detail Message which will be stored in a scrollable message presenter</param>
        /// <param name="title">The title of the MessageBox</param>
        /// <param name="type">Type of the Message, which will result in diffrent images displayed to the user</param>
        /// <param name="button"></param>
        /// <returns></returns>
        IDDialogResult MessageBox(string message, string detail, string title, MessageBoxType type = MessageBoxType.None, MessageBoxWPFButton button = MessageBoxWPFButton.OK);

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Mapping Service
        /// </summary>
        IWindowViewModelMappings MappingService { get; }

        /// <summary>
        /// Returns the standard Dispatcher of the UI
        /// </summary>
        Dispatcher STADispatcher { get; }

        /// <summary>
        /// Get/Set the current Statustext. This is often visualized by Statusbars etc.
        /// </summary>
        string StatusText { get; set; }

        /// <summary>
        /// Indicates that a background action is currently launched
        /// </summary>
        bool IsBackgroundWorking { get; set; }

        //!! Dont expose AvalonDock primitives in this generic interface!

        ///// <summary>
        ///// Returns all hidden Contents
        ///// </summary>
        //IEnumerable<AvalonDock.DockableContent> HiddenContents { get; }

        #endregion


    }
}
