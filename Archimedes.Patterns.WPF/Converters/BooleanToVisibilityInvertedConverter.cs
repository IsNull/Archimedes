using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Archimedes.Patterns.WPF.Converters
{


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityInvertedConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool bb;
            try {
                bb = (bool)value;
            } catch {
                bb = false;
            }
            return bb ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
