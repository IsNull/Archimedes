using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Archimedes.Patterns.WPF.Commands;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public abstract class ChooseItemDialogeBase : DialogViewModel
    {
        protected ICollectionView _itemPresenter;


        /// <summary>
        /// Items which are chooseable
        /// </summary>
        public ICollectionView Items {
            get { return _itemPresenter; }
        }


        #region Commands

        public ICommand ChooseSelectedItemCommand {
            get {
                return new RelayCommand(
                    x => ChooseSelectedItemInternal(),
                    x => CanChooseSelectedItem);
            }
        }

        #endregion


        private void ChooseSelectedItemInternal() {
            ChooseSelectedItem();
            DialogeResult = IDDialogResult.OK;
        }

        protected abstract void ChooseSelectedItem();
        protected abstract bool CanChooseSelectedItem { get; }
    }
}
