using System;

namespace Archimedes.Patterns.Commands.States
{
    public interface IStateCommand<T> : ICommandSimple
    {
        void OnStateChanged();
        T State { get; set; }
        event EventHandler StateChanged;
        event EventHandler<StateChangingEventArgs<T>> StateSwitching;
    }
}
