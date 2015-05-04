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
        private readonly ICollectionView _itemPresenter;


        protected ChooseItemDialogeBase(ICollectionView itemPresenter)
        {
            if (itemPresenter == null)
                throw new ArgumentNullException("itemPresenter");
            _itemPresenter = itemPresenter;
        }

        /// <summary>
        /// Items which are chooseable
        /// </summary>
        public ICollectionView Items {
            get { return _itemPresenter; }
        }

        protected override IEnumerable<DialogCommand> BuildCommands()
        {
           yield return BuildDefaultCommand("CANCEL", DialogResultType.Cancel, false, true);

           var chooseCommand = BuildDefaultCommand("CHOOSE", DialogResultType.OK, true);
           chooseCommand.CustomAction = o => ChooseSelectedItem();
           chooseCommand.CustomCanExecute = o => CanChooseSelectedItem;
           yield return chooseCommand;
        }


        protected abstract void ChooseSelectedItem();
        protected abstract bool CanChooseSelectedItem { get; }
    }
}
