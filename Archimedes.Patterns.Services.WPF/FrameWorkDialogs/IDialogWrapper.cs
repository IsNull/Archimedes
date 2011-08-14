﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Archimedes.Services.WPF.FrameWorkDialogs
{
    internal interface IDialogWrapper : IDisposable
    {
        DialogResult ShowDialog(IWin32Window owner);
    }
}
