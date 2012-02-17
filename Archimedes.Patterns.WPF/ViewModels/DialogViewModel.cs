using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public abstract class DialogViewModel : WorkspaceViewModel, IDialogViewModel
    {
        IDDialogResult _dlgres = IDDialogResult.None;

        /// <summary>
        /// Gets or sets the Dialoge result
        /// </summary>
        public IDDialogResult DialogeResult {
            get { return _dlgres;  }
            set { _dlgres = value;  }
        }
    }
}
