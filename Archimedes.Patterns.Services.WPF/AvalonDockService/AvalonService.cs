using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AvalonDock;

namespace Archimedes.Services.WPF.AvalonDockService
{
    public class AvalonService : IAvalonService
    {
        #region Fields

        DockingManager _primaryDockManager;
        string _layoutSerializepath;
        string _layoutInitialSettings;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the PrimaryDockingManager changes :)
        /// </summary>
        public event EventHandler PrimaryDockManagerChanged;

        /// <summary>
        /// Raised when the PrimaryDockingmanager is Loaded
        /// </summary>
        public event EventHandler PrimaryDockManagerLoaded;

        #endregion

        #region Constructor

        public AvalonService() {
            // default settings
            AuotLoadLastSessionLayout = true;
            _layoutSerializepath = "layout.xml";
            _layoutInitialSettings = "defaultLayout.xml";
        }

        #endregion

        /// <summary>
        /// Reference to the Root DockManager.
        /// </summary>
        public DockingManager PrimaryDockManager {
            get { return _primaryDockManager; }
            set {
                if (_primaryDockManager != null) {
                    _primaryDockManager.DeserializationCallback -= OnDeserializationCallback;
                }

                _primaryDockManager = value;
                if (PrimaryDockManagerChanged != null)
                    PrimaryDockManagerChanged(this, null);

                if (_primaryDockManager.IsLoaded) {
                    if (AuotLoadLastSessionLayout)
                        RestoreLastSessionLayout();
                } else
                    _primaryDockManager.Loaded += (s, e) => OnPrimaryDockManagerLoaded();

                _primaryDockManager.DeserializationCallback += OnDeserializationCallback;
            }
        }

        /// <summary>
        /// Invokes the PrimaryDockManagerLoaded Event
        /// </summary>
        public void OnPrimaryDockManagerLoaded() {
            if (PrimaryDockManagerLoaded != null)
                PrimaryDockManagerLoaded(this, EventArgs.Empty);

            if (AuotLoadLastSessionLayout)
                RestoreLastSessionLayout();
        }


        public string LayoutFilePath {
            get {
                return _layoutSerializepath;
            }
            set {
                _layoutSerializepath = value;
            }
        }

        public void RestoreLastSessionLayout() {
            try {
                if (File.Exists(LayoutFilePath)) {
                    PrimaryDockManager.RestoreLayout(LayoutFilePath);
                }
            } catch { }
        }

        public void RestoreDefaultLayout() {
            try {
                if (File.Exists(_layoutInitialSettings)) {
                    PrimaryDockManager.RestoreLayout(_layoutInitialSettings);
                }
            } catch { }
        }

        public void SaveSessionLayout() {
            if (PrimaryDockManager != null) {
                PrimaryDockManager.SaveLayout(LayoutFilePath);
            }
        }

        public void SaveDefaultLayout() {
            PrimaryDockManager.SaveLayout(_layoutInitialSettings);
        }
        public bool IsDefaultLayoutSaved {
            get { return File.Exists(_layoutInitialSettings); }
        }

        public bool AuotLoadLastSessionLayout {
            get;
            set;
        }



        /// <summary>
        /// Occurs when the AvalonDock Layout deserializer cant find mathcing window...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDeserializationCallback(object sender, DeserializationCallbackEventArgs e) {
            //asserts that content that docking manager is looking for
            //is mynewContent
            //Debug.Assert(e.Name == "mynewContent");

            //var dockableContent = new DockableContent()
            //{
            //    Name = "mynewContent",
            //    Title = "New Dock Content!"
            //};

            ////returns this new content 
            //e.Content = dockableContent;
        }


    }
}
