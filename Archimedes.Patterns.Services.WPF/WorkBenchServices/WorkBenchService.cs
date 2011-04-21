

namespace Archimedes.Services.WPF.WorkBenchServices
{

    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using AvalonDock;
    using System.Collections.Generic;
    using Archimedes.Services.WPF.WorkBenchServices.MessageBox;
    using Archimedes.Services.WPF.AvalonDockService;
    using Archimedes.Services.WPF.WindowViewModelMapping;
    using Archimedes.Patterns.Services;
    using Archimedes.Patterns.WPF.ViewModels;
    using Archimedes.Services.WPF.WorkBenchServices.Loader;

    /// <summary>
    /// Implements the workbenchservice backing AvalonDock
    /// </summary>
    public class AvalonWorkBenchService : IWorkBenchService
    {
        #region Fields

        IWindowViewModelMappings _mappingService = ServiceLocator.Instance.Resolve<IWindowViewModelMappings>();
        IAvalonService _avalonService = ServiceLocator.Instance.Resolve<IAvalonService>();
        DockingManager _dockManager = null;
        string _statusBarText = "";

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Statusbar Text has changed
        /// </summary>
        public event EventHandler StatusTextChanged;

        #endregion

        #region Constructor

        public AvalonWorkBenchService() {

            _avalonService.PrimaryDockManagerChanged += OnPrimaryDockManagerChanged;

            if (_avalonService.PrimaryDockManager != null)
                DockManager = _avalonService.PrimaryDockManager;
        }

        #endregion

        #region IWorkBenchService

        #region Show VM Content Methods

        /// <summary>
        /// Show the VM in a dockable floating window
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        public void ShowDockableFloating(WorkspaceViewModel viewModel, SizeToContent sizeToContent = SizeToContent.WidthAndHeight, Size? windowSize = null) {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            var content = SetupFloatingContent(viewModel, sizeToContent, windowSize);
            content.ShowAsFloatingWindow(DockManager, false);
        }

        /// <summary>
        /// Show the VM in a as standard UNdockable floating window
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        public void ShowFloating(WorkspaceViewModel viewModel, SizeToContent sizeToContent = SizeToContent.WidthAndHeight, Size? windowSize = null) {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            var content = SetupFloatingContent(viewModel, sizeToContent, windowSize);
            content.ShowAsUndockableFloatingWindow(DockManager);
        }





        /// <summary>
        /// Show the VM as modal Dialog window (blocks current thread)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="sizeToContent"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public bool? ShowDialog(WorkspaceViewModel viewModel, SizeToContent sizeToContent = SizeToContent.WidthAndHeight, Size? windowSize = null) {
            if(viewModel == null)
                throw new ArgumentNullException("viewModel");

            var dockableContent = SetUpDockableContent(viewModel, viewModel.DisplayName);
            dockableContent.DockableStyle = DockableStyle.None;
            dockableContent.FloatingWindowSizeToContent = sizeToContent;
            dockableContent.HideOnClose = false;
            dockableContent.Closing += (s, e) => viewModel.OnRequestClose();
            if (windowSize.HasValue) {
                dockableContent.FloatingWindowSize = windowSize.Value;
            }
            var ret = dockableContent.ShowAsDialoge(DockManager);

            return ret;
        }


        public void ShowDockedContent(WorkspaceViewModel viewModel, string title) {
            if(viewModel == null)
                throw new ArgumentNullException("viewModel");
            var dockableContent = SetUpDockableContent(viewModel, title);
            dockableContent.Show(DockManager);
        }

        public void ShowDockedDocument(WorkspaceViewModel viewModel, string title) {
            if(viewModel == null)
                throw new ArgumentNullException("viewModel");
            var dockableContent = SetUpDockableContent(viewModel, title);
            dockableContent.ShowAsDocument(DockManager);
        }

        #endregion

        #region MessageBox 

        /// <summary>
        /// Displays a simple MessageBox
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public DialogWPFResult MessageBox(string message, string title, MessageBoxType type = MessageBoxType.None, MessageBoxWPFButton button = MessageBoxWPFButton.OK) {
            var vm = new MessageBoxViewModel(message, button);
            vm.MessageBoxImage = type;
            var view = vm.BuildView();

            var dc = CreateDockableContent(title, view, vm);
            dc.DockableStyle = AvalonDock.DockableStyle.Floating;
            dc.FloatingWindowSizeToContent = SizeToContent.WidthAndHeight;
            dc.HideOnClose = false;
            dc.Closing += (s, e) => vm.OnRequestClose();
            dc.ShowAsDialoge(DockManager);

            return vm.DialogeResult;
        }

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="messageBoxText">A string that specifies the text to display.</param>
        /// <param name="caption">A string that specifies the title bar caption to display.</param>
        /// <param name="button">
        /// A MessageBoxButton value that specifies which button or buttons to display.
        /// </param>
        /// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>
        /// A MessageBoxResult value that specifies which message box button is clicked by the user.
        /// </returns>
        [Obsolete("Use MessageBox")]
        public MessageBoxResult ShowMessageBox(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon) {
            return System.Windows.MessageBox.Show(Window.GetWindow(DockManager), messageBoxText, caption, button, icon);
        }
        #endregion

        #region Loader

        DockableContent _dcLoaderView;

        /// <summary>
        /// Shows the loaderView
        /// </summary>
        public void LoaderView(){
            LoaderView(true);
        }

