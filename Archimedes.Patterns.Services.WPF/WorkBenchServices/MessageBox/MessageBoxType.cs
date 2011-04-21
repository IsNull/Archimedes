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


    // Summary:
    //     Specifies identifiers to indicate the return value of a dialog box.
    public enum DialogWPFResult
    {
        // Summary:
        //     Nothing is returned from the dialog box. This means that the modal dialog
        //     continues running.
        None = 0,
        //
        // Summary:
        //     The dialog box return value is OK (usually sent from a button labeled OK).
        OK = 1,
        //
        // Summary:
        //     The dialog box return value is Cancel (usually sent from a button labeled
        //     Cancel).
        Cancel = 2,
        //
        // Summary:
        //     The dialog box return value is Abort (usually sent from a button labeled
        //     Abort).
        Abort = 3,
        //
        // Summary:
        //     The dialog box return value is Retry (usually sent from a button labeled
        //     Retry).
        Retry = 4,
        //
        // Summary:
        //     The dialog box return value is Ignore (usually sent from a button labeled
        //     Ignore).
        Ignore = 5,
        //
        // Summary:
        //     The dialog box return value is Yes (usually sent from a button labeled Yes).
        Yes = 6,
        //
        // Summary:
        //     The dialog box return value is No (usually sent from a button labeled No).
        No = 7,
    }
}
