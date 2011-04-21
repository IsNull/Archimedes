using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public static class ViewBuilder
    {
        public static ICollectionView BuildView(object enumeration) {
            var clsrc = new CollectionViewSource();
            clsrc.Source = enumeration;
            return clsrc.View;
        }
    }
}
