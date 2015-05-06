using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Archimedes.DI.AOP;
using Archimedes.Patterns.WPF.Commands;
using Archimedes.Patterns.Commands;
using Archimedes.Patterns.Data;
using Archimedes.Patterns.MVMV.ViewModels.PoolCache;

namespace Archimedes.Patterns.WPF.ViewModels.Commands
{
    /// <summary>
    /// UI friendly wrapper of CommandManager
    /// </summary>
    public class CommandManagerViewModel : WorkspaceViewModel, ICacheable
    {
        #region Fields

        private readonly ICommandManager _commandManager;
        private RelayCommand _undoCommand;
        private RelayCommand _redoCommand;

        #endregion

        #region ICacheable

        /// <summary>
        /// Raise this when caching no longer is needed of this VM
        /// </summary>
        public event EventHandler CacheExpired;

        #endregion

        #region Constructor

        [Inject]
        public CommandManagerViewModel(ICommandManager manager) {
            if (manager == null)
                throw new ArgumentNullException("manager");
            _commandManager = manager;

            _commandManager.CanUndoChanged += OnCanUndoChanged;
            _commandManager.CanRedoChanged += OnCanRedoChanged;

            _commandManager.CommandsExecutedChanged += OnHistoryChanged;
            _commandManager.CommandsUndoneChanged += OnHistoryChanged;

        }

        #endregion

        #region Public VM Property Wrapper

        public ObservableLimitedStack<ICommandUndo> History {
            get { return _commandManager.History; }
        }

        /// <summary>
        /// Can the Application undo something?
        /// </summary>
        public virtual bool CanUndo {
            get { return _commandManager.CanUndo; }
        }

        /// <summary>
        /// Can the Application redo something
        /// </summary>
        public virtual bool CanRedo {
            get { return _commandManager.CanRedo; }
        }

        /// <summary>
        /// Get the next Command which will be undone if UnDo is executed.
        /// </summary>
        public ICommandUndo PreviewUndoCommand {
            get {
                return _commandManager.PreviewUndoCommand;
            }
        }

        /// <summary>
        /// Get the next Command which will be redone if Redo is executed.
        /// </summary>
        public ICommandUndo PreviewRedoCommand {
            get {
                return _commandManager.PreviewRedoCommand;
            }
        }

        public string PreviewUndoCommandText {
            get {

                if (PreviewUndoCommand == null)
                    return null;
                return PreviewUndoCommand.Name + " rückgängig machen!";
            }
        }

        public string PreviewRedoCommandText {
            get {
                if (PreviewRedoCommand == null)
                    return null;
                return PreviewRedoCommand.Name + " wiederholen!";
            }
        }

        #endregion

        #region WPF Commands

        /// <summary>
        /// Returns a command that undos last Action
        /// </summary>
        public ICommand UndoCommand {
            get {
                if (_undoCommand == null) {
                    _undoCommand = new RelayCommand(
                        param => _commandManager.Undo(),
                        param => this.CanUndo
                        );
                }
                return _undoCommand;
            }
        }

        /// <summary>
        /// Returns a command that redos last undo Action
        /// </summary>
        public ICommand RedoCommand {
            get {
                if (_redoCommand == null) {
                    _redoCommand = new RelayCommand(
                        param => _commandManager.Redo(),
                        param => this.CanRedo
                        );
                }
                return _redoCommand;
            }
        }


        #endregion

        #region Event Handlers

        void OnCanUndoChanged(object sender, EventArgs e) {
            OnPropertyChanged(() => CanUndo);
        }

        void OnCanRedoChanged(object sender, EventArgs e) {
            OnPropertyChanged(() => CanRedo);
        }

        void OnHistoryChanged(object sender, EventArgs e) {
            OnPropertyChanged(() => PreviewUndoCommand);
            OnPropertyChanged(() => PreviewRedoCommand);
            OnPropertyChanged(() => PreviewUndoCommandText);
            OnPropertyChanged(() => PreviewRedoCommandText);


        }

        #endregion
    }
}
