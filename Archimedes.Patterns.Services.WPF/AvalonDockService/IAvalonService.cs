using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock;

namespace Archimedes.Services.WPF.AvalonDockService
{
    /// <summary>
    /// Basic Bridge-Service to interact with a Window System (AvalonDock)
    /// </summary>
    public interface IAvalonService
    {
        #region Events

        /// <summary>
        /// Raised when the PrimaryDockingManager changes :)
        /// </summary>
        event EventHandler PrimaryDockManagerChanged;

        /// <summary>
        /// Raised when the PrimaryDockingmanager is Loaded
        /// </summary>
        event EventHandler PrimaryDockManagerLoaded;

        #endregion

        string LayoutFilePath { get; set; }

        bool AuotLoadLastSessionLayout { get; set; }

        bool IsDefaultLayoutSaved { get; }

        void RestoreLastSessionLayout();

        void RestoreDefaultLayout();

        void SaveSessionLayout();

        void SaveDefaultLayout();

        /// <summary>
        /// Reference to the Root DockManager.
        /// </summary>
        DockingManager PrimaryDockManager { get; set; }

        /// <summary>
        /// Invokes the PrimaryDockManagerLoaded Event
        /// </summary>
        void OnPrimaryDockManagerLoaded();


    }
}
