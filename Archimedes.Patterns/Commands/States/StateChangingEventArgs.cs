using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Archimedes.Patterns.Commands.States
{
    public class StateChangingEventArgs<T> : CancelEventArgs
    {
        public StateChangingEventArgs(T desiredState)
            : base() {
            DesiredState = desiredState;
        }
        public StateChangingEventArgs(T desiredState, bool cancel)
            : base(cancel) {
            DesiredState = desiredState;
        }

        public T DesiredState { get; private set; }
    }
}
