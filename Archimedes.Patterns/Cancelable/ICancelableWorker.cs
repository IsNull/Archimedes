using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Cancelable
{
    public interface ICancelableWorker
    {
        void Cancel();
        bool CancellationPending { get; }
        bool IsBusy { get; }
    }
}
