using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Services.WPF.Notifications
{

    public static class ProgressMonitor
    {

        /// <summary>
        /// Specifies an indeterminate progress
        /// </summary>
        public const int ProgressIndeterminate = -1;

        /// <summary>
        /// Reports the current progress of a task
        /// </summary>
        /// <param name="currentTask">A description of the current task</param>
        /// <param name="progress">The actuall progress (0-100) or ProgressIndeterminate.
        ///  ProgressIndeterminate := -1 </param>
        public delegate void ReportProgress(string currentTask, int progress = ProgressIndeterminate);
    }
}
