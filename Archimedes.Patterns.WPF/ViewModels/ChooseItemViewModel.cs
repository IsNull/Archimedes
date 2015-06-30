using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Data;

namespace Archimedes.Patterns.WPF.ViewModels
{

    /// <summary>
    /// Generic ViewModel Class to let the user choose one single Item from a Enumeration of such types
    /// </summary>
    /// <typeparam name="T">ViewModel Type to show</typeparam>
    public class ChooseItemDialogeViewModel<T> : ChooseItemDialogeBase where T:class
    {
        #region Fields

        private T _selectedEntity;
        private readonly Predicate<T> _customTypeSaveFilter;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new CooseDialoge
        /// </summary>
        /// <param name="items">Enumeration to present</param>
        /// <param name="itemFilter">optional Filter</param>
        public ChooseItemDialogeViewModel(IEnumerable<T> items, Predicate<T> itemFilter = null)
            : base(CollectionViewSource.GetDefaultView(items))
        {
            _customTypeSaveFilter = itemFilter;

            if (_customTypeSaveFilter != null)
                Items.Filter = FilterWrapper;
        }

        #endregion

        #region Properties


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

        protected override void ChooseSelectedItem() {
            ChoosenItem = SelectedEntity;
        }

        protected override bool CanChooseSelectedItem {
            get { return SelectedEntity != null; }
        }

        #endregion

        /// <summary>
        /// Wraps a typesave filter to Object-Filter of the View
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool FilterWrapper(object item)
        {
            T other = item as T;
            if (other == null)
                return false;
            return _customTypeSaveFilter(other);
        }

    }
}
