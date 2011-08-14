using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Services.WPF.WorkBenchServices.MessageBox
{

    public enum MessageBoxType
    {
        /// <summary>
        /// No icon is displayed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The message box displays an error icon.
        /// </summary>
        Error = 16,

        /// <summary>
        /// The message box displays a question mark icon.
        /// </summary>
        Question = 32,

        /// <summary>
        /// The message box displays a warning icon.
        /// </summary>
        Warning = 48,

        /// <summary>
        /// The message box displays an information icon.
        /// </summary>
        Information = 64,

        /// <summary>
        /// The message box displays an Ok icon.
        /// </summary>
        Ok
    }

    public enum MessageBoxWPFButton
    {
        /// <summary>
        /// The message box displays an OK button.
        /// </summary>
        OK = 0,

        /// <summary>
        /// The message box displays OK and Cancel buttons.
        /// </summary>
        OKCancel = 1,

        /// <summary>
        /// The message box displays Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        /// The message box displays Yes and No buttons.
        /// </summary>
        YesNo = 4,
    }



}
