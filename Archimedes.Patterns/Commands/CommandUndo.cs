using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace Archimedes.Patterns.Commands
{
    /// <summary>
    /// Represents a Command which can be undone/redone
    /// </summary>
    public abstract class CommandUndo : ICommandUndo
    {
        #region Fields

        string _name = "";
        string _description = "No Description given.";
        bool _isExecuted = false;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Can Execute state changes
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raised when this Command was executed
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> Executed;

        #endregion



        /// <summary>
        /// Returns an Empty, not executable dummy Command
        /// </summary>
        public static ICommandUndo Empty {
            get {
                return NotExecutableCommand.Instance;;
            }
        }

        public virtual void Execute(object args) {
            IsExecuted = true;
            Debug.Print("Executed: " + this.GetType().Name);
            if (Executed != null)
                Executed(this, null);
        }

        public virtual bool CanExecute(object args) {
            return true;
        }

        public virtual void UnExecute() {
            Debug.Print("Undone: " + this.GetType().Name);
        }

        public virtual void Redo() {
            Debug.Print("ReDone: " + this.GetType().Name);
        }


        protected void TriggerCanExecuteChanged(){
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, null);
            }
        }


        /// <summary>
        /// Dont add this Command to the History?
        /// This is generally used when a Command self calls other Commands and exectues them on the CommandContext.
        /// </summary>
        public virtual bool IsTransparentCommand { get { return false; } }

        /// <summary>
        /// Was this Command already Executed?
        /// </summary>
        public bool IsExecuted {
            get { return _isExecuted; }
            private set { _isExecuted = value; }
        }

        /// <summary>
        /// Command Name
        /// </summary>
        public virtual string Name {
            get { return _name == "" ? this.GetType().Name : _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Command Description
        /// </summary>
        public virtual string Description {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Clones this command (save Executed state)
        /// </summary>
        /// <returns></returns>
        public abstract CommandUndo CloneCommand();

        public object Clone() {
            return this.CloneCommand();
        }
    }
}
