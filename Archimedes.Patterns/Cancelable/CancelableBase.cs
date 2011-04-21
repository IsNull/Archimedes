using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Cancelable
{

    /// <summary>
    /// BaseClass for cancelable worker Objects
    /// This class is threadsafe
    /// </summary>
    public abstract class CancelableBase : ICancelableWorker
    {
        #region Fields

        protected bool _isBusy = false;
        protected object _isBusyLock = new object();

        protected bool _cancellationPending = false;
        protected object _cancellationLock = new object();

        protected WorkProgressState _state = WorkProgressState.Unknown;
        protected object _stateLock = new object();

        #endregion

        /// <summary>
        /// Attempts to cancel if currently process is busy
        /// </summary>
        public virtual void Cancel() {
            if (IsBusy) {
                CancellationPending = true;
            }
        }

        /// <summary>
        /// Should the current Processing being canceled?
        /// </summary>
        public virtual bool CancellationPending {
            get { lock (_cancellationLock) { return _cancellationPending; } }
            protected set { lock (_cancellationLock) { _cancellationPending = value; } }
        }

        /// <summary>
        /// Is the current working process busy?
        /// </summary>
        public virtual bool IsBusy {
            get { lock (_isBusyLock) { return _isBusy; } }
            protected set {
                lock (_isBusyLock) {
                    _isBusy = value;
                    if (!_isBusy) {
                        CancellationPending = false;
                    }
                }
            }
        }

        /// <summary>
        /// State of the current work
        /// </summary>
        public virtual WorkProgressState State {
            get { lock (_stateLock) { return _state; } }
            protected set { lock (_stateLock) { _state = value; } }
        }

        protected void InitWorker() {
            CancellationPending = false;
            IsBusy = true;
        }
    }


    public enum WorkProgressState
    {
        Unknown = 0,

        /// <summary>
        /// Indicates that the class has a valid state
        /// </summary>
        Finished = 1,

        /// <summary>
        /// Indicated that the class has an invalid state
        /// (because it was canceled before)
        /// </summary>
        Invalidated = 2,
    }
}
