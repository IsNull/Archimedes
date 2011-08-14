using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Archimedes.Services.WPF.WorkBenchServices
{
    /// <summary>
    /// Independend Dialoge-Result (abstracts underliing Windows.Forms/WPF/Platform)
    /// Specifies identifiers to indicate the return value of a dialog box.
    /// </summary>
    public enum IDDialogResult
    {
        /// <summary>
        /// Nothing is returned from the dialog box. This means that the modal dialog
        /// continues running.
        /// </summary>
        None = 0,

        /// <summary>
        /// The dialog box return value is OK (usually sent from a button labeled OK).
        /// </summary>
        OK = 1,

        /// <summary>
        /// The dialog box return value is Cancel (usually sent from a button labeled Cancel).
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The dialog box return value is Abort (usually sent from a button labeled  Abort).
        /// </summary>
        Abort = 3,

        /// <summary>
        /// The dialog box return value is Retry (usually sent from a button labeled Retry).
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The dialog box return value is Ignore (usually sent from a button labeled Ignore).
        /// </summary>
        Ignore = 5,

        /// <summary>
        ///  The dialog box return value is Yes (usually sent from a button labeled Yes).
        /// </summary>
        Yes = 6,

   
        /// <summary>
        /// The dialog box return value is No (usually sent from a button labeled No).
        /// </summary>
        No = 7,
    }


    internal static class IDDialogResultConverter
    {

        internal static IDDialogResult From(DialogResult dlgResult) {
            switch(dlgResult) {

                case DialogResult.Abort:
                    return IDDialogResult.Abort;
                case DialogResult.Cancel:
                    return IDDialogResult.Cancel;
                case DialogResult.Ignore:
                    return IDDialogResult.Ignore;
                case DialogResult.No:
                    return IDDialogResult.No;
                case DialogResult.None:
                    return IDDialogResult.None;
                case DialogResult.OK:
                    return IDDialogResult.OK;
                case DialogResult.Retry:
                    return IDDialogResult.Retry;
                case DialogResult.Yes:
                    return IDDialogResult.Yes;

                default:
                    return IDDialogResult.None;
            }
        }
    }

}
