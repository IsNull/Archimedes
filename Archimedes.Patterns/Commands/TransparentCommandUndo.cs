using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Command which is transparent for the Executing Context
    /// </summary>
    public abstract class TransparentCommandUndo : ICommandUndo
    {
        bool _isExecuted = false;

        public event EventHandler<System.ComponentModel.PropertyChangedEventArgs> Executed;
        public event EventHandler CanExecuteChanged;


        public abstract void Execute(object args);
        public abstract bool CanExecute(object args);

        public bool IsTransparentCommand {
            get { return true; }
        }

        public bool IsExecuted {
            get { return _isExecuted; }
        }

        [Obsolete("Transparent Commands can't UnExecute! (This is often done elswhere)",true)]
        public void UnExecute() {
            throw new NotImplementedException();
        }

        [Obsolete("Transparent Commands can't ReDo! (This is often done elswhere)", true)]
        public void Redo() {
            throw new NotImplementedException();
        }

        public string Name {
            get { return "Transparent Command. You CANT see me :)"; }
        }

        public string Description {
            get { return "You can't see me too."; }
        }

        [Obsolete("Transparent Commands can't Clone as this is done else where", true)]
        public CommandUndo CloneCommand() {
            throw new NotImplementedException();
        }

        [Obsolete("Transparent Commands can't Clone as this is done else where", true)]
        public object Clone() {
            throw new NotImplementedException();
        }






    }
}
