using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Archimedes.Patterns.Commands.States
{
    public abstract class StateCommand<T> : IStateCommand<T>
    {
        #region Fields

        T _state;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the State has changed
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Raised when the state attempts to change (Cancelable event)
        /// </summary>
        public event EventHandler<StateChangingEventArgs<T>> StateSwitching;

        /// <summary>
        /// Raised when the CanExecute State has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        /// <summary>
        /// Returns the State of this switchable Command
        /// </summary>
        public T State { 
            get { return _state; }
            set {
                _state = value;
                OnStateChanged();
            }
        }

        public abstract void Execute(object parameter);

        public abstract bool CanExecute(object parameter);

        protected void TryChangeState(T newState) {
            var e = new StateChangingEventArgs<T>(newState);
            if (StateSwitching != null)
                StateSwitching(this, e);

            if (e.Cancel)
                return;

            State = newState;
        }

        /// <summary>
        /// Occured when the State has been switched
        /// </summary>
        public virtual void OnStateChanged() {
            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);
        }

    }



}
