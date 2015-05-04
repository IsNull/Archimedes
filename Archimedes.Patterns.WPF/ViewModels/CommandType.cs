using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// The kind of a command in terms of what it does. I.e. accepting , or doing something dangerous.
    /// </summary>
    public enum CommandType
    {
        None,

        /// <summary>
        /// A command which confirms and gets executed by default, i.e. by pressing enter.
        /// </summary>
        DefaultConfirm,

        /// <summary>
        /// A command which does something dangerous, which probably can not get undone easily or at all.
        /// </summary>
        DangerAction,
    }
}
