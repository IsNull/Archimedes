using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Services.WPF.FrameWorkDialogs
{
    /// <summary>
    /// Interface describing the FolderBrowserDialog.
    /// </summary>
    public class FolderBrowserDialogViewModel : IFolderBrowserDialog
    {
        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        public string SelectedPath { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the New Folder button appears in the folder browser
        /// dialog box.
        /// </summary>
        public bool ShowNewFolderButton { get; set; }
    }
}
