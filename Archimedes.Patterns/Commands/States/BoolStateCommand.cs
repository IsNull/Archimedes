using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands.States
{
    public class BoolStateCommand : StateCommand<bool>
    {
        public BoolStateCommand() { }
        public BoolStateCommand(bool initialState) { State = initialState; }

        public override void Execute(object parameter) {
            TryChangeState(!State);
        }

        public override bool CanExecute(object parameter) {
            return true;
        }
    }
}
