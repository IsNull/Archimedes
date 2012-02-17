using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Archimedes.Patterns.WPF.ViewModels;

namespace Archimedes.Services.WPF.WorkBenchServices
{

    internal static class IDDialogResultConverter
    {

        /// <summary>
        /// Converts a Windows Forms Dialoge Result to a Archimedes IDDialogResult
        /// </summary>
        /// <param name="dlgResult"></param>
        /// <returns></returns>
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
