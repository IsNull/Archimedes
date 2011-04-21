using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Returns a Command that never can be executed.
    /// </summary>
    public class NotExecutableCommand : CommandUndo
    {
        static readonly NotExecutableCommand _instance = new NotExecutableCommand();

        NotExecutableCommand() { }
        static NotExecutableCommand() { }

        public static NotExecutableCommand Instance {
            get { return _instance; }
        }

        public override void Execute(object args) {
            throw new NotImplementedException();
        }

        public override void UnExecute() {
            throw new NotImplementedException();
        }

        public override void Redo() {
            throw new NotImplementedException();
        }

        public override CommandUndo CloneCommand() {
            throw new NotImplementedException();
        }

        public override bool CanExecute(object args) {
            return false;
        }
    }
}
