using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Archimedes.Patterns.WPF.ViewModels.Trees
{
    public interface ITreeViewItemViewModel<T> : INotifyPropertyChanged
    {
        ObservableCollection<T> Children { get; }
        bool HasDummyChild { get; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        T Parent { get; }
    }
}