        /// <summary>
        /// Shows/Hides the LoaderView
        /// </summary>
        /// <param name="display">True means show, false hides the Loader</param>
        public void LoaderView(bool display) {

            if (_dcLoaderView == null && display) {
                var loaderView = new LoadingAnimation() { Width = 100, Height = 100 };
                var vm = new LoaderViewModel();
                _dcLoaderView = CreateDockableContent("Bitte Warten", loaderView, vm);
                _dcLoaderView.DockableStyle = DockableStyle.Floating;
                _dcLoaderView.FloatingWindowSizeToContent = SizeToContent.WidthAndHeight;
                //_dcLoaderView.IsCloseable = false;
                _dcLoaderView.HideOnClose = true;
            }

            if (display) {
                _dcLoaderView.ShowAsFloatingWindow(DockManager, false);
            } else {
                if (_dcLoaderView != null)
                    _dcLoaderView.Close();
            }
        }

        #endregion

        /// <summary>
        /// Makes the Mainwindow active
        /// </summary>
        public void ActivateMainWindow() {
            var wnd = Window.GetWindow(DockManager);
            if (wnd != null)
                wnd.Activate();
        }

        /// <summary>
        /// Returns the standard Thread dispatcher of the UI
        /// </summary>
        public Dispatcher STADispatcher {
            get {
                if (DockManager == null)
                    throw new NotSupportedException("Not Ready!");
                return DockManager.Dispatcher;
            }
        }


        public string StatusText {
            get {
                return _statusBarText;
            }
            set {
                _statusBarText = value;
                if (StatusTextChanged != null)
                    StatusTextChanged(this, EventArgs.Empty);
            }
        }

        AvalonDock.DockingManager DockManager {
            get { return _dockManager; }
            set {
                _dockManager = value;
            }
        }

        public IEnumerable<AvalonDock.DockableContent> HiddenContents {
            get {
                return _dockManager.DockableContents.ToList().FindAll(x => x.State == AvalonDock.DockableContentState.Hidden);
            }
        }
        
        #endregion

        #region Helper Methods

        DockableContent SetupFloatingContent(WorkspaceViewModel viewModel, SizeToContent sizeToContent = SizeToContent.WidthAndHeight, Size? windowSize = null) {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            var dockableContent = SetUpDockableContent(viewModel, viewModel.DisplayName);
            dockableContent.DockableStyle = DockableStyle.Floating;
            dockableContent.FloatingWindowSizeToContent = sizeToContent;
            dockableContent.HideOnClose = false;
            dockableContent.Closing += (s, e) => viewModel.OnRequestClose();


            if (windowSize.HasValue) {
                dockableContent.FloatingWindowSize = windowSize.Value;
            }
            return dockableContent;
        }

        DockableContent SetUpDockableContent(WorkspaceViewModel viewModel, string title) {

            var dockableContent = FindContentByViewModel(viewModel) as DockableContent;
            if (dockableContent != null)
                return dockableContent;

            // Create New Content
            var viewType = _mappingService.GetViewTypeFromViewModelType(viewModel.GetType());
            var view = (FrameworkElement)Activator.CreateInstance(viewType);

            dockableContent = CreateDockableContent(title, view, viewModel);
            return dockableContent;
        }

        DockableContent CreateDockableContent(string title, FrameworkElement view, WorkspaceViewModel viewModel) {
            var dockableContent = new DockableContent()
            {
                Name = "myNewDialoge",
                Title = title
            };
            dockableContent.Content = view;
            dockableContent.DataContext = viewModel;
            viewModel.RequestClose += (s, e) => CloseParent(view, viewModel);
            return dockableContent;
        }

        ManagedContent FindContentByViewModel(WorkspaceViewModel vm) {
            ManagedContent content = null;
            content = DockManager.DockableContents.ToList().Find(x => ReferenceEquals(x.Content, vm));
            if (content != null)
                return content;

            content = DockManager.Documents.ToList().Find(x => ReferenceEquals(x.Content, vm));
            if (content != null)
                return content;

            foreach (var floatingWnd in DockManager.FloatingWindows) {
                var floatingPane = floatingWnd.Content as FloatingDockablePane;
                foreach (ManagedContent managedContent in floatingPane.Items) {
                    if (ReferenceEquals((managedContent.Content as FrameworkElement).DataContext, vm)) {
                        return managedContent;
                    }
                }
            }

            return content;
        }


        void CloseParent(FrameworkElement element, WorkspaceViewModel vm) {
            var window = Window.GetWindow(element);
            if(window != null){
                window.Closed += (a, b) => CleanUp(element,vm, window);
                try {
                    window.Close();
                } catch {
                    //
                }
            }
            ActivateMainWindow();
        }

        void CleanUp(FrameworkElement element, WorkspaceViewModel vm, Window window) {
            vm.RequestClose -= (s, e) => CloseParent(element, vm);
            window.Closed -= (a, b) => CleanUp(element, vm, window);
        }



        //void UpdateHiddenContent(){
        //    if (DockManager != null) {
        //        var list = DockManager.DockableContents.Where(dc => dc.State == DockableContentState.Hidden).ToList();
        //        HiddenContents.Clear();
        //        foreach (var dc in list)
        //            HiddenContents.Add(dc);
        //    }
        //}
        #endregion

        #region Event Handlers

        void OnPrimaryDockManagerChanged(object sender, EventArgs e) {
            DockManager = _avalonService.PrimaryDockManager;
        }

        #endregion
    }
}
