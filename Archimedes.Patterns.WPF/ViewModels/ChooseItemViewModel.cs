using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using Archimedes.Patterns.WPF.Commands;

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// Generic ViewModel Class to let the user choose one single Item from a Enumeration of such types
    /// </summary>
    /// <typeparam name="T">ViewModel Type to show</typeparam>
    public class ChooseItemDialogeViewModel<T> : WorkspaceViewModel where T : class
    {
        #region Fields

        T _selectedEntity;
        ICollectionView _itemPresenter;
        Predicate<T> _customTypeSaveFilter;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new CooseDialoge
        /// </summary>
        /// <param name="items">Enumeration to present</param>
        /// <param name="itemFilter">optional Filter</param>
        public ChooseItemDialogeViewModel(IEnumerable<T> items, Predicate<T> itemFilter = null) {
            if (items == null)
                throw new ArgumentNullException("items");

            _itemPresenter = ViewBuilder.BuildView(items);
            _customTypeSaveFilter = itemFilter;

            if (_customTypeSaveFilter != null)
                _itemPresenter.Filter = FilterWrapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Items which are chooseable
        /// </summary>
        public ICollectionView Items {
            get { return _itemPresenter; }
        }

        /// <summary>
        /// The choosen item. This is null if the user didn't choose any.
        /// </summary>
        public T ChoosenItem {
            get;
            private set;
        }

        /// <summary>
        /// The currently selected item
        /// </summary>
        public T SelectedEntity {
            get { return _selectedEntity; }
            set {
                _selectedEntity = value;
                OnPropertyChanged(() => SelectedEntity);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion

        #region Commands

        public ICommand ChooseSelectedItemCommand {
            get {
                return new RelayCommand(
                    x => ChooseSelectedItem(),
                    x => CanChooseSelectedItem);
            }
        }

        void ChooseSelectedItem() {
            ChoosenItem = SelectedEntity;
            this.CloseCommand.Execute(null);
        }

        bool CanChooseSelectedItem {
            get { return SelectedEntity != null; }
        }

        #endregion

        /// <summary>
        /// Wraps a typesave filter to Object-Filter of the View
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool FilterWrapper(object item) {
            T other = item as T;
            if (other == null)
                return false;
            return _customTypeSaveFilter(other);
        }

    }
}
