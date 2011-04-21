using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace Archimedes.Services.WPF.FrameWorkDialogs
{
    /// <summary>
    /// WindowWrapper is an IWin32Window wrapper around a WPF window.
    /// </summary>
    class WindowWrapper : IWin32Window
    {
        /// <summary>
        /// Construct a new wrapper taking a WPF window.
        /// </summary>
        /// <param name="window">The WPF window to wrap.</param>
        public WindowWrapper(Window window) {
            Handle = new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }


        /// <summary>
        /// Gets the handle to the window represented by the implementer.
        /// </summary>
        /// <returns>A handle to the window represented by the implementer.</returns>
        public IntPtr Handle { get; private set; }
    }
}
