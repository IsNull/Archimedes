using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Executes multiple Commands at once (Command Group)
    /// </summary>
    public class CompositeUnDoCommand : CommandUndo
    {
        List<CommandUndo> _commands;

        #region Constructors

        /// <summary>
        /// Create a new Composite Command with given sub commands
        /// </summary>
        /// <param name="commands">Commands to exectue as group</param>
        public CompositeUnDoCommand(IEnumerable<CommandUndo> commands) {
            _commands = new List<CommandUndo>(commands);
        }

        public CompositeUnDoCommand(CommandUndo command)
            : this(new CommandUndo[] { command }) { }

        /// <summary>
        /// Creates an empty Composite Command. Use the provided Add Methods to add Commands.
        /// </summary>
        public CompositeUnDoCommand()
            : this(new CommandUndo[] { }) { }

        #endregion

        /// <summary>
        /// Adds a SubCommand at the end of the composite command flow
        /// </summary>
        /// <param name="command"></param>
        public void Add(CommandUndo command) {
            _commands.Add(command);
        }

        #region CommandUndo

        public override void Execute(object args) {
            foreach (var cmd in _commands)
                cmd.Execute(args);
            base.Execute(args);
        }


        public override void UnExecute() {
            var reversedCmds = new List<CommandUndo>(_commands);
            reversedCmds.Reverse();
            foreach (var cmd in reversedCmds)
                cmd.UnExecute();
        }

        public override void Redo() {
            foreach (var cmd in _commands)
                cmd.Redo();
        }

        public override CommandUndo CloneCommand() {

            List<CommandUndo> cloneCommands = new List<CommandUndo>();
            foreach (var cmd in _commands)
                cloneCommands.Add(cmd.Clone() as CommandUndo);

            return new CompositeUnDoCommand(cloneCommands);
        }

        public override bool CanExecute(object args) {
            foreach (var cmd in _commands)
                if (!cmd.CanExecute(args))
                    return false;

            return true;
        }




        #endregion

        public override string Name {
            get {    
                if (_commands.Any()) {
                    string name = "";
                    foreach (var c in _commands)
                        name += c.Name + ", ";
                    return name.TrimEnd(new char[] {',' , ' '});
                } else
                    return base.Name;
            }
        }
        public override string Description {
            get {
                return CreateDescription();
            }
            set {
                base.Description = value;
            }
        }

        string CreateDescription() {
            string desc = "";
            foreach (var c in _commands) {
                desc += c.Description + ", ";
            }
            return desc;
        }

    }
}
