using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;

namespace Archimedes.Patterns.WPF.ViewModels
{
    public static class ViewBuilder
    {
        public static ICollectionView BuildView(IEnumerable enumeration) {

            if (enumeration == null)
                throw new ArgumentNullException("enumeration");


            var clsrc = new CollectionViewSource
                            {
                                Source = enumeration,
                            };
            
            var view = clsrc.View;

            return view;
        }
    }
}
